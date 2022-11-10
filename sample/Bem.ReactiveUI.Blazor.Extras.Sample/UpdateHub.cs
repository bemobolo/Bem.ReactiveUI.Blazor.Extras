// --------------------------------------
// <copyright file="UpdateHub.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Microsoft.AspNetCore.SignalR;

namespace Bem.ReactiveUI.Blazor.Extras.Sample;

public sealed class UpdateHub : Hub<IUpdatePublisher>
{
#pragma warning disable S1185 // Overriding members should do more than simply call the same member in the base class
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
#pragma warning restore S1185 // Overriding members should do more than simply call the same member in the base class
}
