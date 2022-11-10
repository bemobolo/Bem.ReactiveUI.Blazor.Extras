// --------------------------------------
// <copyright file="Reactive.razor.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.Client.Pages;

public partial class Reactive : ReactiveInjectableComponentBase<IndexViewModel>
{
    [Inject]
    private IViewModelProvider ViewModelProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        ViewModel?.Dispose();
        ViewModel = await ViewModelProvider.GetIndexViewModelAsync();
        StateHasChanged();
        await base.OnInitializedAsync();
    }
}