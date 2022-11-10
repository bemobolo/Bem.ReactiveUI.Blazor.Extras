// --------------------------------------
// <copyright file="IApiClient.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Threading.Tasks;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;

public interface IApiClient
{
    Task UpdatePassengerCountAsync(int departureId, int passengerCount);

    Task UpdateAirportNameAsync(string airportName);

    Task<IndexViewModel> GetIndexViewModelAsync();

    Task ResetAirportViewModelAsync();
}