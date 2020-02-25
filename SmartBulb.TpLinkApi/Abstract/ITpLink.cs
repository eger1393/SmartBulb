using System.Threading.Tasks;

namespace SmartBulb.TpLinkApi.Abstract
{
    public interface ITpLink
    {
        Task<dynamic> GetDeviceList();
    }
}