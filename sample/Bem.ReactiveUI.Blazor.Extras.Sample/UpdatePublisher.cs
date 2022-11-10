// --------------------------------------
// <copyright file="UpdatePublisher.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Microsoft.AspNetCore.SignalR;

namespace Bem.ReactiveUI.Blazor.Extras.Sample;

internal class UpdatePublisher : IUpdatePublisher
{
    private readonly IHubContext<UpdateHub, IUpdatePublisher> _hubContext;

    public UpdatePublisher(
        IHubContext<UpdateHub, IUpdatePublisher> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task PublishPassengerCountChangedAsync(PassengerCountChanged passengerCountChanged)
    {
        return _hubContext.Clients.All.PublishPassengerCountChangedAsync(passengerCountChanged);
    }

    public Task PublishAirportNameChangedAsync(string airportName)
    {
        return _hubContext.Clients.All.PublishAirportNameChangedAsync(airportName);
    }

    public Task PublishAirportResetAsync(AirportViewModel airportViewModel)
    {
        return _hubContext.Clients.All.PublishAirportResetAsync(airportViewModel);
    }
}