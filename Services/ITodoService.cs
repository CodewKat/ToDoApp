using System;
using ToDoApp.Models;
namespace ToDoApp.Services
{
	public interface ITodoService
	{
		Task<List<Todo>> GetTodos();

		Task<Models.Todo> AddItem(Models.Todo todo);
	}
}

