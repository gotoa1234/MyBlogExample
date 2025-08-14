using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

public class Program_ScoreV3
{
    static void Main(string[] args)
    {
        // 啟動 ChromeDriver
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");        

        try
        {
            using (var driver = new ChromeDriver(options))
            {
                // 1. 進入 reCAPTCHA v3 測試頁面
                driver.Navigate().GoToUrl("http://localhost:5000/Home/ScoreV3"); // 改成你的實際 URL

                // 2. 填入 Username
                var usernameField = driver.FindElement(By.Name("Username"));
                usernameField.Clear();
                usernameField.SendKeys("MilkTeaGreen");

                // 3. 等待 g-recaptcha-response 有值
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(d =>
                {
                    var tokenValue = d.FindElement(By.Id("g-recaptcha-response")).GetAttribute("value");
                    return !string.IsNullOrEmpty(tokenValue);
                });

                // 4. 取得 Token
                Console.WriteLine("[Debug] Token 已產生: " +
                    driver.FindElement(By.Id("g-recaptcha-response")).GetAttribute("value"));

                // 5. 送出表單
                driver.FindElement(By.CssSelector("form#score-form button[type='submit']")).Click();

                // 6. 等驗證結果出現
                wait.Until(d => d.PageSource.Contains("Score V3 驗證成功") || d.PageSource.Contains("Score V3 驗證失敗"));

                Console.WriteLine("頁面回應:\n" + driver.PageSource);

                Thread.Sleep(30000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}

