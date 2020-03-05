using Microsoft.EntityFrameworkCore;
using SmartBulb.Data.Models;
using System;

namespace SmartBulb.Data
{
	public class DataContext : DbContext
	{
		public DbSet<Script> Scripts { get; set; }
		public DbSet<SetStateTask> SetStateTask { get; set; }


		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
			base.Database.Migrate();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//Seed(modelBuilder);
			base.OnModelCreating(modelBuilder);
		}
	}
}
