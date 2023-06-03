using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TodoTests.Pages;
using TodoTests.Services;
using TodoTests.Tools;


namespace TodoTests;

[Binding]
public class TodoSteps
{
    private WebDriver _driver;
    private string _todoTitle;
    private string _todoDescription;
    private TodoDatabaseService _todoDbService;
    private int _taskID;

    private TodoPage _todoPage;
    private TodoSteps()
    {
        var configuration = TestHelper.GetIConfigurationRoot(TestContext.CurrentContext.TestDirectory);
        _driver = new ChromeDriver();
        _todoDbService = new TodoDatabaseService(configuration);
        
        _todoTitle = StringTools.GenerateRandomStringOfLength(10);
        _todoDescription = StringTools.GenerateRandomStringOfLength(10);
    }

    [Given(@"I have navigated to the todo page")]
    public void GivenIHaveNavigatedToTheTodoPage()
    {
        _todoPage = new TodoPage(_driver);
    }

    

    [Given(@"I click on the New Todo button")]
    public async void GivenIClickOnTheNewTodoButton()
    {
        _todoPage.ClickNewTodoButton();
    }

    [When(@"I enter a title in the title box")]
    public async void WhenIEnterATitleInTheTitleBox()
    {
        _todoPage.EnterTitle(_todoTitle);
    }

    [When(@"I enter a description in the description box")]
    public async void WhenIEnterADescriptionInTheDescriptionBox()
    {
        _todoPage.EnterDescription(_todoDescription);
    }

    [When(@"I click the save button")]
    public async void WhenIClickTheSaveButton()
    {
       _todoPage.ClickSaveButton();
    }

    [Then(@"The new todo will have saved correctly to the database")]
    public void ThenTheNewTodoWillHaveSavedCorrectlyToTheDatabase()
    { 
        Task.Delay(100);
        _taskID = _todoDbService.GetTodoIdByTitleAndDescription(_todoTitle, _todoDescription);
        Assert.That(_taskID, Is.Not.EqualTo(0));
    }

    

    [Then(@"the todo should no longer exist")]
    public void ThenTheTodoShouldNoLongerExist()
    {
        //ScenarioContext.StepIsPending();
    }

    [Then(@"The new todo exists on the webpage")]
    public async Task WhenTheNewTodoExistsOnTheWebpage()
    {
        Thread.Sleep(10);
        var todoCard = await _todoPage.GetTodoElement(_taskID);
        Assert.That(todoCard, Is.Not.Null);
    }
    
    [When(@"Click the delete button")]
    public async Task WhenClickTheDeleteButton()
    {
        Thread.Sleep(100);
        var todoCard = await _todoPage.GetTodoElement(_taskID);
        _todoPage.ClickDeleteButton(todoCard);
    }

    [Then(@"the todo should not exist in the database")]
    public void ThenTheTodoShouldNotExistInTheDatabase()
    {
        Assert.That(_todoDbService.GetTodoExistsById(_taskID), Is.False);
    }
    
    [Then(@"the todo should no longer exist on the web page")]
    public async Task ThenTheTodoShouldNoLongerExistOnTheWebPage()
    {
        Thread.Sleep(100);
        var cardExists = await _todoPage.GetTodoCardExists(_taskID);
        Assert.That(cardExists, Is.False, $"card should not exist after deletion but did: card #{_taskID}");
    }
    
    [When(@"I click on the edit button")]
    public async Task ThenIClickOnTheUpdateButton()
    {
        Thread.Sleep(300);
        
        var todoCard = await _todoPage.GetTodoElement(_taskID);
        _todoPage.ClickEditButton(todoCard);
    }

    [When(@"I enter details on the update model")]
    public void ThenIEnterDetailsOnTheUpdateModel()
    {
        _todoTitle = StringTools.GenerateRandomStringOfLength(10);
        _todoDescription = StringTools.GenerateRandomStringOfLength(10);
        
        _todoPage.EnterTitle(_todoTitle);
        _todoPage.EnterDescription(_todoDescription);
    }

    
    [Then(@"The updated todo will be saved to the database")]
    public void ThenTheUpdatedTodoWillBeSavedToTheDatabase()
    {
        Thread.Sleep(3000);
        var todo = _todoDbService.GetTaskById(_taskID);
        
        Assert.Multiple(() =>
        {
            Assert.That(todo.Title, Is.EqualTo(_todoTitle));
            Assert.That(todo.Description, Is.EqualTo(_todoDescription));
        });
    }

    [AfterScenario]
    public void TeardownTest()
    {
        _driver.Close();
    }
}