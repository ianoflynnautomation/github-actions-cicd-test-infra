﻿
using Playwright.DotNet.SyncPlaywright.Core;
using Playwright.DotNet.SyncPlaywright.Core.Elements;

namespace Playwright.DotNet.Find;

public class FindCssStrategy(string value) : FindStrategy(value)
{
    public override WebElement Resolve(BrowserPage searchContext)
    {
        if (!Value.StartsWith("css=")) return searchContext.Locate($"css={Value}");
        return searchContext.Locate($"{Value}");
    }

    public override WebElement Resolve(WebElement searchContext)
    {
        if (!Value.StartsWith("css=")) return searchContext.Locate($"css={Value}");
        return searchContext.Locate($"{Value}");
    }

    public override string ToString()
    {
        return $"CSS = {Value}";
    }
}