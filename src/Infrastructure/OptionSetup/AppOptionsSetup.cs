using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CasseroleX.Infrastructure.OptionSetup
{
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
    /// Background framework page display settings
    /// </summary>
    public class AppOptions
    {
        #region app settings

        public bool AppDebug { get; set; } = true;
        
        public bool AppTrace { get; set; } = false;
        
        public string AppStatus { get; set; } = "";

        /// <summary>
        /// Enable multi-level menu navigation  
        /// </summary>
        public bool MultipleNav { get; set; } = false;
        /// <summary>
        /// Whether to enable multiple tabs (only effective when opening multi-level menus)
        /// </summary>
        public bool MultipleTab { get; set; } = true;
         
        /// <summary>
        /// Default Display Submenu
        /// </summary>
        public bool ShowSubMenu { get; set; } = false;

        public string AdminSkin { get; set; } = "skin-blue-light";
         
        public bool LangSwitchOn { get; set; } = true;

        #endregion


    }
}
