using LineBot.Dtos.Richmenu;

namespace LineBot.Services
{
    public interface IRichMenuService
    {
        void ValidateRichMenu(RichMenuDto richMenu);
        void CreateRichMenu(RichMenuDto richMenu);
        void GetRichMenuList();
        void UploadRichMenuImage(string richMenuId, IFormFile image);
        void SetDefaultRichMenu(string richMenuId);
    }
}
