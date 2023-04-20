using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TodoApp.Data;
using TodoTests.Interfaces;

namespace TodoTests.Services;

public class TodoDatabaseService : ITodoRepository
{
    private ToDoDbContext _todoDbContext;
    private readonly string _dbConnectionString;

    public TodoDatabaseService(IConfiguration configuration)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ToDoDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("TododbConnectionString"));
        _todoDbContext = new ToDoDbContext(optionsBuilder.Options);
    }
    
    public int GetTodoIdByTitleAndDescription(string title, string description)
    {
        return  _todoDbContext.Tasks.FirstOrDefault(t => t.Title == title && t.Description == description).TaskID;
    }
    
    public bool GetTodoExistsById(int id)
    {
        return _todoDbContext.Tasks.Any(t => t.TaskID == id);
    }
}