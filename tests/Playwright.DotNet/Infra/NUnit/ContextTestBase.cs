using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Playwright.DotNet.Infra.NUnit;

/// <summary>
/// Each test will get a fresh copy of a BrowserContext. 
/// You can create as many pages in this context as you'd like. 
/// Using this test is the easiest way to test multi-page scenarios 
/// where you need more than one tab.
/// Note: You can override the ContextOptions method in each test file to control context options, 
/// the ones typically passed into the Browser.NewContextAsync() method. 
/// That way you can specify all kinds of emulation options for your test file individually.
/// </summary>

[TestFixture]
public class ContextTestBase : ContextTest
{
    // Declare Page
    public IPage Page { get; private set; } = null!;

    protected TestContext TestContext => TestContext.CurrentContext;

    public virtual BrowserNewContextOptions ContextOptions()
    {
        return new()
        {
            Locale = "en-US",
            ColorScheme = ColorScheme.Light,
            RecordVideoDir = ".videos"
        };
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
    }

    [SetUp]
    public async Task SetUp()
    {
        await Context.Tracing.StartAsync(new()
        {
            Title = TestContext.CurrentContext.Test.Name,
            Screenshots = true,
            Snapshots = true,
            Sources = true

        });

        Page = await Context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        var failed = TestContext.CurrentContext.Result.Outcome == ResultState.Error
            || TestContext.CurrentContext.Result.Outcome == ResultState.Failure;

        Directory.CreateDirectory("playwright-traces");

        var tracePath = Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            "playwright-traces",
            $"{TestContext.CurrentContext.Test.Name}-{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.zip");

        await Context.Tracing.StopAsync(new()
        {
            Path = failed ? tracePath : null
        });
        TestContext.AddTestAttachment(tracePath, description: "Trace");

        Directory.CreateDirectory("playwright-screenshot");

        if (TestContext.CurrentContext.Result.Outcome == ResultState.Error)
        {
            var screenshotPath = Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                "playwright-screenshot",
                $"{TestContext.CurrentContext.Test.Name}-{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.png");

            await Page.ScreenshotAsync(new()
            {
                Path = screenshotPath,
            });
            TestContext.AddTestAttachment(screenshotPath, description: "Screenshot");
        }

        await Context.CloseAsync();

        Directory.CreateDirectory("playwright-videos");

        var videoPath = Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            "playwright-videos",
            $"{TestContext.CurrentContext.Test.Name}-{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.webm");
        if (Page.Video != null)
        {
            await Page.Video.SaveAsAsync(videoPath);
            TestContext.AddTestAttachment(videoPath, description: "Video");
        }

    }
}