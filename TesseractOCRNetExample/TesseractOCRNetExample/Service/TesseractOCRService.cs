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
            // 1. 語言包路徑
            string tessDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"Source\tessdata");

            // 2. 解析的文字內容
            var result = string.Empty;
            try
            {
                // 3. 設定語言包，必須使用副檔名前的名稱，並且用 List 才能支持多語言
                var languages = new List<Language>() { Language.English, Language.ChineseSimplified };
                using (var engine = new Engine(tessDataPath, languages, EngineMode.Default))
                {
                    // 4. 讀取圖片
                    using (var img = TesseractOCR.Pix.Image.LoadFromFile(StaticHelper._ImageFileName))
                    {
                        // 5. 執行 OCR 結果
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
