// --------------------------------------
// <copyright file="ISignalRClient.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Microsoft.AspNetCore.SignalR.Client;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.Client;

public interface ISignalRClient : IAsyncDisposable
{
    HubConnection HubConnection { get; }
}