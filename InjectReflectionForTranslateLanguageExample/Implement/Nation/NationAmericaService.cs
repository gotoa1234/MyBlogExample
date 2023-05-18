using InjectReflectionForTranslateLanguageExample.Interface.Nation;

namespace InjectReflectionForTranslateLanguageExample.Implement.Nation
{
    public class NationAmericaService : INationAmericaService
    {
        private readonly ILogger<NationAmericaService> _logger;
        public NationAmericaService(ILogger<NationAmericaService> logger)
        {
            _logger = logger;
        }

        public string GetCorrespondMessage(string input)
        {
            #region 此段應實作引用庫，轉成對應文化語言，這邊只是舉例
            if (input == "哈囉")
                return "Hello";
            #endregion

            return string.Empty;
        }
    }
}
