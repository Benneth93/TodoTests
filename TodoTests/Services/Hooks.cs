using TechTalk.SpecFlow;
namespace TodoTests.Services;

[Binding]
public static class Hooks
{
    [BeforeTestRun]
    public static void SetUp()
    {
        SettingsRetrievalService.SettingsRetrievalServiceConfigure();
    }
}