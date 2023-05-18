using InjectReflectionForTranslateLanguageExample.Interface.Nation;

namespace InjectReflectionForTranslateLanguageExample.Implement.Nation
{
    public class NationJapanService : INationJapanService
    {
        private readonly ILogger<NationJapanService> _logger;
        public NationJapanService(ILogger<NationJapanService> logger)
        {
            _logger = logger;
        }
        public string GetCorrespondMessage(string input)
        {
            #region 此段應實作引用庫，轉成對應文化語言，這邊只是舉例
            if (input == "哈囉")
                return "こんにちは";
            #endregion

            return string.Empty;
        }
    }
}
