// --------------------------------------
// <copyright file="UpdateReceiver.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Reactive.Linq;
using Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.Client;

internal class UpdateReceiver : SignalRClientBase, IUpdateReceiver
{
    public UpdateReceiver(IHubUrlHelper hubUrlHelper)
        : base(hubUrlHelper, "/update")
    {
        PassengerCountChanged = Observable.Create<PassengerCountChanged>(observer =>
            HubConnection.On<PassengerCountChanged>(nameof(IUpdatePublisher.PublishPassengerCountChangedAsync), args => observer.OnNext(args))).Publish().AutoConnect();
        AirportNameChanged = Observable.Create<string>(observer =>
            HubConnection.On<string>(nameof(IUpdatePublisher.PublishAirportNameChangedAsync), args => observer.OnNext(args))).Publish().AutoConnect();
        AirportReset = Observable.Create<AirportViewModel>(observer => HubConnection.On<AirportViewModel>(nameof(IUpdatePublisher.PublishAirportResetAsync), args => observer.OnNext(args))).Publish().AutoConnect();
    }

    public IObservable<PassengerCountChanged> PassengerCountChanged { get; set; }

    public IObservable<string> AirportNameChanged { get; set; }

    public IObservable<AirportViewModel> AirportReset { get; set; }
}