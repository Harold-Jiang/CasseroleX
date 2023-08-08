using CasseroleX.Application.Common.Models;

namespace CasseroleX.Application.Utils;

public static class Tree
{
    private readonly static string[] Icon = { "│", "├", "└" };
    private readonly static string Nbsp = "&nbsp;";

    /// <summary>
    /// 得到子级数组(不包含自己)
    /// </summary>
    public static List<T> GetChild<T>(List<T> list, int myid) where T : TreeDto<T>
    {
        List<T> newarr = new();
        foreach (var value in list)
        {
            if (value.Pid == myid)
                newarr.Add(value);
        }
        return newarr;
    }


    /// <summary>
    /// 读取指定节点的所有孩子节点
    /// </summary>
    public static List<T> GetChildren<T>(List<T> list, int myid, bool withself = false) where T : TreeDto<T>
    {
        List<T> newarr = new();
        foreach (var value in list)
        {
            if (value.Pid == myid)
            {
                newarr.Add(value);
                newarr.AddRange(GetChildren(list, value.Id));
            }
            else if (withself && value.Id == myid)
                newarr.Add(value);
        }
        return newarr;
    }

    /// <summary>
    /// 读取指定节点的所有孩子节点ID
    /// </summary>
    public static List<int> GetChildrenIds<T>(List<T> list, int myid, bool withself = false) where T : TreeDto<T>
    {
        var childrenlist = GetChildren(list, myid, withself);
        return childrenlist.Select(x => x.Id).ToList();
    }

    /// <summary>
    /// 得到当前位置父辈数组
    /// </summary> 
    public static List<T> GetParent<T>(List<T> list, int myid) where T : TreeDto<T>
    {
        int? pid = 0;
        List<T> newarr = new();
        foreach (var value in list)
        {
            if (value.Id == myid)
            {
                pid = value.Pid;
                break;
            }
        }
        if (pid > 0)
        {
            foreach (var value in list)
            {
                if (value.Id == pid)
                {
                    newarr.Add(value);
                    break;
                }
            }
        }
        return newarr;
    }


    /// <summary>
    /// 得到当前位置所有父辈数组
    /// </summary> 
    public static List<T> GetParents<T>(List<T> list, int myid, bool withself = false) where T : TreeDto<T>
    {
        int? pid = 0;
        List<T> newarr = new();
        foreach (var value in list)
        {
            if (value.Id == myid)
            {
                if (withself)
                {
                    newarr.Add(value);
                }
                pid = value.Pid;
                break;
            }
        }
        if (pid > 0)
        {
            List<T> arr = GetParents(list, pid.Value, true);
            newarr.AddRange(arr);
        }
        return newarr;
    }

    /// <summary>
    /// 读取指定节点所有父类节点ID
    /// </summary>
    public static List<int> GetParentsIds<T>(List<T> list, int myid, bool withself = false) where T : TreeDto<T>
    {
        var parentlist = GetParents(list, myid, withself);
        return parentlist.Select(x => x.Id).ToList();
    }

    /// <summary>
    /// 树型结构Option
    /// </summary>
    /// <param name="myid">表示获得这个ID下的所有子级</param>
    /// <param name="itemtpl">条目模板 如："<option value=@id @selected @disabled>@spacer@name</option>"</param>
    /// <param name="selectedids">被选中的ID，比如在做树型下拉框的时候需要用到</param>
    /// <param name="disabledids">被禁用的ID，比如在做树型下拉框的时候需要用到</param>
    /// <param name="itemprefix">每一项前缀</param>
    /// <param name="toptpl">顶级栏目的模板</param>
    /// <returns></returns>
    public static string GetTree<T>(List<T> list, int myid, string itemtpl = "<option value=@id @selected @disabled>@spacer@name</option>", List<int>? selectedids = default, List<int>? disabledids = default, string itemprefix = "", string toptpl = "") where T : TreeDto<T>
    {
        string ret = "";
        int number = 1;
        var childs = GetChild(list, myid);
        if (childs != null)
        {
            int total = childs.Count;
            foreach (var value in childs)
            {
                string j = "";
                string k = "";
                if (number == total)
                {
                    j += Icon[2];
                    k = itemprefix != "" ? Nbsp : "";
                }
                else
                {
                    j += Icon[1];
                    k = itemprefix != "" ? Icon[0] : "";
                }
                string spacer = itemprefix != "" ? itemprefix + j : "";
                string selected = (selectedids != null && selectedids.Contains(value.Id)) ? "selected" : "";
                string disabled = (disabledids != null && disabledids.Contains(value.Id)) ? "disabled" : "";
                //object to dic
                var valueDict = value.GetType().GetProperties()
                    .ToDictionary(
                        prop => "@" + prop.Name.ToLowerInvariant(),
                        prop => prop.GetValue(value)?.ToString()
                    );
                valueDict["@selected"] = selected;
                valueDict["@disabled"] = disabled;
                 
                itemtpl = (value.Pid == 0 || GetChild(list,value.Id) is not null) && !string.IsNullOrEmpty(toptpl) ? toptpl: itemtpl;
                foreach (var entry in valueDict)
                {
                    itemtpl = itemtpl.Replace(entry.Key, entry.Value);
                } 
                ret += itemtpl;
                ret += GetTree(list, value.Id, itemtpl, selectedids, disabledids, itemprefix + k + Nbsp, toptpl);
                number++;
            }
        }
        return ret;
    }
     

    /// <summary>
    /// 获取树状数组
    /// </summary>
    public static List<T> GetTreeArray<T>(List<T> list, int myid, string itemprefix = "") where T : TreeDto<T>
    {
        var childs = GetChild(list, myid);
        int n = 0;
        List<T> data = new();
        int number = 1;
        if (childs != null)
        {
            int total = childs.Count;
            foreach (var value in childs)
            {
                int id = value.Id;
                string j = "";
                string k = "";
                if (number == total)
                {
                    j += Icon[2];
                    k = itemprefix != "" ? Nbsp : "";
                }
                else
                {
                    j += Icon[1];
                    k = itemprefix != "" ? Icon[0] : "";
                }
                value.Spacer = itemprefix != "" ? itemprefix + j : "";

                data.Add(value);
                data[n].ChildList = GetTreeArray(list, id, itemprefix + k + Nbsp);
                n++;
                number++;
            }
        }
        return data;
    }

    /// <summary>
    /// 将getTreeArray的结果返回为二维数组
    /// </summary>
    /// <param name="data"></param>
    /// <param name="field"></param>
    /// <returns></returns>
    public static List<T> GetTreeList<T>(List<T> data, string field = "Name") where T : TreeDto<T>
    {
        List<T> arr = new();
        foreach (var item in data)
        {
            var childlist = item.ChildList ?? new();
            if (field == "Name")
            {
                item.Name = $"{item.Spacer} {item.Name}";
            }
            else if (field == "Title")
            {
                item.Title = $"{item.Spacer} {item.Title}";
            }
            item.HasChild = childlist is not null && childlist.Count > 0 ? 1 : 0;
            if (item.Id > 0)
                arr.Add(item);

            if (childlist is not null && childlist.Count > 0)
            {
                arr.AddRange(GetTreeList(childlist, field));
            }
        }
        return arr;
    }  
}