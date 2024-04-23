using System;
namespace ToDoApp.ViewModels
{
	public class AddTodoItemRequest
	{
		public string Item {get; set;}

		public DateTime DueOn { get; set; }

		public DateTime? CreatedAt { get; set; }
	}
}

