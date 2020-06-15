using System;
using System.Collections.Generic;
using SmartBulb.Data.Models;

namespace SmartBulb.Data.Repositories.Abstract
{
	public interface IScriptRepository
	{
		void Add(Script script);

		IEnumerable<Script> GetAll();

		void Delete(Guid id);
	}
}