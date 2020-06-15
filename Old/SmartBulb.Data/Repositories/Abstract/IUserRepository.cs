using System;
using System.Collections.Generic;
using SmartBulb.Data.Models;

namespace SmartBulb.Data.Repositories.Abstract
{
	public interface IUserRepository
	{
		void Add(User user);

		IEnumerable<User> GetAll();

		void Delete(Guid id);

		void SetToken(Guid id, string token);

		User GetBy(Guid id);
	}
}