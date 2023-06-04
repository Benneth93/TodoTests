using Microsoft.Extensions.Configuration;
using TodoTests.Dtos;
using TodoTests.Models;
using TodoTests.Tools;

namespace TodoTests.Interfaces;

public interface ITodoClient
{
    //public void TodoAPIService(IConfiguration configuration);

    public Task<TodoModel> CreateTodo(TodoDto newTodo);

    public List<TodoModel> GetAllTodos();

    public TodoModel GetTodoByID(int id);

    public TodoModel EditTodo(int id, TodoDto todoDto);

    public void DeleteTodo(int id);
}