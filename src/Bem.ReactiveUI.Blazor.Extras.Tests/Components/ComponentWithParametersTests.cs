// --------------------------------------
// <copyright file="ComponentWithParametersTests.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Bunit;
using NUnit.Framework;

namespace Bem.ReactiveUI.Blazor.Extras.Tests.Components;

public class ComponentWithParametersTests : BunitTestContext
{
    private object _object = default!;

    public override void Setup()
    {
        base.Setup();

        _object = new object();
    }

    [Test]
    public void Rerenders_Component_When_Event_Occurs_And_Mutable_Parameter_Available()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithParameters>(
            ComponentParameter.CreateParameter(nameof(ComponentWithParameters.IntegerParameter), 1),
            ComponentParameter.CreateParameter(nameof(ComponentWithParameters.ReferenceParameter), _object));

        component.Instance.ForceRenderOnEvent = false;
        component.Find("div > button").Click();

        AssertComponent(component, integerParameter: 2, referenceParameter: _object, renderCount: 2);
    }

    [Test]
    public void Rerenders_Component_When_Mutable_Parameter_Not_Available_Any_More()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithParameters>(
            ComponentParameter.CreateParameter(nameof(ComponentWithParameters.IntegerParameter), 1),
            ComponentParameter.CreateParameter(nameof(ComponentWithParameters.ReferenceParameter), _object));

        component.SetParametersAndRender(
            ComponentParameter.CreateParameter(nameof(ComponentWithParameters.IntegerParameter), 1),
            ComponentParameter.CreateParameter(nameof(ComponentWithParameters.ReferenceParameter), null));

        AssertComponent(component, integerParameter: 1, referenceParameter: null, renderCount: 2);
    }

    [Test]
    public void Rerenders_Component_When_Mutable_Parameter_Not_Set()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithParameters>(
            ComponentParameter.CreateParameter(nameof(ComponentWithParameters.IntegerParameter), 1),
            ComponentParameter.CreateParameter(nameof(ComponentWithParameters.ReferenceParameter), _object));

        component.SetParametersAndRender(
            ComponentParameter.CreateParameter(nameof(ComponentWithParameters.IntegerParameter), 1));

        AssertComponent(component, integerParameter: 1, referenceParameter: _object, renderCount: 2);
    }

    [Test]
    public void Does_Not_Rerender_Component_When_Mutable_Parameter_Was_Null_And_Not_Set()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithParameters>(
            ComponentParameter.CreateParameter(nameof(ComponentWithParameters.IntegerParameter), 1),
            ComponentParameter.CreateParameter(nameof(ComponentWithParameters.ReferenceParameter), null));

        component.SetParametersAndRender(
            ComponentParameter.CreateParameter(nameof(ComponentWithParameters.IntegerParameter), 1));

        AssertComponent(component, integerParameter: 1, referenceParameter: null, renderCount: 1);
    }

    private static void AssertComponent(IRenderedComponent<ComponentWithParameters> component, int integerParameter, object? referenceParameter, int renderCount)
    {
        Assert.That(component.Instance.IntegerParameter, Is.EqualTo(integerParameter));
        Assert.That(component.Instance.ReferenceParameter, Is.EqualTo(referenceParameter));
        Assert.That(component.Instance.RenderCount, Is.EqualTo(renderCount));

        Assert.That(component.Find("div > p#integerParam").InnerHtml, Is.EqualTo(integerParameter.ToString()));
        Assert.That(component.Find("div > p#referenceParam").InnerHtml, Is.EqualTo(referenceParameter?.GetHashCode().ToString() ?? string.Empty));
        Assert.That(component.Find("div > p#rc").InnerHtml, Is.EqualTo((renderCount - 1).ToString()));
    }
}