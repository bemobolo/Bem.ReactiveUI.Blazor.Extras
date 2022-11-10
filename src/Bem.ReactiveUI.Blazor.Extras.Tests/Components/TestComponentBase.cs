// --------------------------------------
// <copyright file="TestComponentBase.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Bem.ReactiveUI.Blazor.Extras.Components;

namespace Bem.ReactiveUI.Blazor.Extras.Tests.Components;

public abstract class TestComponentBase<TViewModel> : AdvancedComponentBase<TViewModel>
    where TViewModel : class, INotifyPropertyChanged
{
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        RenderCount++;
    }

    public int RenderCount { get; protected set; }
}