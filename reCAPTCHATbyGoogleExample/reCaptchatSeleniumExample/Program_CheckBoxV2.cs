using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;



public class Program_CheckBoxV2
{
    static void Main(string[] args)
    {

        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        try
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                // 1. 進入 reCAPTCHA v2 Checkbox 測試頁面
                driver.Navigate().GoToUrl("http://localhost:5000/Home/CheckBoxV2");

                // 2. 等待頁面載入
                Thread.Sleep(2000);

                // 3. 填入名稱
                var nameInput = driver.FindElement(By.Name("Username"));
                nameInput.Clear();
                nameInput.SendKeys("MilkTeaGreen");

                Thread.Sleep(2000);

                // 4. 切換到 reCAPTCHA iframe
                IWebElement iframe = driver.FindElement(By.CssSelector("iframe[title='reCAPTCHA']"));
                driver.SwitchTo().Frame(iframe);

                // 5. 直接點擊（很容易被判定機器人）
                Console.WriteLine("直接 DOM Click reCAPTCHA...");
                IWebElement checkbox = driver.FindElement(By.Id("recaptcha-anchor"));
                checkbox.Click();

                Thread.Sleep(30000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

}
