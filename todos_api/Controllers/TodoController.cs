using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MyApiProject.Models;
using MyApiProject.Services;

namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            return await _context.GetTodosAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodoById(string id)
        {
            var todo = await _context.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return todo;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo(Todo todo)
        {
            await _context.CreateTodoAsync(todo);
            return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(string id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            var existingTodo = await _context.GetTodoByIdAsync(id);
            if (existingTodo == null)
            {
                return NotFound();
            }

            await _context.UpdateTodoAsync(id, todo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoById(string id)
        {
            var todo = await _context.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            await _context.DeleteTodoAsync(id);
            return NoContent();
        }
    }
}
