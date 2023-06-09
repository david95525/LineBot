using LineBot.Dtos.Messages.Request;
using LineBot.Dtos.Webhook;

namespace LineBot.Services
{
    public interface ILineBotService
    {
        void ReceiveWebhook(WebhookRequestBodyDto requestBody);
        void BroadcastMessageHandler(string messageType, object requestBody);
    }
}
