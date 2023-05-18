using InjectReflectionForTranslateLanguageExample.Interface;
using System.Reflection;

namespace InjectReflectionForTranslateLanguageExample.Implement
{
    public class LanguageService : ILanguageService
    {
        private readonly ILogger<LanguageService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public LanguageService(IServiceProvider serviceProvider, ILogger<LanguageService> logger) 
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public List<string> GetAllMessage(string message)
        {
            var resultMessages = new List<string>();

            //取得所有國家
            var nationServices = GetNationSericeMapDic();
            foreach (var service in nationServices)
            {
               resultMessages.Add(service.GetCorrespondMessage(message));
            }
            return resultMessages;
        }

        private List<INationBase> GetNationSericeMapDic()
        {
            //反射取出所有引用INationBase的實例
            var nationSericeItems = new List<INationBase>();
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Contains(typeof(INationBase)));
            foreach (var type in types)
            {
                var service = _serviceProvider.CreateScope().ServiceProvider.GetService(type) as INationBase;
                if (service != null)
                {
                    nationSericeItems.Add(service);
                }
            }
            return nationSericeItems;
        }
    }
}
