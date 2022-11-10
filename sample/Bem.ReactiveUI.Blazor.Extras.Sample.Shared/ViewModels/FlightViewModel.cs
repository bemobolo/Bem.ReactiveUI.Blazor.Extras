// --------------------------------------
// <copyright file="FlightViewModel.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;

public class FlightViewModel : ReactiveObject
{
    [Reactive]
    public string Airline { get; set; } = default!;

    [Reactive]
    public string Code { get; set; } = default!;

    [Reactive]
    public string PlaneType { get; set; } = default!;
}