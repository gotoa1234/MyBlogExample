using Tesseract;
using TesseractOCRNetExample.Helper;

namespace TesseractOCRNetExample.Service
{
    public class TesseractService : ITesseractService
    {
        private readonly ILogger<ITesseractService> _logger;        
        public TesseractService(ILogger<ITesseractService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 使用 Tessract 進行 OCR 分辨文字
        /// </summary>
        /// <returns></returns>
        public string TesseractVersionImage()
        {
            // 1. 語言包路徑
            var tessDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"Source\tessdata");

            // 2. 解析的文字內容
            var result = string.Empty;
            try
            {
                // 3. 設定語言包，必須使用副檔名前的名稱，並且用 + 才能支持多語言
                using (var engine = new TesseractEngine(tessDataPath, "eng+chi_sim", EngineMode.Default))
                {
                    // 4. 讀取圖片
                    using (var img = Pix.LoadFromFile(StaticHelper._ImageFileName))
                    {
                        // 5. 執行 OCR 結果
                        using (var page = engine.Process(img))
                        {                            
                            result = page.GetText();
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
