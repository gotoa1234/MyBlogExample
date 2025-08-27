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
            // 語言包路徑
            var tessDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"Source\tessdata");

            // 解析的文字內容
            var result = string.Empty;
            try
            {
                using (var engine = new TesseractEngine(tessDataPath, "chi_sim", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(StaticHelper._ImageFileName))
                    {
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
