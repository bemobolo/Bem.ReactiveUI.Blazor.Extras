// --------------------------------------
// <copyright file="ViewModelFactory.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bem.ReactiveUI.Blazor.Extras.Sample.Extensions;
using Bogus;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;

public class ViewModelFactory
{
    public IndexViewModel CreateIndexViewModel()
    {
        var indexViewModelFaker = new Faker<IndexViewModel>()
            .UseSeed(1)
            .RuleFor(x => x.Airport, _ => CreateAirportViewModel());

        return indexViewModelFaker.Generate();
    }

    public AirportViewModel CreateAirportViewModel()
    {
        var terminalNames = new Queue<string>(new[] { "Terminal A", "Terminal B", "Terminal C" });
        var airlines = new[] { "Blaze Air", "Blazing Airways", "React Air" };
        var delays = new[] { 0, 5, 10, 15, 20, 25, 30 };
        var delayChances = new[] { 0.8f, 0.05f, 0.05f, 0.02f, 0.03f, 0.01f, 0.04f };
        var planeTypes = new[] { "Airbus A319", "Airbus A320", "Boeing 737" };

        var flight = new Faker<FlightViewModel>()
            .UseSeed(1)
            .RuleFor(x => x.Airline, f => f.PickRandom(airlines))
            .RuleFor(x => x.Code, f => f.Random.AlphaNumeric(6))
            .RuleFor(x => x.PlaneType, f => f.PickRandom(planeTypes));

        var arrival = new Faker<ArrivalViewModel>()
            .UseSeed(1)
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Arrival, f => DateTime.Now.AddHours(f.Random.Int(0, 23)).RoundUpToHour())
            .RuleFor(x => x.PassengerCount, f => f.Random.Int(100, 200))
            .RuleFor(x => x.Delay, f => TimeSpan.FromMinutes(f.Random.WeightedRandom(delays, delayChances)))
            .RuleFor(x => x.Flight, _ => flight.Generate());

        var departure = new Faker<DepartureViewModel>()
            .UseSeed(1)
            .RuleFor(x => x.Id, f => f.IndexFaker)
            .RuleFor(x => x.Departure, f => DateTime.Now.AddHours(f.Random.Int(0, 23)).RoundUpToHour())
            .RuleFor(x => x.PassengerCount, f => f.Random.Int(100, 200))
            .RuleFor(x => x.Delay, f => TimeSpan.FromMinutes(f.Random.WeightedRandom(delays, delayChances)))
            .RuleFor(x => x.Flight, _ => flight.Generate())
            .RuleFor(x => x.Boarding, (_, vm) => vm.Departure <= DateTime.Now.AddHours(1));

        var terminal = new Faker<TerminalViewModel>()
            .UseSeed(1)
            .RuleFor(x => x.Name, _ => terminalNames.Dequeue())
            .RuleFor(x => x.Arrivals, _ => new ObservableCollection<ArrivalViewModel>(arrival.Generate(20)))
            .RuleFor(x => x.Departures, _ => new ObservableCollection<DepartureViewModel>(departure.Generate(15)));

        var airport = new Faker<AirportViewModel>()
            .UseSeed(1)
            .RuleFor(x => x.Name, "Three Mountains Airport")
            .RuleFor(x => x.Terminals, _ => new ObservableCollection<TerminalViewModel>(terminal.Generate(2)));

        return airport.Generate();
    }
}