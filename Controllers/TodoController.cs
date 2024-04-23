using System;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Services;
using ToDoApp.ViewModels;
using ToDoApp.Models;
using Microsoft.EntityFrameworkCore;
using Azure.Core;

namespace ToDoApp.Controllers;

public class TodoController : Controller
{
	private readonly TodoDbContext _context;
	private readonly ITodoService _todoService;

	public TodoController(TodoDbContext context, ITodoService todoService)
	{
		_context = context;
		_todoService = todoService;
	}

	//GET: Todo
	public async Task<IActionResult> Index()
	{
		var todos = await _todoService.GetTodos();
		return View(todos);
	}

    //GET: Todo/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.ToDos == null)
        {
            return NotFound();
        }

        var todo = await _context.ToDos
            .FirstOrDefaultAsync(m => m.Id == id);

        if (todo == null)
        {
            return NotFound();
        }

        return View(todo);
    }

    //GET: Todo/Add
    public IActionResult Add()
	{
		return View();
	}

	//POST: Todo/Add
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Add(AddTodoItemRequest request)
	{
        if (!ModelState.IsValid)
        {
            return View(request);
        }

		var todo = new Todo
		{
			Item = request.Item,
			DueOn = request.DueOn,
			CreatedAt = DateTime.Now

        };

        _context.ToDos.Add(todo);
        await _context.SaveChangesAsync();

		return RedirectToAction(nameof(Index));

    }

	//GET: Todo/Edit/5
	public async Task<IActionResult> Edit(int? id)
	{
		if(id == null || _context.ToDos == null)
		{
			return NotFound();
		}

		var item = await _context.ToDos.FindAsync(id);

		if(item == null)
		{
			return NotFound();
		}
		return View(item);
	}

	//POST: Todo/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, [Bind("Id, Item, IsCompleted, DueOn, CreatedAt, UpdatedAt")] Todo todo)
	{
		if(id != todo.Id)
		{
			return NotFound();
		}

        if (ModelState.IsValid)
		{
			try
			{
				todo.UpdatedAt = DateTime.Now;
				_context.Update(todo);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ToDoExists(todo.Id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
			return RedirectToAction(nameof(Index));
		}
		return View(todo);
	}

	//GET: Todo/Delete/5
	public async Task<IActionResult> Delete(int? id)
	{
        if (id == null || _context.ToDos == null)
        {
            return NotFound();
        }

        var todo = await _context.ToDos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (todo == null)
        {
            return NotFound();
        }

        return View(todo);
    }

	//POST: Todo/Delete/5
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.ToDos == null)
        {
            return Problem("Entity set 'TodoDbContext.Todos'  is null.");
        }
        var todo = await _context.ToDos.FindAsync(id);
        if (todo != null)
        {
            _context.ToDos.Remove(todo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ToDoExists(int id)
	{
		return (_context.ToDos?.Any(e => e.Id == id)).GetValueOrDefault();
	}
}
