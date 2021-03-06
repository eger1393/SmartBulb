﻿using Microsoft.EntityFrameworkCore;
using SmartBulb.Data.Models;
using System;

namespace SmartBulb.Data
{
	public class DataContext : DbContext
	{
		public DbSet<Script> Scripts { get; set; }
		public DbSet<BaseTask> BaseTasks { get; set; }
		public DbSet<User> Users { get; set; }

		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
			base.Database.EnsureDeleted();
			base.Database.EnsureCreated();
			//base.Database.Migrate();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasIndex(x => x.Login).IsUnique();
			Seed(modelBuilder);
			base.OnModelCreating(modelBuilder);
		}

		private void Seed(ModelBuilder modelBuilder)
		{
			
		}
	}
}
