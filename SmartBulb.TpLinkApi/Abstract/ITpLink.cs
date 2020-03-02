using System.Threading.Tasks;
using SmartBulb.TpLinkApi.Implementation;
using SmartBulb.TpLinkApi.Models;

namespace SmartBulb.TpLinkApi.Abstract
{
    public interface ITpLink
    {
        Task<dynamic> GetDeviceList();
        Task SetDeviceState(string deviceId, BulbState bulbState);

        Task GetDeviceState(string deviceId);
    }
}