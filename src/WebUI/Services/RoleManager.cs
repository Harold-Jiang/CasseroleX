using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CasseroleX.Application.Common.Caching;
using CasseroleX.Application.Common.Caching.Constants;
using CasseroleX.Application.Common.Interfaces;
using CasseroleX.Application.Roles.Queries;
using CasseroleX.Application.Utils;
using CasseroleX.Domain.Entities.Role;
using CasseroleX.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Services;

public class RoleManager:IRoleManager
{
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public RoleManager(ICacheService cache, IApplicationDbContext context, IMapper mapper)
    {
        _cache = cache;
        _context = context;
        _mapper = mapper;
    }
  
    /// <summary>
    /// 获取管理员所有拥有的权限列表
    /// </summary>
    /// <param name="adminId"></param>
    /// <returns></returns>
    public async Task<HashSet<string>> GetPermissionsAsync(int adminId,CancellationToken cancellationToken = default)
    { 
        //获取缓存
        var permissions = await _cache.GetAsync<HashSet<string>>(string.Format(CacheKeys. ADMIN_ROLEPERMISSIONS_BY_ADMINID_KEY,adminId), cancellationToken);
        if (permissions is not null)
        {
            return permissions;
        }
        //获取管理员角色拥有的权限ID集合
        var (ids,roleIds) = await GetRolePermissionIdsAsync(adminId, cancellationToken);
        if (ids.Count == 0)
        {
            return new HashSet<string>();
        }
        //筛选条件
        Expression<Func<RolePermissions, bool>> where = x => x.Status == Status.normal;
        if (!ids.Contains("*"))
        {
            where = x => x.Status == Status.normal && ids.Contains(x.Id.ToString());
        }
        //读取角色组所有权限规则
        var rolePermissions = await _context.RolePermissions.Where(where).ToArrayAsync(cancellationToken);

        //循环规则，判断结果。
        permissions = new HashSet<string>(); 
        //if (ids.Contains("*"))
        //{
        //    permissions.Add("*");
        //}
        foreach (var rule in rolePermissions)
        {
            //超级管理员无需验证condition
            if (!string.IsNullOrEmpty(rule.Condition) && !ids.Contains("*"))
            {
                //根据condition进行验证
                var model = await _context.Admins.FindAsync(new object?[] { adminId, cancellationToken }, cancellationToken: cancellationToken); //获取用户信息,一维数组
                var user = model?.ObjectToDictionary();
                var nums = 0;
                var condition = rule.Condition?.Replace("&&", "\r\n").Replace("||", "\r\n");
                condition = Regex.Replace(condition??"", @"\{(\w*?)\}", "$1");
                var conditionArr = condition.Split("\r\n");
                foreach (var item in conditionArr)
                {
                    var matches = Regex.Match(item.Trim(), @"^(\w+)\s?([\>\<\=]+)\s?(.*)$");
                    if (matches.Success && user is not null && user.TryGetValue(matches.Groups[1].Value, out object? value) && RegexExtensions.VersionCompare(value.ToString()??"", matches.Groups[3].Value, matches.Groups[2].Value))
                    {
                        nums++;
                    }
                }
                if (conditionArr.Length > 0 && ((rule.Condition?.IndexOf("||") != -1 && nums > 0) || conditionArr.Length == nums))
                {
                    permissions.Add(rule.Name?.ToLower()??"");
                }
            }
            else
            {
                //只要存在就记录
                permissions.Add(rule.Name?.ToLower()??"");
            }
        }
        //登录验证则需要保存规则列表
        await _cache.SetAsync(string.Format(CacheKeys.ADMIN_ROLEPERMISSIONS_BY_ADMINID_KEY, adminId), permissions,29990,cancellationToken);

        return permissions;
    }

    /// <summary>
    /// 获取管理员角色拥有的权限ID集合
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<(List<string>, List<int>)> GetRolePermissionIdsAsync(int adminId, CancellationToken cancellationToken = default)
    {
        var roles = await GetRolesAsync(adminId,cancellationToken); 
        var ids = new List<string>();
        foreach (var r in roles)
        {
            if (r.Rules.Contains("*"))
            {
                ids.Clear();
                ids.Add("*");
                break;
            }
            ids.AddRange(r.Rules.ToIList<string>());
        }
        //角色组Id
        var roleIds = roles.Select(x => x.Id).ToList();
        return (ids.Distinct().ToList(),roleIds);
    }

    /// <summary>
    /// 获取用户的角色组
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<RoleDto>> GetRolesAsync(int adminId, CancellationToken cancellationToken = default)
    {
        return await _context.AdminRoles
            .Include(x => x.Role)
            .Where(x => x.AdminId == adminId)
            .Select(x => x.Role)
            .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
     
    /// <summary>
    /// 取出管理员所拥有权限的分组
    /// </summary> 
    public async Task<List<int>> GetChildrenRoleIds(int adminId, bool withself = false,CancellationToken cancellationToken = default)
    {
        // 管理员所有的角色组 
        var roles = await GetRolesAsync(adminId, cancellationToken);

        var roleIds = roles.Select(x => x.Id).ToList();
        var originRoleIds = new List<int>();
        for (int i = 0; i < roles.Count; i++)
        {
            if (originRoleIds.Contains(roles[i].Pid))
            {
                roleIds.Remove(roles[i].Id);
                roles.Remove(roles[i]);
            }
        }
        // 取出所有角色组
        var allRoles = await _context.Roles
            .Where(x => x.Status == Status.normal)
            .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        var objList = new List<RoleDto>();
        foreach (var role in roles)
        {
            if (role.Rules == "*")
            {
                objList = allRoles;
                break;
            }
            // 取出包含自己的所有子节点
            var childrenList = Tree.GetChildren(allRoles, role.Id, true);
            var obj = Tree.GetTreeArray(childrenList, role.Pid);
            objList.AddRange(Tree.GetTreeList(obj));
        }
        var childrenRoleIds = objList.Select(obj => obj.Id).ToList();
        if (!withself)
        {
            childrenRoleIds = childrenRoleIds.Except(roleIds).ToList();
        }
        return childrenRoleIds;
    }

    /// <summary>
    /// 取出管理员所拥有权限的管理员
    /// </summary> 
    public async Task<List<int>> GetChildrenAdminIds(bool isSuperAdmin,int adminId, bool withself = false, CancellationToken cancellationToken = default)
    {
        List<int> childrenAdminIds = new List<int>();

        if (!isSuperAdmin)
        {
            List<int> roleIds = await GetChildrenRoleIds(adminId, false,cancellationToken);
            List<int> adminIds = await _context.AdminRoles
                .Where(a => roleIds.Contains(a.RoleId))
                .Select(a => a.AdminId)
                .ToListAsync(cancellationToken);

            foreach (var id in adminIds)
            {
                childrenAdminIds.Add(id);
            }
        }
        else
        {
            // 超级管理员拥有所有人的权限
            childrenAdminIds = await _context.Admins.Select(a => a.Id).ToListAsync();
        }

        if (withself)
        {
            if (!childrenAdminIds.Contains(adminId))
            {
                childrenAdminIds.Add(adminId);
            }
        }
        else
        {
            childrenAdminIds.Remove(adminId);
        }

        return childrenAdminIds;
    }

    /// <summary>
    /// 检查用户的权限
    /// </summary>
    /// <param name="permissionName"></param>
    /// <returns></returns>
    public async Task<bool> CheckPermissionAsync(int userId,string permissionName,CancellationToken cancellationToken= default)
    {
        if (userId == 0)
        {
            return false;
        }
        var permissions = await GetPermissionsAsync(userId, cancellationToken);

        return permissions.Any(x => x == permissionName);
    }

}
