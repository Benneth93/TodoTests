using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenQA.Selenium;
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
            return await Task.FromException<RestResponse>(e);
        }
    }

    public async Task<RestResponse> GetTodoByID(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<RestResponse> EditTodo(TodoDto todoDto)
    {
        var request = new RestRequest("/UpdateTodo", Method.Patch);
        request.AddJsonBody(todoDto);
        
        try
        {
            var response =  await _restClient.PatchAsync(request);
            return response;
        }
        catch (Exception e)
        {
            return await Task.FromException<RestResponse>(e);
        }
    }

    public async Task<RestResponse> DeleteTodo(int id)
    {
        var request = new RestRequest($"/DeleteTodo?id={id}");

        try
        {
            var response = await _restClient.DeleteAsync(request);
            return response;
        }
        catch (Exception e)
        {
            return await Task.FromException<RestResponse>(e);
        }
    }

    record TodoSingleObject<T>(T data);
    public void Dispose()
    {
        _restClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}