using InjectReflectionForTranslateLanguageExample.Interface;
using InjectReflectionForTranslateLanguageExample.Interface.Nation;

namespace InjectReflectionForTranslateLanguageExample.Implement.Nation
{
    public class NationGermanyService : INationGermanyService
    {
        private readonly ILogger<NationGermanyService> _logger;
        public NationGermanyService(ILogger<NationGermanyService> logger)
        {
            _logger = logger;
        }
        public string GetCorrespondMessage(string input)
        {
            #region 此段應實作引用庫，轉成對應文化語言，這邊只是舉例
            if (input == "哈囉")
                return "Hallo";
            #endregion
            return string.Empty;
        }
    }
}
