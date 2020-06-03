using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SmartBulb.Data.Models;

namespace SmartBulb.TpLinkApi.HttpClients
{
	public interface ITpLinkHttpClient
	{
		/// <summary>
		/// Авторизует пользователя
		/// </summary>
		/// <param name="login"></param>
		/// <param name="password"></param>
		/// <param name="termId"></param>
		/// <returns>Токен</returns>
		Task<string> Authorize(string login, string password, string termId);

		/// <summary>
		/// Создает запрос к серверам TpLink 
		/// </summary>
		/// <param name="token">Токен авторизации</param>
		/// <param name="method">Метод который нужно вызвать</param>
		/// <param name="param">Список параметров которые будут переданны методу</param>
		/// <returns></returns>
		Task<HttpResponseMessage> CreateRequest(string token, string method, Dictionary<string, string> param);
	}
}