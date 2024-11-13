﻿
using Playwright.DotNet.Components;
using Playwright.DotNet.Find;
using Playwright.DotNet.Validations;
using Playwright.DotNet.Waits;

namespace Playwright.DotNet.PageObjects.Basket.SuccessPage;

/// <summary>
/// Represents the success page of the eShopOnWeb application.
/// </summary>
public class SuccessPage : WebPage, ISuccessPage
{
    private Heading OrderCompleteMessage => App.Component.CreateByTag<Heading>("h1").ToBeVisible();
    private Anchor ContinueShoppingAnchor => App.Component.CreateByDataTestId<Anchor>("success-continue-shopping-anchor").ToBeClickable();

    public override string Url => throw new NotImplementedException();

    /// <inheritdoc/>
    public async Task<ISuccessPage> SuccessMessageShouldBe(string message)
    {
        await OrderCompleteMessage.ValidateToHaveTextAsync(message);

        return this;
    }

    /// <inheritdoc/>
    public async Task<ISuccessPage> ContinueShopping()
    {
        await ContinueShoppingAnchor.ClickAsync();

        return this;
    }
}
