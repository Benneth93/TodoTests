using Newtonsoft.Json;
using RestSharp;
using TechTalk.SpecFlow;
using TodoTests.Clients;
using TodoTests.Dtos;
using TodoTests.Models;
using TodoTests.Services;

namespace TodoTests.Features.TodoAPI;

[Binding]
public class TodoAPI
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    private readonly TodoClient _todoService;
    private CreateTodoDto _createTodoDto;
    private RestResponse _createTodoResponse;
    private TodoModel _createTodoResponseModel;
    public TodoAPI(ScenarioContext scenarioContext)
    {
        var configuration = TestHelper.GetIConfigurationRoot(TestContext.CurrentContext.TestDirectory);

        _scenarioContext = scenarioContext;
        _todoService = new TodoClient(configuration);
    }

    [Given(@"I have a new task to create")]
    public void GivenIHaveANewTaskToCreate()
    {
        _createTodoDto = new CreateTodoDto()
        {
            Title = "Title: API Test",
            Description = "Descrition: API Test"
        };
    }

    [When(@"I send a request to create a new todo with the task details")]
    public async Task WhenISendARequestToCreateANewTodoWithTheTaskDetails()
    {
        _createTodoResponse = await _todoService.CreateTodo(_createTodoDto);
    }

    [Then(@"I should get a successful response")]
    public void ThenIShouldGetASuccessfulResponse()
    {
        Assert.That(_createTodoResponse.IsSuccessStatusCode, Is.True);

        _createTodoResponseModel = JsonConvert.DeserializeObject <TodoModel>(_createTodoResponse.Content);
        _scenarioContext["CreatedTodoID"] = _createTodoResponseModel.TaskID;
    }

    [Then(@"The response should contain an ID")]
    public void ThenTheResponseShouldContainAnId()
    {
        Assert.That(_createTodoResponseModel.TaskID, Is.Not.EqualTo(0));
    }

    [Then(@"The response should contain the correct details of the task")]
    public void ThenTheResponseShouldContainTheCorrectDetailsOfTheTask()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_createTodoResponseModel.Title, Is.EqualTo(_createTodoDto.Title));
            Assert.That(_createTodoResponseModel.Description, Is.EqualTo(_createTodoDto.Description));
        });
    }

    [Given(@"I have an existing todo")]
    public void GivenIHaveAnExistingTodo()
    {
        ScenarioContext.StepIsPending();
    }

    [Given(@"Data ready to edit that todo")]
    public void GivenDataReadyToEditThatTodo()
    {
        ScenarioContext.StepIsPending();
    }

    [When(@"I send the request to edit the todo")]
    public void WhenISendTheRequestToEditTheTodo()
    {
        ScenarioContext.StepIsPending();
    }


    [Then(@"The response should be successful")]
    public void ThenTheResponseShouldBeSuccessful()
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"The details of the todo should contain the details of the edit")]
    public void ThenTheDetailsOfTheTodoShouldContainTheDetailsOfTheEdit()
    {
        ScenarioContext.StepIsPending();
    }
}