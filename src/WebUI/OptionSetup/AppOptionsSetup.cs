using Microsoft.Extensions.Options;

namespace WebUI.OptionSetup
{

    /// <summary>
    /// 框架设置
    /// </summary>
    public class AppOptionsSetup: IConfigureOptions<AppOptions>
    {
        private const string _sectionName = "AppOptions";
        private readonly IConfiguration _configuration;

        public AppOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(AppOptions options)
        {
            _configuration.GetSection(_sectionName).Bind(options);
        } 
    }
    // <summary>
    /// 后台框架页面展示设置
    /// </summary>
    public class AppOptions
    {
        #region 应用设置
        /// <summary>
        /// 应用调试模式
        /// </summary>
        public bool AppDebug { get; set; } = true;
        /// <summary>
        /// 应用Trace
        /// </summary>
        public bool AppTrace { get; set; } = false;
        /// <summary>
        /// 应用模式状态
        /// </summary>
        public string AppStatus { get; set; } = "";
        /// <summary>
        /// 应用版本号
        /// </summary>
        public string Version { get; set; } = "1.0.3";
        
        #endregion

        //视图输出字符串内容替换;留空则会自动进行计算
        public string PUBLIC { get; set; } = "";
        public string ROOT { get; set; } = "";
        public string CDN { get; set; } = "";
        public string Prefix { get; set; } = "";

        /// <summary>
        /// 是否启用多级菜单导航(主菜单为横向展示点击后子菜单左侧展示)
        /// </summary>
        public bool MultipleNav { get; set; } = false;
        /// <summary>
        /// 是否开启多选项卡(仅在开启多级菜单时起作用)
        /// </summary>
        public bool MultipleTab { get; set; } = true;
        /// <summary>
        /// 是否默认展示子菜单
        /// </summary>
        public bool ShowSubMenu { get; set; } = false;
        //后台皮肤;为空时表示使用skin-black-blue
        public string AdminSkin { get; set; } = "skin-blue-light";
        
        /// <summary>
        /// 登录页默认背景图
        /// </summary>
        public string? LoginBackground { get; set; }


    }
}
