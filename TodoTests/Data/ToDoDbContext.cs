using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = SettingsRetrievalService.TodoDbConnectionString;
        optionsBuilder.UseSqlServer(connectionString);
    }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
    {
        
    }
}