using IronOcr;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace IronOcrForDotnetExample.Service
{
    public class IronOCRService : IIronOCRService
    {
        // 1. 準備好金鑰
        private readonly string _IronKey = $@"IRONSUITE.CAP8825.GMAIL.COM.18455-CACDC23502-DTDNB6J-Q3Q5AMHJVXJ2-ADS7HO6J44XR-SBCKFEWPNVQC-SWBQ4AEI2Z2G-FNKDCLCSERQV-6VDQF5T2P7DH-HNJBQ2-TQKLHBRK2LKQEA-DEPLOYMENT.TRIAL-P23PJU.TRIAL.EXPIRES.24.SEP.2025";

        private readonly ILogger<IronOCRService> _logger;

        public IronOCRService(ILogger<IronOCRService> logger)
        {
            _logger = logger;
        }

        public string IronOCR()
        {
            // 2. 設定金鑰
            IronOcr.License.LicenseKey = _IronKey;
            var ocr = new IronTesseract();
            var ocrResult = string.Empty;

            // 3. 可以選擇多個語言，選擇英文 + 簡中
            ocr.Language = OcrLanguage.EnglishBest; 
            ocr.AddSecondaryLanguage(OcrLanguage.ChineseSimplifiedBest);

            try
            {
                // 4. 開始 OCR 工作
                using (var ocrInput = new OcrInput())
                {
                    // 4-1. 設定 IronOCR 會為圖片處理以下工作 
                    ocrInput.DeNoise();// a. 去除雜訊
                    ocrInput.Deskew(); // b. 自動校正傾斜
                    ocrInput.Invert(); // c. 反色 (黑底白字)                                       
                    
                    // 4-2. 執行 OCR (會套用上述的圖片處理)
                    ocrInput.LoadImage("div.png");
                    ocrResult = ocr.Read(ocrInput).Text;                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return ocrResult;
        }
    }
}
