using OpenQA.Selenium;

namespace TodoTests.Pages;

public class TodoPage
{
    private WebDriver _driver;
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
    
    public TodoPage(WebDriver driver)
    {
        _driver = driver;
        TestContext.WriteLine("Loading webpage...");
        _driver.Navigate().GoToUrl("http://localhost:4200/todos");
    }

    public void ClickNewTodoButton()
    {
        var createButton =   WaitForElement(_driver,By.Id("createNewTodoBtn"), 3);
        Assert.That(createButton, Is.Not.Null);
        createButton.Click();
    }

    public void EnterTitle(string title)
    {
        var todoTitleTextBox =  WaitForElement(_driver, By.Id("todoTitleTxt"), 5);
        Assert.That(todoTitleTextBox, Is.Not.Null);
        
        todoTitleTextBox.Clear();
        todoTitleTextBox.SendKeys(title);
    }

    public void EnterDescription(string description)
    {
        var todoDescriptionTextBox =
            WaitForElement(_driver, By.Id("todoDescriptionTxt"), 3);
        
        todoDescriptionTextBox.Clear();
        todoDescriptionTextBox.SendKeys(description);
    }

    public void ClickSaveButton()
    {
        var saveButton = WaitForElement(_driver, By.Id("todoSaveBtn"), 3);
        saveButton.Click();
    }

    public async Task<IWebElement> GetTodoElement(int taskID)
    {
        var todoCards = _driver.FindElements(By.CssSelector(".todo-card"));
        var todoCard = todoCards.FirstOrDefault(e => e.GetAttribute("id") == taskID.ToString());

        return todoCard;
    }

    public async Task<bool> GetTodoCardExists(int taskID)
    {
        var cardExists =false;

        try
        {
            var todoCards = _driver.FindElements(By.CssSelector(".todo-card"));
            cardExists = todoCards.Any(e => e.GetAttribute("id") == taskID.ToString());
        }
        catch (StaleElementReferenceException)
        {
            cardExists = false;
        }

        return cardExists;
    }

    public void ClickDeleteButton(IWebElement todoCard)
    {
        var deleteButton = todoCard.FindElement(By.CssSelector(".card-delete-button"));
        deleteButton.Click();
    }

    public void ClickEditButton(IWebElement todoCard)
    {
        var editButton = todoCard.FindElement(By.CssSelector(".card-edit-button"));
        editButton.Click();
    }
}