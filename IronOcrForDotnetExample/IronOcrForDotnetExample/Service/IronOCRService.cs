using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace IronOcrForDotnetExample.Service
{
    public class IronOCRService : IIronOCRService
    {
        private readonly string _IronKey = $@"IRONSUITE.CAP8825.GMAIL.COM.18455-CACDC23502-DTDNB6J-Q3Q5AMHJVXJ2-ADS7HO6J44XR-SBCKFEWPNVQC-SWBQ4AEI2Z2G-FNKDCLCSERQV-6VDQF5T2P7DH-HNJBQ2-TQKLHBRK2LKQEA-DEPLOYMENT.TRIAL-P23PJU.TRIAL.EXPIRES.24.SEP.2025";

        private readonly ILogger<IronOCRService> _logger;

        public IronOCRService(ILogger<IronOCRService> logger)
        {
            _logger = logger;
        }

        public void SSS()
        {
            IronOcr.License.LicenseKey = key;
            var ocr = new IronTesseract();
            ocr.Language = OcrLanguage.EnglishBest; // 簡中
            ocr.AddSecondaryLanguage(OcrLanguage.ChineseSimplifiedBest);
            try
            {
                using (var ocrInput = new OcrInput())
                {
                    ocrInput.DeNoise();
                    ocrInput.Deskew();         // 自動校正傾斜
                    ocrInput.Invert();         // 反色 (黑底白字)
                                               // ocrInput.EnhanceResolution(); // 增加對比
                    ocrInput.LoadImage("div.png");
                    var ocrResult = ocr.Read(ocrInput);
                    Console.WriteLine(ocrResult.Text);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
