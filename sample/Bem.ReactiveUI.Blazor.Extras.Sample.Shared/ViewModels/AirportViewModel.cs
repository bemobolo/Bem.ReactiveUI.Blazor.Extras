// --------------------------------------
// <copyright file="AirportViewModel.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;

public sealed class AirportViewModel : ReactiveObject, IDisposable
{
    private readonly IApiClient _apiClient;
    private readonly CompositeDisposable _compositeDisposable;

    public AirportViewModel()
    {
        _compositeDisposable = new();
        _apiClient = default!;
    }

    [ActivatorUtilitiesConstructor]
    public AirportViewModel(IApiClient apiClient, IUpdateReceiver updateReceiver)
        : this()
    {
        _apiClient = apiClient;

        updateReceiver.AirportNameChanged.BindTo(this, x => x.Name).DisposeWith(_compositeDisposable);
    }

    [Reactive]
    public string Name { get; set; } = default!;

    [Reactive]
    public ObservableCollection<TerminalViewModel> Terminals { get; init; } = new();

    public Task UpdateAirportNameAsync(string airportName)
    {
        return _apiClient.UpdateAirportNameAsync(airportName);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();

        foreach (var terminalViewModel in Terminals)
        {
            terminalViewModel.Dispose();
        }

        Terminals.Clear();
    }

    public Task ResetAsync()
    {
        return _apiClient.ResetAirportViewModelAsync();
    }
}