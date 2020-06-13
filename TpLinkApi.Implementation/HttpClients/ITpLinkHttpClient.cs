using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TpLinkApi.Implementation.HttpClients
{
	public interface ITpLinkHttpClient
	{
		/// <summary>
		/// Авторизует пользователя
		/// </summary>
		/// <param name="login"></param>
		/// <param name="password"></param>
		/// <returns>Токен</returns>
		Task<string> Authorize(string login, string password);

		/// <summary>
		/// Вызывает метод Passthrough который меняет состояние устройства
		/// </summary>
		/// <param name="token">Токен авторизации</param>
		/// <param name="deviceId">Ид устройства</param>
		/// <param name="command">Сеарелизованная команда</param>
		/// <returns></returns>
		Task<string> SendPassthroughRequest(string token, string deviceId, string command);

		/// <summary>
		/// Создает запрос к серверам TpLink 
		/// </summary>
		/// <param name="token">Токен авторизации</param>
		/// <param name="method">Метод который нужно вызвать</param>
		/// <param name="param">Список параметров которые будут переданны методу</param>
		/// <returns></returns>
		Task<string> SendRequest(string token, string method, Dictionary<string, string> param);


	}
}