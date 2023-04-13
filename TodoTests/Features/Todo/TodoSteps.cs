using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TodoTests.Services;
using TodoTests.Tools;


namespace TodoTests;

[Binding]
public class TodoSteps
{
    private IWebDriver driver;
    private string _todoTitle;
    private string _todoDescription;
    private TodoDatabaseService _todoDbService;
    private int _taskID;
    
    private TodoSteps()
    {
        driver = new ChromeDriver();
        _todoDbService = new TodoDatabaseService();
        
        _todoTitle = StringTools.GenerateRandomStringOfLength(10);
        _todoDescription = StringTools.GenerateRandomStringOfLength(10);
    }

    [Given(@"I have navigated to the todo page")]
    public void GivenIHaveNavigatedToTheTodoPage()
    {
        TestContext.WriteLine("Loading webpage...");
        driver.Navigate().GoToUrl("http://localhost:4200/todos");
    }

    public static async Task<IWebElement> WaitForElementAsync(IWebDriver driver, By by, TimeSpan timeout)
    {
        var wait = new WebDriverWait(driver, timeout);
        return await wait.Until(async (d) =>
        {
            try
            {
                var element =  d.FindElement(by);
                if (element != null && element.Displayed)
                {
                    return element;
                }
                return null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        });
    }

    [Given(@"I click on the New Todo button")]
    public async void GivenIClickOnTheNewTodoButton()
    {
        var createButton =  await WaitForElementAsync(driver,By.Id("createNewTodoBtn"), TimeSpan.FromSeconds(3));
        createButton.Click();
    }

    [When(@"I enter a title in the title box")]
    public async void WhenIEnterATitleInTheTitleBox()
    {
        var todoTitleTextBox = await WaitForElementAsync(driver, By.Id("todoTitleTxt"), TimeSpan.FromSeconds(3));
        todoTitleTextBox.SendKeys(_todoTitle);
    }

    [When(@"I enter a description in the description box")]
    public async void WhenIEnterADescriptionInTheDescriptionBox()
    {
        var todoDescriptionTextBox =
            await WaitForElementAsync(driver, By.Id("todoDescruotuibTxt"), TimeSpan.FromSeconds(3));
        
        todoDescriptionTextBox.SendKeys(_todoDescription);
    }

    [When(@"I click the save button")]
    public async void WhenIClickTheSaveButton()
    {
        var saveButton = await WaitForElementAsync(driver, By.Id("todoSaveBtn"), TimeSpan.FromSeconds(3));
        saveButton.Click();
    }

    [Then(@"The new todo will have saved correctly to the database")]
    public void ThenTheNewTodoWillHaveSavedCorrectlyToTheDatabase()
    { 
        _taskID = _todoDbService.GetTodoIdByTitleAndDescription(_todoTitle, _todoDescription);
        Assert.That(_taskID, Is.Not.EqualTo(0));
    }

    

    [Then(@"the todo should no longer exist")]
    public void ThenTheTodoShouldNoLongerExist()
    {
        //ScenarioContext.StepIsPending();
    }

    [When(@"The new todo exists on the webpage")]
    public void WhenTheNewTodoExistsOnTheWebpage()
    {
        var todoCards = driver.FindElements(By.CssSelector(".todo-card"));
        var todoCard = todoCards.FirstOrDefault(e => e.GetAttribute("id") == _taskID.ToString());
        
        Assert.That(todoCard, Is.Not.Null);
    }
    
    [When(@"Click the delete button")]
    public void WhenClickTheDeleteButton()
    {
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
        var cardExists = driver.FindElements(By.CssSelector(".todo-card"))
            .FirstOrDefault(e => e.GetAttribute("id") == _taskID.ToString());
       Assert.That(cardExists, Is.Null);
    }
    
    [TearDown]
    public void TeardownTest()
    {
        driver.Close();
    }
}