// 啟動 Chrome
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

ChromeOptions options = new ChromeOptions();
options.AddArgument("--start-maximized");
try
{
    using (IWebDriver driver = new ChromeDriver(options))
    {
        // 你的測試頁（必須含有 reCAPTCHA v2 Checkbox）
        driver.Navigate().GoToUrl("http://localhost:5000");

        // 等待頁面載入
        Thread.Sleep(2000);

        // 切換到 reCAPTCHA iframe
        IWebElement iframe = driver.FindElement(By.CssSelector("iframe[title='reCAPTCHA']"));
        driver.SwitchTo().Frame(iframe);

        // 方式 1：直接點擊（很容易被判定機器人）
        Console.WriteLine("直接 DOM Click reCAPTCHA...");
        IWebElement checkbox = driver.FindElement(By.Id("recaptcha-anchor"));
        checkbox.Click();

        Thread.Sleep(5000);

        // 回到主頁面
        driver.SwitchTo().DefaultContent();
        Thread.Sleep(2000);

        // 方式 2：模擬滑鼠移動
        driver.Navigate().Refresh();
        Thread.Sleep(2000);
        iframe = driver.FindElement(By.CssSelector("iframe[title='reCAPTCHA']"));
        driver.SwitchTo().Frame(iframe);

        Console.WriteLine("模擬滑鼠移動到 reCAPTCHA...");
        checkbox = driver.FindElement(By.Id("recaptcha-anchor"));

        Actions actions = new Actions(driver);
        actions.MoveToElement(checkbox)
               .Pause(TimeSpan.FromSeconds(1))
               .Click()
               .Perform();

        Thread.Sleep(10000); // 等待觀察
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}