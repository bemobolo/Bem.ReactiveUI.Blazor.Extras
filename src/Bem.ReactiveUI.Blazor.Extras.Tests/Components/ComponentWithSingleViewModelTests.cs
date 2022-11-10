// --------------------------------------
// <copyright file="ComponentWithSingleViewModelTests.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using AngleSharp.Html.Dom;
using Bem.ReactiveUI.Blazor.Extras.Tests.ViewModels;
using Bunit;
using DynamicData.Binding;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Bem.ReactiveUI.Blazor.Extras.Tests.Components;

public class ComponentWithSingleViewModelTests : BunitTestContext
{
    private TestViewModel _viewModel = default!;

    public override void Setup()
    {
        base.Setup();

        _viewModel = new() { ObservableCollection = new ObservableCollectionExtended<TestObject>(new[] { new TestObject(3) }), TestObject = new(value: 2), Value = 1 };
    }

    [Test]
    public void Renders_Component()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithSingleViewModel>(ComponentParameter.CreateParameter("ViewModel", _viewModel));

        AssertComponent(component, vmValue: 1, vmObjectValue: 2, vmCollectionItems: [3], renderCount: 1);
    }

    [Test]
    public void Renders_Component_Even_If_Bound_Property_Chain_Has_NullReferences()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithSingleViewModel>(ComponentParameter.CreateParameter("ViewModel", new TestViewModel()));

        Assert.That(component.Find("div > p#vmValue").InnerHtml, Is.EqualTo("0"));
        Assert.That(component.Find("div > p#vmObjectValue").InnerHtml, Is.EqualTo("0"));
        Assert.That(component.Find("div > input").As<IHtmlInputElement>()!.Value, Is.EqualTo("0"));
        CollectionAssert.AreEqual(Enumerable.Empty<string>(), component.FindAll("div > ul#vmColl > li").Select(e => e.InnerHtml));
        Assert.That(component.Find("div > p#rc").InnerHtml, Is.EqualTo("0"));
    }

    [Test]
    public void Rerenders_When_Bound_Object_Changes()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithSingleViewModel>(ComponentParameter.CreateParameter("ViewModel", _viewModel));
        _viewModel.TestObject = new TestObject(5);

        AssertComponent(component, vmValue: 1, vmObjectValue: 5, vmCollectionItems: [3], renderCount: 2);
    }

    [Test]
    public void Rerenders_When_Bound_Property_Changes()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithSingleViewModel>(ComponentParameter.CreateParameter("ViewModel", _viewModel));
        _viewModel.Value = 4;

        AssertComponent(component, vmValue: 4, vmObjectValue: 2, vmCollectionItems: [3], renderCount: 2);
    }

    [Test]
    public void Rerenders_When_Bound_Property_Of_Reactive_Object_Changes()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithSingleViewModel>(ComponentParameter.CreateParameter("ViewModel", _viewModel));
        _viewModel.TestObject!.Value = 4;

        AssertComponent(component, vmValue: 1, vmObjectValue: 4, vmCollectionItems: [3], renderCount: 2);
    }

    [Test]
    public void Rerenders_When_Item_In_ObservableCollection_Changes()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithSingleViewModel>(ComponentParameter.CreateParameter("ViewModel", _viewModel));
        _viewModel.ObservableCollection![0].Value = 5;

        AssertComponent(component, vmValue: 1, vmObjectValue: 2, vmCollectionItems: [5], renderCount: 2);
    }

    [Test]
    public void Rerenders_When_ObservableCollection_Changes()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithSingleViewModel>(ComponentParameter.CreateParameter("ViewModel", _viewModel));
        _viewModel.ObservableCollection!.Add(new TestObject(6));

        AssertComponent(component, vmValue: 1, vmObjectValue: 2, vmCollectionItems: [3, 6], renderCount: 2);
    }

    [Test]
    public void Rerenders_When_ForceRenderOnEvent_Enabled_And_Event_Handled()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithSingleViewModel>(ComponentParameter.CreateParameter("ViewModel", _viewModel));
        component.Instance.ForceRenderOnEvent = true;

        component.Find("div > button").Click();

        AssertComponent(component, vmValue: 1, vmObjectValue: 2, vmCollectionItems: [3], renderCount: 2);
    }

    [Test]
    public void Does_Not_Rerender_When_AutoRenderOnEvent_Disabled_And_Event_Handled()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithSingleViewModel>(ComponentParameter.CreateParameter("ViewModel", _viewModel));
        component.Instance.ForceRenderOnEvent = false;

        component.Find("div > button").Click();

        AssertComponent(component, vmValue: 1, vmObjectValue: 2, vmCollectionItems: [3], renderCount: 1);
    }

    private static void AssertComponent(IRenderedComponent<ComponentWithSingleViewModel> component, int vmValue, int vmObjectValue, int[] vmCollectionItems, int renderCount)
    {
        Assert.That(component.Instance.ViewModel!.Value, Is.EqualTo(vmValue));
        Assert.That(component.Instance.ViewModel.TestObject!.Value, Is.EqualTo(vmObjectValue));
        Assert.That(component.Instance.InputNumber!.Value, Is.EqualTo(vmValue));
        CollectionAssert.AreEqual(vmCollectionItems, component.Instance.ViewModel.ObservableCollection!.Select(i => i.Value));
        Assert.That(component.Instance.RenderCount, Is.EqualTo(renderCount));

        Assert.That(component.Find("div > p#vmValue").InnerHtml, Is.EqualTo(vmValue.ToString()));
        Assert.That(component.Find("div > p#vmObjectValue").InnerHtml, Is.EqualTo(vmObjectValue.ToString()));
        Assert.That(component.Find("div > input").As<IHtmlInputElement>()!.Value, Is.EqualTo(vmValue.ToString()));
        CollectionAssert.AreEqual(vmCollectionItems.Select(v => v.ToString()), component.FindAll("div > ul#vmColl > li").Select(e => e.InnerHtml));
        Assert.That(component.Find("div > p#rc").InnerHtml, Is.EqualTo((renderCount - 1).ToString()));
    }
}