using TodoTests.Models;

namespace TodoTests.Dtos;

public class DeletedTodoResponseDto
{
    public string message { get; set; }
    public TodoModel deletedTodo { get; set; }
}