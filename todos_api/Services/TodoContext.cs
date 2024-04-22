using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyApiProject.Models;

namespace MyApiProject.Services
{
    public class TodoContext
    {
        private readonly IMongoCollection<Todo> _todos;

        public TodoContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDBSettings"));
            var database = client.GetDatabase(config.GetValue<string>("MongoDBSettings:DatabaseName"));
            _todos = database.GetCollection<Todo>("Todos");
        }

        public async Task<List<Todo>> GetTodosAsync()
        {
            return await _todos.Find(todo => true).ToListAsync();
        }

        public async Task<Todo> GetTodoByIdAsync(string id)
        {
            return await _todos.Find(todo => todo.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateTodoAsync(Todo todo)
        {
            await _todos.InsertOneAsync(todo);
        }

        public async Task UpdateTodoAsync(string id, Todo todo)
        {
            todo.Id = id;
            await _todos.ReplaceOneAsync(t => t.Id == id, todo);
        }

        public async Task DeleteTodoAsync(string id)
        {
            await _todos.DeleteOneAsync(todo => todo.Id == id);
        }
    }
}
