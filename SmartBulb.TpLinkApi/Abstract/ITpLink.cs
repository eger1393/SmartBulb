using System.Threading.Tasks;
using SmartBulb.TpLinkApi.Implementation;

namespace SmartBulb.TpLinkApi.Abstract
{
    public interface ITpLink
    {
        Task<dynamic> GetDeviceList();
        Task SetDeviceState(string deviceId, LightState lightState);
    }
}