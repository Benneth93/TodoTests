using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoTests.Interfaces;

namespace TodoTests.Services;

public class TodoDatabaseService : ITodoRepository
{
    private ToDoDbContext _todoDbContext;

    public TodoDatabaseService()
    {
        _todoDbContext = new ToDoDbContext(new DbContextOptions<ToDoDbContext>());
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