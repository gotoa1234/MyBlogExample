namespace WebSiteConfigReplaceExample.Service
{
    /// <summary>
    /// STEP1： 建立取得設定(IConfiguration)方法
    /// </summary>
    public class ConfigureSettingService
    {
        private string _masterDatabase = string.Empty;
        public string MasterDatabase { get { return _masterDatabase; } }        
        public ConfigureSettingService(IConfiguration configuration)
        {
            _masterDatabase = configuration["ConnectionStrings:MasterDatabase"];
        }
    }
}
