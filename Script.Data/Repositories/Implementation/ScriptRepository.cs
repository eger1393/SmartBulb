using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Service.Script.Data.Repositories.Abstract;

namespace Service.Script.Data.Repositories.Implementation
{
	public class ScriptRepository : IScriptRepository
	{
		private readonly DataContext _context;

		public ScriptRepository(DataContext context)
		{
			_context = context;
		}

		public void Add(Models.Script script)
		{
			_context.Scripts.Add(script);
			_context.SaveChanges();
		}

		public IEnumerable<Models.Script> GetAll()
		{
			return _context.Scripts
				.Include(x => x.User)
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