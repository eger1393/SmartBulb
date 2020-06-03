using System;
using System.Threading.Tasks;
using SmartBulb.Data.Models;
using SmartBulb.TpLinkApi.Implementation;
using SmartBulb.TpLinkApi.Models;

namespace SmartBulb.TpLinkApi.Abstract
{
    public interface ITpLink
    {
	    /// <summary>
        /// Возвращает список доступных устройств
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <returns></returns>
        Task<ResponseDeviceListPayload> GetDeviceList(Guid userId);

        /// <summary>
        /// Переводит устройство в новое состояние
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="deviceId">Ид устройства</param>
        /// <param name="bulbState">Новое состояние</param>
        /// <returns></returns>
        Task SetDeviceState(Guid userId, string deviceId, BulbState bulbState);
    }
}