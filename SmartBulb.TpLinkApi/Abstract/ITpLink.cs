using System.Threading.Tasks;
using SmartBulb.Data.Models;
using SmartBulb.TpLinkApi.Implementation;

namespace SmartBulb.TpLinkApi.Abstract
{
    public interface ITpLink
    {
        Task<dynamic> GetDeviceList();
        Task SetDeviceState(string deviceId, BulbState bulbState);

        Task GetDeviceState(string deviceId);

        Task StartScript(Script script );
    }
}