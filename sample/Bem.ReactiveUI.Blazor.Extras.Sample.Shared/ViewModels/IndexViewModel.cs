// --------------------------------------
// <copyright file="IndexViewModel.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using PrimitivesLinq = ReactiveUI.Primitives.LinqExtensions;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels
{
    public sealed partial class IndexViewModel : ReactiveObject, IDisposable
    {
        private readonly IDisposable? _subscription;

        public IndexViewModel()
        {
        }

        [ActivatorUtilitiesConstructor]
        public IndexViewModel(IViewModelProvider viewModelProvider)
            : this()
        {
            _subscription = PrimitivesLinq.ObserveOn(viewModelProvider.AirportViewModels, RxSchedulers.MainThreadScheduler).Subscribe(airportViewModel =>
            {
                using var oldAirport = Airport;
                Airport = airportViewModel;
            });
        }

        [Reactive]
        public partial AirportViewModel Airport { get; set; } = new();

        public void Dispose()
        {
            _subscription?.Dispose();
            Airport.Dispose();
        }
    }
}
