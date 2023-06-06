using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using TodoTests.Dtos;
using TodoTests.Interfaces;
using TodoTests.Models;
using TodoTests.Tools;

namespace TodoTests.Clients;

public class TodoClient : ITodoClient, IDisposable
{
    private string _todoUrl;
    private readonly RestClient _restClient;
    
    public TodoClient(IConfiguration configuration)
    {
        _todoUrl = configuration.GetUriString("TodoAPIUrl");
        
        var options = new RestClientOptions(_todoUrl);
        _restClient = new RestClient(options);
    }

    public async Task<RestResponse> CreateTodo(CreateTodoDto newTodo)
    {
        var request = new RestRequest("/CreateNewTodo", Method.Post);
        request.AddJsonBody(newTodo);
        
        try
        {
           var response = await _restClient.PostAsync(request);
           return response;
        }
        catch (Exception e)
        {
            return await Task.FromException<RestResponse>(new Exception(e.Message));
        }
    }

    public async Task<RestResponse> GetAllTodos()
    {
        try
        {
            var response = await _restClient.GetAsync(new RestRequest("/GetTasks"));
            return response;
        }
        catch (Exception e)
        {
            return await Task.FromException<RestResponse>(new Exception(e.Message));
        }
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

    record TodoSingleObject<T>(T data);
    public void Dispose()
    {
        _restClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}