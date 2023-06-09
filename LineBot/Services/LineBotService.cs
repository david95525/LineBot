using LineBot.Dtos.Messages.Request;
using LineBot.Dtos.Messages;
using LineBot.Dtos.Webhook;
using LineBot.Enum;
using LineBot.Providers;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using LineBot.Dtos.Actions;

namespace LineBot.Services
{
    public class LineBotService : ILineBotService
    {
        private readonly string channelAccessToken;
        private readonly string channelSecret;
        private readonly IConfiguration _configuration;
        private readonly string replyMessageUri = "https://api.line.me/v2/bot/message/reply";
        private readonly string broadcastMessageUri = "https://api.line.me/v2/bot/message/broadcast";
        private static HttpClient client = new HttpClient(); // 負責處理HttpRequest
        private readonly JsonProvider _jsonProvider = new JsonProvider();

        public LineBotService(IConfiguration configuration)
        {
            _configuration = configuration;
            channelAccessToken = _configuration.GetSection("channelAccessToken").Value;
            channelSecret = _configuration.GetSection("channelSecret").Value;
        }
        /// <summary>
        /// 接收 webhook event 處理
        /// </summary>
        /// <param name="requestBody"></param>
        public void ReceiveWebhook(WebhookRequestBodyDto requestBody)
        {
            dynamic replyMessage;
            foreach (var eventObject in requestBody.Events)
            {
                switch (eventObject.Type)
                {
                    case WebhookEventTypeEnum.Message:
                        if (eventObject.Message.Type == MessageEnum.Text)
                            ReceiveMessageWebhookEvent(eventObject);
                        break;
                    case WebhookEventTypeEnum.Unsend:
                        Console.WriteLine($"使用者{eventObject.Source.UserId}在聊天室收回訊息！");
                        break;
                    case WebhookEventTypeEnum.Follow:
                        Console.WriteLine($"使用者{eventObject.Source.UserId}將我們新增為好友！");
                        break;
                    case WebhookEventTypeEnum.Unfollow:
                        Console.WriteLine($"使用者{eventObject.Source.UserId}封鎖了我們！");
                        break;
                    case WebhookEventTypeEnum.Join:
                        Console.WriteLine("我們被邀請進入聊天室了！");
                        break;
                    case WebhookEventTypeEnum.Leave:
                        Console.WriteLine("我們被聊天室踢出了");
                        break;
                    // -------- 新增內容 --------
                    case WebhookEventTypeEnum.MemberJoined:
                        string joinedMemberIds = "";
                        foreach (var member in eventObject.Joined.Members)
                        {
                            joinedMemberIds += $"{member.UserId} ";
                        }
                        Console.WriteLine($"使用者{joinedMemberIds}加入了群組！");
                        break;
                    case WebhookEventTypeEnum.MemberLeft:
                        string leftMemberIds = "";
                        foreach (var member in eventObject.Left.Members)
                        {
                            leftMemberIds += $"{member.UserId} ";
                        }
                        Console.WriteLine($"使用者{leftMemberIds}離開了群組！");
                        break;
                    case WebhookEventTypeEnum.Postback:
                        Console.WriteLine($"使用者{eventObject.Source.UserId}觸發了postback事件");
                        break;
                    case WebhookEventTypeEnum.VideoPlayComplete:
                        replyMessage = new ReplyMessageRequestDto<TextMessageDto>()
                        {
                            ReplyToken = eventObject.ReplyToken,
                            Messages = new List<TextMessageDto>
                            {
                                new TextMessageDto(){Text = $"使用者您好，謝謝您收看我們的宣傳影片，祝您身體健康萬事如意 !"}
                            }
                        };
                        ReplyMessageHandler(replyMessage);
                        break;
                }
            }
        }
        /// <summary>
        /// 接收到廣播請求時，在將請求傳至 Line 前多一層處理，依據收到的 messageType 將 messages 轉換成正確的型別，這樣 Json 轉換時才能正確轉換。
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="requestBody"></param>
        public void BroadcastMessageHandler(string messageType, object requestBody)
        {
            string strBody = requestBody.ToString();
            dynamic messageRequest = new BroadcastMessageRequestDto<BaseMessageDto>();
            switch (messageType)
            {
                case MessageEnum.Text:
                    messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<TextMessageDto>>(strBody);
                    break;
                case MessageEnum.Sticker:
                    messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<StickerMessageDto>>(strBody);
                    break;
                case MessageEnum.Image:
                    messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<ImageMessageDto>>(strBody);
                    break;
                case MessageEnum.Video:
                    messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<VideoMessageDto>>(strBody);
                    break;
                case MessageEnum.Audio:
                    messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<AudioMessageDto>>(strBody);
                    break;
                case MessageEnum.Location:
                    messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<LocationMessageDto>>(strBody);
                    break;
                case MessageEnum.Imagemap:
                    messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<ImagemapMessageDto>>(strBody);
                    break;
            }
            BroadcastMessage(messageRequest);
        }
        /// <summary>
        /// 將廣播訊息請求送到 Line
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        private async void BroadcastMessage<T>(BroadcastMessageRequestDto<T> request)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //帶入 channel access token
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);
            var json = _jsonProvider.Serialize(request);
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(broadcastMessageUri),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// 接收到回覆請求時，在將請求傳至 Line 前多一層處理(目前為預留)
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="requestBody"></param>
        public void ReplyMessageHandler<T>(ReplyMessageRequestDto<T> requestBody)
        {
            ReplyMessage(requestBody);
        }
        /// <summary>
        /// 將回覆訊息請求送到 Line
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>    
        private async void ReplyMessage<T>(ReplyMessageRequestDto<T> request)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken); //帶入 channel access token
            var json = _jsonProvider.Serialize(request);
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(replyMessageUri),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        private void ReceiveMessageWebhookEvent(WebhookEventDto eventDto)
        {
            dynamic replyMessage = new ReplyMessageRequestDto<BaseMessageDto>();

            switch (eventDto.Message.Type)
            {
                // 收到文字訊息
                case MessageEnum.Text:
                    // 訊息內容等於 "測試" 時
                    if (eventDto.Message.Text == "測試")
                    {
                        //回覆訊息範例
                        replyMessage = new ReplyMessageRequestDto<TextMessageDto>
                        {
                            ReplyToken = eventDto.ReplyToken,
                            Messages = new List<TextMessageDto>
                            {
                                new TextMessageDto
                                {
                                    Text = "測試訊息 請至手機上瀏覽完整功能",
                                    QuickReply = new QuickReplyItemDto
                                    {
                                        Items = new List<QuickReplyButtonDto>
                                        {
                                            // message action
                                            new QuickReplyButtonDto {
                                                Action = new ActionDto {
                                                    Type = ActionTypeEnum.Message,
                                                    Label = "message 測試" ,
                                                    Text = "測試"
                                                }
                                            },
                                            // uri action
                                            new QuickReplyButtonDto {
                                                Action = new ActionDto {
                                                    Type = ActionTypeEnum.Uri,
                                                    Label = "uri 測試" ,
                                                    Uri = "https://www.appx.com.tw"
                                                }
                                            },
                                             // 使用 uri schema 分享 Line Bot 資訊
                                            new QuickReplyButtonDto {
                                                Action = new ActionDto {
                                                    Type = ActionTypeEnum.Uri,
                                                    Label = "分享 Line Bot 資訊" ,
                                                    Uri = "https://line.me/R/nv/recommendOA/@089yvykp"
                                                }
                                            },
                                            // postback action
                                            new QuickReplyButtonDto {
                                                Action = new ActionDto {
                                                    Type = ActionTypeEnum.Postback,
                                                    Label = "postback 測試" ,
                                                    Data = "quick reply postback action" ,
                                                    DisplayText = "使用者傳送 displayTex，但不會有 Webhook event 產生。",
                                                    InputOption = PostbackInputOptionEnum.OpenKeyboard,
                                                    FillInText = "第一行\n第二行"
                                                }
                                            },
                                            // datetime picker action
                                            new QuickReplyButtonDto {
                                                Action = new ActionDto {
                                                Type = ActionTypeEnum.DatetimePicker,
                                                Label = "日期時間選擇",
                                                    Data = "quick reply datetime picker action",
                                                    Mode = DatetimePickerModeEnum.Datetime,
                                                    Initial = "2022-09-30T19:00",
                                                    Max = "2022-12-31T23:59",
                                                    Min = "2021-01-01T00:00"
                                                }
                                            },
                                            // camera action
                                            new QuickReplyButtonDto {
                                                Action = new ActionDto {
                                                    Type = ActionTypeEnum.Camera,
                                                    Label = "開啟相機"
                                                }
                                            },
                                            // camera roll action
                                            new QuickReplyButtonDto {
                                                Action = new ActionDto {
                                                    Type = ActionTypeEnum.CameraRoll,
                                                    Label = "開啟相簿"
                                                }
                                            },
                                            // location action
                                            new QuickReplyButtonDto {
                                                Action = new ActionDto {
                                                    Type = ActionTypeEnum.Location,
                                                    Label = "開啟位置"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        };
                    }
                    if (eventDto.Message.Text == "Sender")
                    {
                        replyMessage = new ReplyMessageRequestDto<TextMessageDto>
                        {
                            ReplyToken = eventDto.ReplyToken,
                            Messages = new List<TextMessageDto>
                            {
                                new TextMessageDto
                                {
                                    Text = "你好，我是客服人員 1號",
                                    Sender = new SenderDto
                                    {
                                        Name = "客服人員 1號",
                                        IconUrl = "https://f65a-61-30-129-78.ngrok-free.app/UploadFiles/man.png"
                                    }
                                },
                                new TextMessageDto
                                {
                                    Text = "你好，我是客服人員 2號",
                                    Sender = new SenderDto
                                    {
                                        Name = "客服人員 2號",
                                        IconUrl = "https://f65a-61-30-129-78.ngrok-free.app/UploadFiles/gamer.png"
                                    }
                                }
                            }
                        };
                    }
                    break;
            }
            ReplyMessageHandler(replyMessage);
        }

    }
}
