using System;
using ToDoApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Services

{
	public class TodoService : ITodoService
	{
		private readonly TodoDbContext _context;


		public TodoService(TodoDbContext context)
		{
			_context = context;
		}

        public async Task<Todo> AddItem(Todo todo)
        {
			if(todo.Id == 0)
			{
				_context.Add(todo).State = EntityState.Added;
			}
			else
			{
				_context.Add(todo);
			}

			await _context.SaveChangesAsync();
			return todo;
  
        }

        public async Task<List<Todo>> GetTodos()
        {
			return await _context.ToDos.ToListAsync();
        }
       
    }
}

