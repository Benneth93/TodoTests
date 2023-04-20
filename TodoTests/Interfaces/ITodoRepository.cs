using Microsoft.Extensions.Configuration;
using TodoApp.Data;

namespace TodoTests.Interfaces;

public interface ITodoRepository
{
    public int GetTodoIdByTitleAndDescription(string title, string description);
    public bool GetTodoExistsById(int id);
}