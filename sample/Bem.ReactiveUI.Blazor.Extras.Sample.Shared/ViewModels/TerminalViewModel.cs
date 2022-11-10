// --------------------------------------
// <copyright file="TerminalViewModel.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;

public sealed class TerminalViewModel : ReactiveObject, IDisposable
{
    private readonly CompositeDisposable _compositeDisposable;
    private IDisposable? _expected24HOutgoingPassengerCountSubscription;

    public TerminalViewModel()
    {
        _expected24HOutgoingPassengerCountSubscription = default!;
        _compositeDisposable = new();

        this.WhenAnyValue(x => x.Departures)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(coll =>
            {
                _expected24HOutgoingPassengerCountSubscription?.Dispose();
                _expected24HOutgoingPassengerCountSubscription = coll
                    .ToObservableChangeSet()
                    .AutoRefresh(x => x.PassengerCount)
                    .Select(_ => Departures.Sum(x => x.PassengerCount))
                    .BindTo(this, x => x.Expected24hOutgoingPassengers);
            }).DisposeWith(_compositeDisposable);
    }

    [ActivatorUtilitiesConstructor]
    public TerminalViewModel(IUpdateReceiver updateReceiver)
        : this()
    {
        updateReceiver.PassengerCountChanged.Subscribe(passengerCountChanged =>
        {
            var departure =
                Departures.FirstOrDefault(departure => departure.Id == passengerCountChanged.DepartureId);
            if (departure != null)
            {
                departure.PassengerCount = passengerCountChanged.PassengerCount;
            }
        })
        .DisposeWith(_compositeDisposable);
    }

    [Reactive]
    public int Expected24hOutgoingPassengers { get; set; }

    [Reactive]
    public string Name { get; set; } = default!;

    [Reactive]
    public ObservableCollection<ArrivalViewModel> Arrivals { get; init; } = new();

    [Reactive]
    public ObservableCollection<DepartureViewModel> Departures { get; init; } = new();

    public void Dispose()
    {
        _compositeDisposable.Dispose();
        _expected24HOutgoingPassengerCountSubscription?.Dispose();
    }
}