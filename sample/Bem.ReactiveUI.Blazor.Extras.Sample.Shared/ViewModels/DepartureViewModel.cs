// --------------------------------------
// <copyright file="DepartureViewModel.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI.Fody.Helpers;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;

public sealed class DepartureViewModel : JourneyViewModel
{
    private readonly IApiClient _apiClient;

    public DepartureViewModel()
    {
        _apiClient = default!;
    }

    [ActivatorUtilitiesConstructor]
    public DepartureViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [Reactive]
    public bool Boarding { get; set; }

    [Reactive]
    public DateTime Departure { get; set; }

    public Task UpdatePassengerCountAsync(int passengerCount)
    {
        PassengerCount = passengerCount;
        return _apiClient.UpdatePassengerCountAsync(Id, passengerCount);
    }
}