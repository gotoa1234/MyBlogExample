using Microsoft.AspNetCore.Mvc;
using reCAPTCHATbyGoogleExample.Models.ResponseModel;
using System.Text.Json;

namespace reCAPTCHATbyGoogleExample.Controllers;

/// <summary>
/// reCAPTCHA - CheckBox V2 版本
/// </summary>
public partial class HomeController : Controller
{
    [HttpGet]
    public IActionResult CheckBoxV2()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CheckBoxV2Validate(string Username)
    {
        var captchaResponse = Request.Form["g-recaptcha-response"];

        if (string.IsNullOrEmpty(captchaResponse))
        {
            ViewBag.Message = "請完成 reCAPTCHA 驗證";
            return View();
        }

        var client = _httpClientFactory.CreateClient();
        var response = await client.GetStringAsync(
            $"https://www.google.com/recaptcha/api/siteverify?secret={_secretKey}&response={captchaResponse}");

        var captchaResult = JsonSerializer.Deserialize<ReCaptchaResponse>(response);

        if (captchaResult.success)
        {
            ViewBag.Message = $"驗證成功，歡迎 {Username}！";
        }
        else
        {
            ViewBag.Message = "驗證失敗，請重試。";
        }

        return View();
    }
}
