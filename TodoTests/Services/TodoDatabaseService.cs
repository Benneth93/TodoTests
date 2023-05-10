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
        _todoDbContext.ChangeTracker.QueryTrackingBehavior =QueryTrackingBehavior.NoTracking;
        
    }
    
    public int GetTodoIdByTitleAndDescription(string title, string description)
    {
        var taskID = _todoDbContext.Tasks.FirstOrDefault(t => t.Title == title && t.Description == description).TaskID;
        _todoDbContext.Database.CloseConnection();
        return taskID;
    }

    //Returns task and it's details
    public TodoTask GetTaskById(int id)
    {
        var task = _todoDbContext.Tasks.FirstOrDefault(t => t.TaskID == id);
        return task;
    }
    
    //returns True if the task exists in the database
    public bool GetTodoExistsById(int id)
    {
        var exists =_todoDbContext.Tasks.Any(t => t.TaskID == id);
        return exists;
    }
}