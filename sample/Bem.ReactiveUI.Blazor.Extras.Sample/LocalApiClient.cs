// --------------------------------------
// <copyright file="LocalApiClient.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.Sample;

internal sealed class LocalApiClient : IApiClient
{
    private readonly ViewModelService _viewModelService;

    public LocalApiClient(ViewModelService viewModelService)
    {
        _viewModelService = viewModelService;
    }

    public Task UpdatePassengerCountAsync(int departureId, int passengerCount)
    {
        _viewModelService.UpdatePassengerCount(departureId, passengerCount);
        return Task.CompletedTask;
    }

    public Task UpdateAirportNameAsync(string airportName)
    {
        _viewModelService.UpdateAirportName(airportName);
        return Task.CompletedTask;
    }

    public Task<IndexViewModel> GetIndexViewModelAsync()
    {
        return Task.FromResult(_viewModelService.IndexViewModel);
    }

    public Task ResetAirportViewModelAsync()
    {
        _viewModelService.ResetAirport();
        return Task.CompletedTask;
    }
}