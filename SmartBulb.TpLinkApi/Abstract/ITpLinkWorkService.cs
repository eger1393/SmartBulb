using System.Threading.Tasks;
using SmartBulb.Data.Models;

namespace SmartBulb.TpLinkApi.Abstract
{
	public interface ITpLinkWorkService
	{
		/// <summary>
		/// Запускает скрипт
		/// </summary>
		/// <param name="script"></param>
		/// <returns></returns>
		Task StartScript(Script script);
	}
}