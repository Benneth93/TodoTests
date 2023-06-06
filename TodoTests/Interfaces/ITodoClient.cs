using Microsoft.Extensions.Configuration;
using RestSharp;
using TodoTests.Dtos;
using TodoTests.Models;
using TodoTests.Tools;

namespace TodoTests.Interfaces;

public interface ITodoClient
{
    public Task<RestResponse> CreateTodo(CreateTodoDto newTodo);

    public Task<RestResponse> GetAllTodos();

    public TodoModel GetTodoByID(int id);

    public TodoModel EditTodo(int id, TodoDto todoDto);

    public void DeleteTodo(int id);
}