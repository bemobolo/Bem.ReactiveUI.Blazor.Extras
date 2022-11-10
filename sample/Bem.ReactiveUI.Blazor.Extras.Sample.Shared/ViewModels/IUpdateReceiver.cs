// --------------------------------------
// <copyright file="IUpdateReceiver.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;

public interface IUpdateReceiver
{
    IObservable<PassengerCountChanged> PassengerCountChanged { get; }

    IObservable<string> AirportNameChanged { get; }

    IObservable<AirportViewModel> AirportReset { get; }
}