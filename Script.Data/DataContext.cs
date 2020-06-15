using Microsoft.EntityFrameworkCore;
using Service.Script.Data.Models;

namespace Service.Script.Data
{
	public class DataContext : DbContext
	{
		public DbSet<Models.Script> Scripts { get; set; }
		public DbSet<BaseTask> BaseTasks { get; set; }
		public DbSet<User> Users { get; set; }

		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
			base.Database.EnsureDeleted();
			base.Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			Seed(modelBuilder);
			base.OnModelCreating(modelBuilder);
		}

		private void Seed(ModelBuilder modelBuilder)
		{
		}
	}
}
