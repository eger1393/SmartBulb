using System.Net.Http;
using System.Threading.Tasks;
using SmartBulb.TpLinkApi.Abstract;

namespace SmartBulb.TpLinkApi.Implementation
{
    public class TpLink : ITpLink
    {
        private string TermId { get; set; }
        private string Token { get; set; }
        private readonly HttpClient _client;
        
        public TpLink(HttpClient client)
        {
            this._client = client;
        }
        
        private async Task Authorize()
        {
            var message = new HttpRequestMessage(HttpMethod.Post, "https://wap.tplinkcloud.com");
            //message.Content = 
            await _client.SendAsync(message);
        }
    }
}