using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace TodoTests.Features.TodoAPI;

[Binding]
public sealed class TodoAPI
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;

    public TodoAPI(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"I have a new task to create")]
    public void GivenIHaveANewTaskToCreate()
    {
        ScenarioContext.StepIsPending();
    }

    [When(@"I send a request to create a new todo with the task details")]
    public void WhenISendARequestToCreateANewTodoWithTheTaskDetails()
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"I should get a successful response")]
    public void ThenIShouldGetASuccessfulResponse()
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"The response should contain an ID")]
    public void ThenTheResponseShouldContainAnId()
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@"The response should contain the correct details of the task")]
    public void ThenTheResponseShouldContainTheCorrectDetailsOfTheTask()
    {
        ScenarioContext.StepIsPending();
    }
}