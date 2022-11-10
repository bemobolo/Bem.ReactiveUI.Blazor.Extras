// --------------------------------------
// <copyright file="JourneyViewModel.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;

public abstract class JourneyViewModel : ReactiveObject
{
    protected JourneyViewModel()
    {
        Flight = new();
    }

    public int Id { get; set; }

    [Reactive]
    public FlightViewModel Flight { get; set; }

    [Reactive]
    public int PassengerCount { get; set; }

    [Reactive]
    public TimeSpan Delay { get; set; }
}