
using LineBot.Dtos.Webhook;
using LineBot.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LineBot.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LineBotController : ControllerBase
    {
        //service
        private readonly ILineBotService _lineBotService;
        // constructor
        public LineBotController(ILineBotService lineBotService)
        {
            _lineBotService = lineBotService;
        }
        //接收 Line 傳送的 webhook event
        [HttpPost("Webhook")]
        public IActionResult Webhook(WebhookRequestBodyDto body)
        {
            _lineBotService.ReceiveWebhook(body); // 呼叫 Service
            return Ok();
        }
        [HttpPost("SendMessage/Broadcast")]
        //{"messages":[{"type":"text","text":"Hello, world1"}]}
        public IActionResult Broadcast([Required] string messageType,[FromBody] object body)
        {
            _lineBotService.BroadcastMessageHandler(messageType, body);
            return Ok();
        }
    }
}
