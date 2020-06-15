using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TpLinkApi.Implementation.Models;

namespace TpLinkApi.Implementation
{
    public interface ITpLink
    {
		/// <summary>
		/// Авторизация пользователя
		/// </summary>
		/// <param name="login"></param>
		/// <param name="password"></param>
		/// <returns>токен</returns>
	    Task<string> Authorize(string login, string password);

		/// <summary>
		/// Получение списка устройств
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		Task<List<Device>> GetDeviceList(string token);

        /// <summary>
		/// Установка нового состояния для устройства
		/// </summary>
		/// <param name="token"></param>
		/// <param name="deviceId"></param>
		/// <param name="newBulbState"></param>
		/// <returns></returns>
        Task SetDeviceState(string token, string deviceId, BulbState newBulbState);
    }
}