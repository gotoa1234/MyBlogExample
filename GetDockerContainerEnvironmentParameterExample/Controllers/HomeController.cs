using GetDockerContainerEnvironmentParameterExample.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace GetDockerContainerEnvironmentParameterExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var getEnviromentInfo = new ContainerEnvironmentModel();
            getEnviromentInfo.SecurityKeyHashMAC = ComputeHMACSHA256("Account:Louis", getEnviromentInfo.SecurityKey);
            return View(getEnviromentInfo);
        }

        /// <summary>
        /// HMAC SHA 256 �[�K
        /// </summary>
        /// <param name="message">��l�r��</param>
        /// <param name="key">���_</param>
        /// <returns></returns>
        static string ComputeHMACSHA256(string message, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(messageBytes);

                StringBuilder builder = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
