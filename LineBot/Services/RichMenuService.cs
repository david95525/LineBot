using LineBot.Dtos.Richmenu;
using LineBot.Providers;

namespace LineBot.Services
{
    public class RichMenuService:IRichMenuService
    {
        // 貼上 messaging api channel 中的 accessToken & secret
        private readonly string channelAccessToken = "YourAccessToken";
        private readonly string channelSecret = "YourChannelSecret";

        private static HttpClient client = new HttpClient();
        private readonly JsonProvider _jsonProvider = new JsonProvider();

        public RichMenuService()
        {
        }

        public async void ValidateRichMenu(RichMenuDto richMenu)
        {

        }

        public async void CreateRichMenu(RichMenuDto richMenu)
        {

        }

        public async void GetRichMenuList()
        {

        }

        public async void UploadRichMenuImage(string richMenuId, IFormFile image)
        {

        }

        public async void SetDefaultRichMenu(string richMenuId)
        {

        }
    }
}
