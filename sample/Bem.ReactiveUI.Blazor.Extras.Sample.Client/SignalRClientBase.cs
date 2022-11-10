// --------------------------------------
// <copyright file="SignalRClientBase.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.Client;

public abstract class SignalRClientBase : ISignalRClient
{
    protected SignalRClientBase(IHubUrlHelper hubUrlHelper, string hubPath)
    {
        HubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrlHelper.ToAbsoluteUri(hubPath))
            .WithAutomaticReconnect()
            .Build();
        _ = HubConnection.StartAsync();
    }

    public HubConnection HubConnection { get; }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    protected virtual ValueTask DisposeAsync(bool disposing)
    {
        if (disposing)
        {
            return HubConnection.DisposeAsync();
        }

        return ValueTask.CompletedTask;
    }
}