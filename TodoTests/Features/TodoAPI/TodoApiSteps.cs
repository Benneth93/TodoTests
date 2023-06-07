using Newtonsoft.Json;
using RestSharp;
using TechTalk.SpecFlow;
using TodoTests.Clients;
using TodoTests.Dtos;
using TodoTests.Models;
using TodoTests.Services;

namespace TodoTests.Features.TodoAPI;

[Binding]
public class TodoApi
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    private readonly TodoClient _todoService;
    
    //TODO: Add responses to _scenarioContext
    private RestResponse _createTodoResponse;
    private RestResponse _editTodoResponse;
    private RestResponse _deleteTodoResponse;

    public TodoApi(ScenarioContext scenarioContext)
    {
        var configuration = TestHelper.GetIConfigurationRoot(TestContext.CurrentContext.TestDirectory);

        _scenarioContext = scenarioContext;
        _todoService = new TodoClient(configuration);
    }

    [Given(@"I have a new task to create")]
    public void GivenIHaveANewTaskToCreate()
    {
        var data = new CreateTodoDto()
        {
            Title = "Title: API Test",
            Description = "Description: API Test"
        };
        _scenarioContext.Add("createTodoDto",data);
        
    }

    [When(@"I send a request to create a new todo with the task details")]
    public async Task WhenISendARequestToCreateANewTodoWithTheTaskDetails()
    {
        _createTodoResponse = await _todoService.CreateTodo(_scenarioContext.Get<CreateTodoDto>("createTodoDto"));
    }

    [Then(@"I should get a successful response")]
    public void ThenIShouldGetASuccessfulResponse()
    {
        Assert.That(_createTodoResponse.IsSuccessStatusCode, Is.True);

        _scenarioContext.Add("CreateTodoResponseModel", JsonConvert.DeserializeObject <TodoModel>(_createTodoResponse.Content));
    }

    [Then(@"The response should contain an ID")]
    public void ThenTheResponseShouldContainAnId()
    {
        var todo = _scenarioContext.Get<TodoModel>("CreateTodoResponseModel");
        Assert.That(todo.TaskID, Is.Not.EqualTo(0));
    }

    [Then(@"The response should contain the correct details of the task")]
    public void ThenTheResponseShouldContainTheCorrectDetailsOfTheTask()
    {
        var expected = _scenarioContext.Get<CreateTodoDto>("createTodoDto");
        var actual = _scenarioContext.Get<TodoModel>("CreateTodoResponseModel");
        
        Assert.Multiple(() =>
        {
            Assert.That(actual.Title, Is.EqualTo(expected.Title));
            Assert.That(actual.Description, Is.EqualTo(expected.Description));
        });
    }

    [Given(@"I have an existing todo")]
    public async Task GivenIHaveAnExistingTodo()
    {
        _scenarioContext.Add("CreateTodoResponseModel", await CreateTodoData());
    }

    [Given(@"Data ready to edit that todo")]
    public void GivenDataReadyToEditThatTodo()
    {
        var editedTodo = new TodoDto()
        {
            TaskID = Convert.ToInt32(_scenarioContext.Get<TodoModel>("CreateTodoResponseModel").TaskID),
            Title = "Edited Title",
            Description = "Edited Description"
        };
        
        _scenarioContext.Add("EditedTodo", editedTodo);
    }

    [When(@"I send the request to edit the todo")]
    public async Task WhenISendTheRequestToEditTheTodo()
    {
        _editTodoResponse = await _todoService.EditTodo(_scenarioContext.Get<TodoDto>("EditedTodo"));
    }


    [Then(@"The response should be successful from the edit")]
    public void ThenTheResponseShouldBeSuccessful()
    {
        Assert.That(_editTodoResponse.IsSuccessStatusCode, Is.True);
        _scenarioContext.Add("EditedTodoResponseDto",JsonConvert.DeserializeObject<EditTodoResponseDto>(_editTodoResponse.Content));
    }

    [Then(@"The details of the todo should contain the details of the edit")]
    public void ThenTheDetailsOfTheTodoShouldContainTheDetailsOfTheEdit()
    {
        var expected = _scenarioContext.Get<TodoDto>("EditedTodo");
        var actual = _scenarioContext.Get<EditTodoResponseDto>("EditedTodoResponseDto");
        Assert.Multiple(() =>
        {
            Assert.That(actual.updatedTodo.TaskID, Is.EqualTo(expected.TaskID));
            Assert.That(actual.updatedTodo.Title, Is.EqualTo(expected.Title));
            Assert.That(actual.updatedTodo.Description, Is.EqualTo(expected.Description));
        });
    }
    
    [When(@"I send a request to delete that todo")]
    public async Task WhenISendARequestToDeleteThatTodo()
    {
        _deleteTodoResponse = await _todoService.DeleteTodo(_scenarioContext.Get<TodoModel>("CreateTodoResponseModel").TaskID);
    }

    [Then(@"I should get a successful response from the delete")]
    public void ThenIShouldGetASuccessfulResponseFromTheDelete()
    {
        Assert.That(_deleteTodoResponse.IsSuccessStatusCode, Is.True);
        _scenarioContext.Add("deletedTodo", JsonConvert.DeserializeObject<DeletedTodoResponseDto>(_deleteTodoResponse.Content));
    }

    [Then(@"the details of the todo deleted should be correct")]
    public void ThenTheDetailsOfTheTodoDeletedShouldBeCorrect()
    {
        var expected = _scenarioContext.Get<TodoModel>("CreateTodoResponseModel");
        var actual = _scenarioContext.Get<DeletedTodoResponseDto>("deletedTodo");
        
        Assert.Multiple(() =>
        {
            Assert.That(actual.deletedTodo.TaskID, Is.EqualTo(expected.TaskID));
            Assert.That(actual.deletedTodo.Title, Is.EqualTo(expected.Title));
            Assert.That(actual.deletedTodo.Description, Is.EqualTo(expected.Description));
        });
    }

    private async Task<TodoModel> CreateTodoData()
    {
        var requestDto = new CreateTodoDto()
        {
            Title = "Title: API Test",
            Description = "Description: API Test"
        };

        var response = await _todoService.CreateTodo(requestDto);
        Assert.That(response.IsSuccessStatusCode, Is.True);

        return JsonConvert.DeserializeObject<TodoModel>(response.Content);
    }

    [AfterScenario("CleardownTodo")]
    private async Task DeleteTodo()
    {
        var response = await _todoService.DeleteTodo(_scenarioContext.Get<TodoModel>("CreateTodoResponseModel").TaskID);
        Assert.That(response.IsSuccessStatusCode);
    }
    
}