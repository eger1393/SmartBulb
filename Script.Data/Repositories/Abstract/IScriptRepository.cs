using System;
using System.Collections.Generic;

namespace Service.Script.Data.Repositories.Abstract
{
	public interface IScriptRepository
	{
		void Add(Service.Script.Data.Models.Script script);

		IEnumerable<Service.Script.Data.Models.Script> GetAll();

		void Delete(Guid id);
	}
}