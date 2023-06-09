namespace LineBot.Dtos.Webhook
{
    public class MessageEventDto
    {
        public string Id { get; set; }
        public string Type { get; set; }

        // Text Message Event
        public string? Text { get; set; }
        public List<TextMessageEventEmojiDto>? Emojis { get; set; }
        public TextMessageEventMentionDto? Mention { get; set; }
        //Image Message Event
        public ContentProviderDto? ContentProvider { get; set; }
        public ImageMessageEventImageSetDto? ImageSet { get; set; }
        //video  Audio
        public int? Duration { get; set; } // 影片 or 音檔時長(單位：豪秒)
        //File Message Event
        public string? FileName { get; set; }
        public int? FileSize { get; set; }
        //Location Message Event 
        public string? Title { get; set; }
        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        // Sticker Message Event
        public string? PackageId { get; set; }
        public string? StickerId { get; set; }
        public string? StickerResourceType { get; set; }
        public List<string>? Keywords { get; set; }
    }

    //text
    public class TextMessageEventEmojiDto
    {
        public int Index { get; set; }
        public int Length { get; set; }
        public string ProductId { get; set; }
        public string EmojiId { get; set; }
    }
    public class TextMessageEventMentionDto
    {
        public List<TextMessageEventMentioneeDto> Mentionees { get; set; }
    }

    public class TextMessageEventMentioneeDto
    {
        public int Index { get; set; }
        public int Length { get; set; }
        public string UserId { get; set; }
    }
    //image
    public class ContentProviderDto
    {
        public string Type { get; set; }
        public string? OriginalContentUrl { get; set; }
        public string? PreviewImageUrl { get; set; }
    }

    public class ImageMessageEventImageSetDto
    {
        public string Id { get; set; }
        public string Index { get; set; }
        public string Total { get; set; }
    }

}
