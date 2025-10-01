using Microsoft.AspNetCore.Mvc;
using reCAPTCHATbyGoogleExample.Models.ResponseModel;
using System.Text.Json;

namespace reCAPTCHATbyGoogleExample.Controllers;

/// <summary>
/// reCAPTCHA - Invisible V2 版本
/// </summary>
public partial class HomeController : Controller
{
    // 1-1. 前端使用的網站金鑰
    private readonly string _InvisibleV2WebSiteKey = $@"6Ld0m6QrAAAAALRum4xz9IUcUB55tjDtMl4WMx7";
    // 1-2. 後端使用的金鑰
    private readonly string _InvisibleV2WebSecretKey = $@"6Ld0m6QrAAAAAKsAwscHz1qP_eAi3YOFNYHOIeQX";
    
    /// <summary>
    /// 2. 檢視 View
    /// </summary>    
    [HttpGet]
    public IActionResult InvisibleV2()
    {
        return View();
    }

    /// <summary>
    /// 3. 從前端獲得訪問 Token ，並反饋 reCAPTCHA Invisible V2 驗證結果
    /// </summary>    
    [HttpPost]
    public async Task<IActionResult> InvisibleV2Validate(string Username)
    {
        // 3-1. 前端使用的 WebSite Key 
        var captchaResponse = Request.Form["g-recaptcha-response"];
        if (string.IsNullOrEmpty(captchaResponse))
        {
            ViewBag.Message = "請完成 reCAPTCHA - Invisible V2 驗證";
            return View();
        }

        // 3-2. 將後端 + 前端使用的 key 送到 Google 的 reCAPTCHA 驗證
        var client = _httpClientFactory.CreateClient();
        var postData = new Dictionary<string, string> {
                          { "secret", _InvisibleV2WebSecretKey },
                          { "response", captchaResponse }
                      };
        
        // 3-3. 獲得響應結果
        var httpResponse = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify",
                                                   new FormUrlEncodedContent(postData));
        var json = await httpResponse.Content.ReadAsStringAsync();
        var captchaResult = JsonSerializer.Deserialize<ReCaptchaResponse>(json);

        // 3-4. 響應 - 並將結果放進 ViewBag 給用戶提示
        if (captchaResult.success)
        {
            ViewBag.Message = $"Invisible V2 驗證成功，歡迎 {Username}！";
        }
        else
        {
            ViewBag.Message = "Invisible V2 驗證失敗，請重試。";
        }

        // 4. 返回頁面
        return View("InvisibleV2");
    }
}