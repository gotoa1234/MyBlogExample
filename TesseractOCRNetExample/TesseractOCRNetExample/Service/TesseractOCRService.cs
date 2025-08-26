using TesseractOCR.Enums;
using TesseractOCR;
using TesseractOCRNetExample.Helper;

namespace TesseractOCRNetExample.Service
{
    public class TesseractOCRService : ITesseractOCRService
    {
        private readonly ILogger<ITesseractOCRService> _logger;        
        public TesseractOCRService(ILogger<ITesseractOCRService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 使用 TessractOCR 進行 OCR 分辨文字
        /// </summary>
        /// <returns></returns>
        public string TesseractOCRVersionImage()
        {
            // 語言包路徑
            string tessDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
            
            // 解析的文字內容
            var result = string.Empty;
            try
            {
                // 使用的語言
                var languages = new List<Language>() { Language.English, Language.ChineseSimplified };
                
                using (var engine = new Engine(tessDataPath, languages, EngineMode.Default))
                {
                    using (var img = TesseractOCR.Pix.Image.LoadFromFile(StaticHelper._ImageFileName))
                    {
                        using (var page = engine.Process(img))
                        {                            
                            result = page.Text;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }
    }
}
