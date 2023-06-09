using LineBot.Enum;

namespace LineBot.Dtos.Messages
{
    public class StickerMessageDto : BaseMessageDto
    {
        public StickerMessageDto()
        {
            Type = MessageEnum.Sticker;
        }
        public string PackageId { get; set; }
        public string StickerId { get; set; }
    }
}
