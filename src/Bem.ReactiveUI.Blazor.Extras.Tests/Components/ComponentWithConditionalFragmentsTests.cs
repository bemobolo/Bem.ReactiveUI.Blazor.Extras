// --------------------------------------
// <copyright file="ComponentWithConditionalFragmentsTests.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Bem.ReactiveUI.Blazor.Extras.Tests.ViewModels;
using Bunit;
using NUnit.Framework;

namespace Bem.ReactiveUI.Blazor.Extras.Tests.Components;

public class ComponentWithConditionalFragmentsTests : BunitTestContext
{
    private StringsViewModel _viewModel = default!;

    public override void Setup()
    {
        base.Setup();

        _viewModel = new();
    }

    [Test]
    public void Render_Discards_Binding_NotUsed()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithConditionalFragments>(ComponentParameter.CreateParameter("ViewModel", _viewModel));
        component.Instance.CityVisible = false;
        component.Instance.StateHasChanged();

        Assert.That(component.Instance.InlineBindings.Count, Is.EqualTo(1));
    }

    [Test]
    public void Render_Creates_Bindings()
    {
        // Arrange
        using var component = RenderComponent<ComponentWithConditionalFragments>(ComponentParameter.CreateParameter("ViewModel", _viewModel));

        Assert.That(component.Instance.InlineBindings.Count, Is.EqualTo(2));
    }
}