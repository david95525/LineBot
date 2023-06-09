using LineBot.Enum;

namespace LineBot.Dtos.Messages
{
    public class VideoMessageDto : BaseMessageDto
    {
        public VideoMessageDto()
        {
            Type = MessageEnum.Video;
        }

        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get; set; }
        public string? TrackingId { get; set; }
    }
}
