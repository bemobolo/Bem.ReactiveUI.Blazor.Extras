// --------------------------------------
// <copyright file="ComponentWithTwoWayBindingTests.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using AngleSharp.Html.Dom;
using Bem.ReactiveUI.Blazor.Extras.Tests.ViewModels;
using Bunit;
using NUnit.Framework;

namespace Bem.ReactiveUI.Blazor.Extras.Tests.Components;

public class ComponentWithTwoWayBindingTests : BunitTestContext
{
    private StringsViewModel _viewModel = default!;

    public override void Setup()
    {
        base.Setup();

        _viewModel = new();
    }

    [Test]
    public void Rerenders_Component_When_Subscribed_And_Input_Value_Changes_Even_If_ForceRenderOnEvent_Disabled()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithTwoWayBinding>(ComponentParameter.CreateParameter("ViewModel", _viewModel));

        component.Instance.ForceRenderOnEvent = false;
        component.Find("input#first").As<IHtmlInputElement>()!.Change("first");
        component.Find("input#city").As<IHtmlInputElement>()!.Change("city");

        AssertComponent(component, first: "first", last: string.Empty, city: "city", renderCount: 3);
    }

    [Test]
    public void Does_Not_Rerender_Component_When_Not_Subscribed_And_ForceRenderOnEvent_Disabled_And_Input_Value_Changes()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithTwoWayBinding>(ComponentParameter.CreateParameter("ViewModel", _viewModel));

        component.Instance.ForceRenderOnEvent = false;
        component.Find("input#last").As<IHtmlInputElement>()!.Change("last");

        AssertComponent(component, first: string.Empty, last: "last", city: string.Empty, renderCount: 1, last2: string.Empty);
    }

    private static void AssertComponent(
        IRenderedComponent<ComponentWithTwoWayBinding> component, string first, string last, string city, int renderCount, string? first2 = null, string? last2 = null, string? city2 = null)
    {
        Assert.That(component.Instance.RenderCount, Is.EqualTo(renderCount));

        Assert.That(component.Find("input#first").As<IHtmlInputElement>()!.Value, Is.EqualTo(first));
        Assert.That(component.Find("input#first2").As<IHtmlInputElement>()!.Value, Is.EqualTo(first2 ?? first));
        Assert.That(component.Find("input#last").As<IHtmlInputElement>()!.Value, Is.EqualTo(last));
        Assert.That(component.Find("input#last2").As<IHtmlInputElement>()!.Value, Is.EqualTo(last2 ?? last));
        Assert.That(component.Find("input#city").As<IHtmlInputElement>()!.Value, Is.EqualTo(city));
        Assert.That(component.Find("input#city2").As<IHtmlInputElement>()!.Value, Is.EqualTo(city2 ?? city));
    }
}