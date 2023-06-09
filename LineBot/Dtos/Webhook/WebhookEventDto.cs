namespace LineBot.Dtos.Webhook
{
    public class WebhookEventDto
    {
        //common property
        // 事件類型
        public string Type { get; set; }
        // Channel state : active | standby
        public string Mode { get; set; }
        // 事件發生時間 : event occurred time in milliseconds
        public long Timestamp { get; set; }
        // 事件來源 : user | group chat | multi-person chat
        public SourceDto Source { get; set; }
        // webhook event id - ULID format
        public string WebhookEventId { get; set; }
        // 是否為重新傳送之事件 DeliveryContext.IsRedelivery : true | false
        public DeliverycontextDto DeliveryContext { get; set; }

        //Text
        // 回覆此事件所使用的 token
        public string? ReplyToken { get; set; }
        // 收到訊息的事件，可收到 text、sticker、image、file、video、audio、location 訊息
        public MessageEventDto? Message { get; set; }
        //unused 使用者“收回”訊息事件
        public UnsendEventDto? Unsend { get; set; } 
        // Memmber Joined Event
        public MemberEventDto? Joined { get; set; } 
        // Member Leave Event
        public MemberEventDto? Left { get; set; } 
        // Postback Event
        public PostbackEventDto? Postback { get; set; }
        // Video viewing complete event
        public VideoViewingCompleteEventDto? VideoPlayComplete { get; set; } 
    }
    public class SourceDto
    {
        public string Type { get; set; }
        public string? UserId { get; set; }
        public string? GroupId { get; set; }
        public string? RoomId { get; set; }
    }
    public class DeliverycontextDto
    {
        public bool IsRedelivery { get; set; }
    }
    //Unsend
    public class UnsendEventDto
    {
        public string messageId { get; set; }
    }
    //Member Event
    public class MemberEventDto
    {
        public List<SourceDto> Members { get; set; }
    }
    public class PostbackEventDto
    {
        public string? Data { get; set; }
        public PostbackEventParamDto? Params { get; set; }
    }
    //Postback Event
    public class PostbackEventParamDto
    {
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? Datetime { get; set; }
        public string? NewRichMenuAliasId { get; set; }
        public string? Status { get; set; }
    }
    public class VideoViewingCompleteEventDto
    {
        public string TrackingId { get; set; }
    }
}
