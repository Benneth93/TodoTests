using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoTests.Services;

namespace TodoApp.Data;


public class TodoTask
{
    [Key]
    public int TaskID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
public class ToDoDbContext : DbContext
{
    public DbSet<TodoTask> Tasks { get; set; }
 

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
    {
        
    }
}