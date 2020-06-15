using System;
using System.Collections.Generic;
using System.Linq;
using SmartBulb.Data.Models;
using SmartBulb.Data.Repositories.Abstract;

namespace SmartBulb.Data.Repositories.Implementation
{
	public class UserRepository : IUserRepository
	{
		private readonly DataContext _context;

		public UserRepository(DataContext context)
		{
			_context = context;
		}

		public void Add(User user)
		{
			_context.Users.Add(user);
			_context.SaveChanges();
		}

		public IEnumerable<User> GetAll()
		{
			return _context.Users.ToList();
		}

		public void Delete(Guid id)
		{
			var user = _context.Users.Find(id);
			if (user == null) return;
			_context.Users.Remove(user);
		}

		public void SetToken(Guid id, string token)
		{
			var user = _context.Users.Find(id);
			user.Token = token;
			_context.SaveChanges();
		}

		public User GetBy(Guid id)
		{
			return _context.Users.Find(id);
		}
	}
}