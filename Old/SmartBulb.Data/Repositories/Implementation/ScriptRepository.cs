using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartBulb.Data.Models;
using SmartBulb.Data.Repositories.Abstract;

namespace SmartBulb.Data.Repositories.Implementation
{
	public class ScriptRepository : IScriptRepository
	{
		private readonly DataContext _context;

		public ScriptRepository(DataContext context)
		{
			_context = context;
		}

		public void Add(Script script)
		{
			_context.Scripts.Add(script);
			_context.SaveChanges();
		}

		public IEnumerable<Script> GetAll()
		{
			return _context.Scripts
				.Include(x => x.EndState).ThenInclude(x => x.State)
				.Include(x => x.StartState).ThenInclude(x => x.State)
				.Include(x => x.RepeatedTasks).ThenInclude(x => x.State)
				.ToList();
		}

		public void Delete(Guid id)
		{
			var script = _context.Scripts.Find(id);
			if (script == null) return;
			_context.Scripts.Remove(script);
			_context.SaveChanges();
		}
	}
}