using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TodoTests.Services;
using TodoTests.Tools;


namespace TodoTests;

[Binding]
public class TodoUISteps
{
    private IWebDriver driver;
    private string _todoTitle;
    private string _todoDescription;
    private TodoDatabaseService _todoDbService;
    private int _taskID;
    
    private TodoUISteps()
    {
        var configuration = TestHelper.GetIConfigurationRoot(TestContext.CurrentContext.TestDirectory);
        driver = new ChromeDriver();
        _todoDbService = new TodoDatabaseService(configuration);
        
        _todoTitle = StringTools.GenerateRandomStringOfLength(10);
        _todoDescription = StringTools.GenerateRandomStringOfLength(10);
    }

    [Given(@"I have navigated to the todo page")]
    public void GivenIHaveNavigatedToTheTodoPage()
    {
        TestContext.WriteLine("Loading webpage...");
        driver.Navigate().GoToUrl("http://localhost:4200/todos");
    }

    public static IWebElement WaitForElement(IWebDriver driver, By by, int timeout)
    {
        IWebElement element = null;
        
        Task.Run(() =>
        {
            while (element == null)
            {
                element = driver.FindElement(by);
                
                if (element.Displayed) return;
                element = null;
            }
        }).Wait(TimeSpan.FromSeconds(timeout));

        return element;
    }

    [Given(@"I click on the New Todo button")]
    public async void GivenIClickOnTheNewTodoButton()
    {
        var createButton =   WaitForElement(driver,By.Id("createNewTodoBtn"), 3);
        Assert.That(createButton, Is.Not.Null);
        createButton.Click();
    }

    [When(@"I enter a title in the title box")]
    public async void WhenIEnterATitleInTheTitleBox()
    {
        var todoTitleTextBox =  WaitForElement(driver, By.Id("todoTitleTxt"), 5);
        Assert.That(todoTitleTextBox, Is.Not.Null);
        todoTitleTextBox.SendKeys(_todoTitle);
    }

    [When(@"I enter a description in the description box")]
    public async void WhenIEnterADescriptionInTheDescriptionBox()
    {
        var todoDescriptionTextBox =
             WaitForElement(driver, By.Id("todoDescriptionTxt"), 3);
        
        todoDescriptionTextBox.SendKeys(_todoDescription);
    }

    [When(@"I click the save button")]
    public async void WhenIClickTheSaveButton()
    {
        var saveButton = WaitForElement(driver, By.Id("todoSaveBtn"), 3);
        saveButton.Click();
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
    public void WhenTheNewTodoExistsOnTheWebpage()
    {
        Thread.Sleep(10);
        var todoCards = driver.FindElements(By.CssSelector(".todo-card"));
        var todoCard = todoCards.FirstOrDefault(e => e.GetAttribute("id") == _taskID.ToString());
        
        Assert.That(todoCard, Is.Not.Null);
    }
    
    [When(@"Click the delete button")]
    public void WhenClickTheDeleteButton()
    {
        Thread.Sleep(100);
        var todoCard = driver.FindElements(By.CssSelector(".todo-card"))
            .FirstOrDefault(e => e.GetAttribute("id") == _taskID.ToString());
        var deleteButton = todoCard.FindElement(By.CssSelector(".card-delete-button"));
        
        deleteButton.Click();
    }

    [Then(@"the todo should not exist in the database")]
    public void ThenTheTodoShouldNotExistInTheDatabase()
    {
        Assert.That(_todoDbService.GetTodoExistsById(_taskID), Is.False);
    }
    
    [Then(@"the todo should no longer exist on the web page")]
    public void ThenTheTodoShouldNoLongerExistOnTheWebPage()
    {
        Thread.Sleep(100);
        var cardExists =false;

        try
        {
            var todoCards = driver.FindElements(By.CssSelector(".todo-card"));
            cardExists = todoCards.Any(e => e.GetAttribute("id") == _taskID.ToString());
        }
        catch (StaleElementReferenceException)
        {
            cardExists = false;
        }
        Assert.That(cardExists, Is.False, $"card should not exist after deletion but did: card #{_taskID}");
    }
    
    [When(@"I click on the edit button")]
    public void ThenIClickOnTheUpdateButton()
    {
        Thread.Sleep(100);
        var todoCard = driver.FindElements(By.CssSelector(".todo-card"))
            .FirstOrDefault(e => e.GetAttribute("id") == _taskID.ToString());
        var editButton = todoCard.FindElement(By.CssSelector(".card-edit-button"));
        
        editButton.Click();
    }

    [When(@"I enter details on the update model")]
    public void ThenIEnterDetailsOnTheUpdateModel()
    {
        _todoTitle = StringTools.GenerateRandomStringOfLength(10);
        _todoDescription = StringTools.GenerateRandomStringOfLength(10);
        
        var todoTitleTextBox =  WaitForElement(driver, By.Id("todoTitleTxt"), 5);
        Assert.That(todoTitleTextBox, Is.Not.Null);
        todoTitleTextBox.Clear();
        todoTitleTextBox.SendKeys(_todoTitle);
        
        var todoDescriptionTextBox =
            WaitForElement(driver, By.Id("todoDescriptionTxt"), 3);
        
        todoDescriptionTextBox.Clear();
        todoDescriptionTextBox.SendKeys(_todoDescription);
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
        driver.Close();
    }
}