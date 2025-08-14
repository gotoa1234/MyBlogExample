using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

public class Program_InvisibleV2
{
    static void Main(string[] args)
    {
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        try
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                // 1. 進入 Invisible V2 驗證器測試頁面
                driver.Navigate().GoToUrl("http://localhost:5000/Home/InvisibleV2");

                // 2. 填入名稱
                var nameInput = driver.FindElement(By.Name("Username"));
                nameInput.Clear();
                nameInput.SendKeys("MilkTeaGreen");

                // 3. 點擊送出按鈕（Invisible reCAPTCHA 會自動觸發）
                var submitButton = driver.FindElement(By.CssSelector(".g-recaptcha"));
                submitButton.Click();

                // 4. 等待驗證處理（實際應該用顯示等待 WebDriverWait）
                Thread.Sleep(5000);

                // 5. 取得頁面結果
                var bodyText = driver.FindElement(By.TagName("body")).Text;
                Console.WriteLine("頁面內容：\n" + bodyText);

                Thread.Sleep(30000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}