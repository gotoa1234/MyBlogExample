using InjectReflectionForTranslateLanguageExample.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InjectReflectionForTranslateLanguageExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly ILogger<LanguageController> _logger;
        private readonly ILanguageService _languageService;
        public LanguageController(ILogger<LanguageController> logger, ILanguageService languageService)
        {
            _logger = logger;
            _languageService = languageService;
        }


        [HttpGet(Name = "GetLanaguage")]
        public List<string> GetLanaguage(string message)
        {           
            return _languageService.GetAllMessage(message);
        }        
    }
}
