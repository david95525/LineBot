using LineBot.Enum;

namespace LineBot.Dtos.Messages
{
    public class ImageMessageDto : BaseMessageDto
    {
        public ImageMessageDto()
        {
            Type = MessageEnum.Image;
        }

        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get; set; }
    }
}
