using Microsoft.Extensions.Configuration;
using RestSharp;
using TodoTests.Dtos;
using TodoTests.Interfaces;
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
        
        var response = await _restClient.ExecuteAsync(request);
        return response;
    }

    public async Task<RestResponse> GetAllTodos()
    {
        var response = await _restClient.ExecuteAsync(new RestRequest("/GetTasks"));
        return response;
    }

    public async Task<RestResponse> GetTodoByID(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<RestResponse> EditTodo(TodoDto todoDto)
    {
        var request = new RestRequest("/UpdateTodo", Method.Patch);
        request.AddJsonBody(todoDto);
       
        var response =  await _restClient.ExecuteAsync(request);
        return response;
    }

    public async Task<RestResponse> DeleteTodo(int id)
    {
        var request = new RestRequest($"/DeleteTodo?id={id}", Method.Delete);
        
        var response = await _restClient.ExecuteAsync(request);
        return response;
    }

    public void Dispose()
    {
        _restClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}