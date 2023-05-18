using InjectReflectionForTranslateLanguageExample.Interface.Nation;

namespace InjectReflectionForTranslateLanguageExample.Implement.Nation
{
    public class NationChinaService : INationChinaService
    {
        private readonly ILogger<NationChinaService> _logger;
        public NationChinaService(ILogger<NationChinaService> logger)
        {
            _logger = logger;
        }
        public string GetCorrespondMessage(string input)
        {
            #region 此段應實作引用庫，轉成對應文化語言，這邊只是舉例
            if (input == "哈囉")
                return "哈啰";
            #endregion
            #region 假設這個文化有些文字要過濾
            if (input == "辱華文字")
                return string.Empty;
            #endregion
            return string.Empty;
        }
    }
}
