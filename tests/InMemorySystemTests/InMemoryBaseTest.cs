
using Autofac;
using Azure;
using Microsoft.Playwright;
using Playwright.DotNet.Fixtures;
using Playwright.DotNet.Infra.NUnit;

namespace EShopOnWeb.InMemorySystemTests;

public class InMemoryBaseTest : PageTestBase
{
    protected SystemTestFixture _fixture;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _fixture = GetSystemTestFixture();

    }

    [SetUp]
    public async Task SetUp()
    {
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _fixture.Dispose();

    }

    private SystemTestFixture GetSystemTestFixture()
    {
        return new SystemTestFixture();
    }

}
