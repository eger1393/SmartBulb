using System;
using System.Threading.Tasks;
using SmartBulb.Data.Models;
using SmartBulb.TpLinkApi.Implementation;

namespace SmartBulb.TpLinkApi.Abstract
{
    public interface ITpLink
    {
        Task<dynamic> GetDeviceList(Guid userId);
        Task SetDeviceState(Guid userId, string deviceId, BulbState bulbState);

        Task StartScript(Script script );
    }
}