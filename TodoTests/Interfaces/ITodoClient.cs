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

    public Task<RestResponse> GetTodoByID(int id);

    public Task<RestResponse> EditTodo(TodoDto todoDto);

    public Task<RestResponse> DeleteTodo(int id);
}