using System;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Models
{
	public class TodoDbContext : DbContext
	{
		public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
		{

		}

		public DbSet<Todo> ToDos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<Todo>()
				.Property(prop => prop.Item)
				.HasMaxLength(256);

			base.OnModelCreating(modelBuilder);
        }


    }
}

