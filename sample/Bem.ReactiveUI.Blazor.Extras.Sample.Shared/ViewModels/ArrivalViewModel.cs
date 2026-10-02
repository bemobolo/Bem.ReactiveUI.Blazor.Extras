// --------------------------------------
// <copyright file="ArrivalViewModel.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System;
using ReactiveUI.SourceGenerators;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;

public partial class ArrivalViewModel : JourneyViewModel
{
    [Reactive]
    public partial DateTime Arrival { get; set; }
}