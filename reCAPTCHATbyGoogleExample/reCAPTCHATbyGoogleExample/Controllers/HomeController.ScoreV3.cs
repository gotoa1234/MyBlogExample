using Microsoft.AspNetCore.Mvc;
using reCAPTCHATbyGoogleExample.Models.ResponseModel;
using System.Text.Json;

namespace reCAPTCHATbyGoogleExample.Controllers;

/// <summary>
/// reCAPTCHA -  Score V3 版本
/// </summary>
public partial class HomeController : Controller
{
    // 1-1.  前端使用的網站金鑰
    private readonly string _ScoreV3WebSiteKey = $@"";
    // 1-2. 後端使用的金鑰
    private readonly string _ScoreV3WebSecretKey = $@"";

    /// <summary>
    /// 2. 檢視 View
    /// </summary>
    [HttpGet]
    public IActionResult ScoreV3()
    {
        return View();
    }

    /// <summary>
    /// 3. 從前端獲得訪問 Token ，並反饋 reCAPTCHA Score V3 驗證結果
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ScoreV3Validate(string Username)
    {
        // 3-1. 前端使用的 WebSite Key 
        var captchaResponse = Request.Form["g-recaptcha-response"];
        if (string.IsNullOrEmpty(captchaResponse))
        {
            ViewBag.Message = "請完成 reCAPTCHA - Score V3 驗證";
            return View();
        }

        // 3-2. 將後端 + 前端使用的 key 送到 Google 的 reCAPTCHA 驗證
        var client = _httpClientFactory.CreateClient();
        var postData = new Dictionary<string, string> {
                           {"secret", _ScoreV3WebSecretKey},
                           {"response", captchaResponse}
                       };
        
        // 3-3. 獲得響應結果 Score V3 的 Json 格式多了 score 欄位
        var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", 
                                               new FormUrlEncodedContent(postData));

        var json = await response.Content.ReadAsStringAsync();
        var captchaResult = JsonSerializer.Deserialize<ReCaptchaResponseV3>(json);

        // 3-4. 響應 - 並將結果放進 ViewBag 給用戶提示
        if (captchaResult.success)
        {
            // 3-5. 可以依照分數決定自己網站對於機器人應對策略
            ViewBag.Message = $"Score V3 驗證成功，當前分數 : {captchaResult.score}，歡迎 {Username}！";
        }
        else
        {
            ViewBag.Message = "Score V3 驗證失敗，請重試。";
        }

        // 4. 返回頁面
        return View("ScoreV3");
    }

}