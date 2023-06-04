using Microsoft.Extensions.Configuration;
using TodoTests.Dtos;
using TodoTests.Models;
using TodoTests.Tools;
using RestSharp;
using TodoTests.Interfaces;

namespace TodoTests.Services;

public class TodoApiService : ITodoClient, IDisposable
{
    private string _todoUrl;
    private readonly RestClient _restClient;
    public TodoApiService(IConfiguration configuration)
    {
        _todoUrl = configuration.GetUriString("TodoAPIUrl");
    }

    public Task<TodoModel> CreateTodo(TodoDto newTodo)
    {
        return null;
    }

    public List<TodoModel> GetAllTodos()
    {
        return null;
    }

    public TodoModel GetTodoByID(int id)
    {
        return null;
    }

    public TodoModel EditTodo(int id, TodoDto todoDto)
    {
        return null;
    }

    public void DeleteTodo(int id)
    {
        
    }

    public void Dispose()
    {
        _restClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}