using TodoTests.Models;

namespace TodoTests.Dtos;

public class EditTodoResponseDto
{
    public string message { get; set; }
    public TodoModel todo { get; set; }
}