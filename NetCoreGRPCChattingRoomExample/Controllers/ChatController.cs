using ChatApp;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreGRPCChattingRoomExample.Controllers
{
    public class ChatController : Controller
    {
        private readonly ChatService.ChatServiceClient _chatClient;

        public ChatController(ChatService.ChatServiceClient chatClient)
        {
            _chatClient = chatClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> JoinChat(string username)
        {
            var request = new JoinRequest { Username = username };
            var response = await _chatClient.JoinAsync(request);

            // Process the response (e.g., update UI with active users)
            // ...

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SendMessage(string username, string message)
        {
            var request = new MessageRequest { Username = username, Message = message };
            await _chatClient.SendMessageAsync(request);

            // Process the response (e.g., update UI with new message)
            // ...

            return RedirectToAction("Index");
        }
    }
}
