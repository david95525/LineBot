
using LineBot.Dtos.Richmenu;
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
        private readonly IRichMenuService _richMenuService;
        // constructor
        public LineBotController(ILineBotService lineBotService, IRichMenuService richMenuService)
        {
            _lineBotService = lineBotService;
            _richMenuService = richMenuService; 
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
        //rich menu api
        [HttpPost("RichMenu/Validate")]
        public IActionResult ValidateRichMenu(RichMenuDto richMenu)
        {
            _richMenuService.ValidateRichMenu(richMenu);
            return Ok();
        }

        [HttpPost("RichMenu/Create")]
        public IActionResult CreateRichMenu(RichMenuDto richMenu)
        {
            _richMenuService.CreateRichMenu(richMenu);
            return Ok();
        }

        [HttpGet("RichMenu/GetList")]
        public async Task<IActionResult> GetRichMenuList()
        {
            _richMenuService.GetRichMenuList();
            return Ok();
        }

        [HttpPost("RichMenu/UploadImage/{richMenuId}")]
        public IActionResult UploadRichMenuImage(IFormFile imageFile, string richMenuId)
        {
            _richMenuService.UploadRichMenuImage(richMenuId, imageFile);
            return Ok();
        }

        [HttpPost("RichMenu/SetDefault/{richMenuId}")]
        public IActionResult SetDefaultRichMenu(string richMenuId)
        {
            _richMenuService.SetDefaultRichMenu(richMenuId);
            return Ok();
        }
    }
}
