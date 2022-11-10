// --------------------------------------
// <copyright file="ViewModelService.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Linq;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;

public class ViewModelService
{
    private readonly ViewModelFactory _viewModelFactory;
    private readonly IUpdatePublisher _updatePublisher;
    private IndexViewModel? _indexViewModel;

    public ViewModelService(ViewModelFactory viewModelFactory, IUpdatePublisher updatePublisher)
    {
        _viewModelFactory = viewModelFactory;
        _updatePublisher = updatePublisher;
    }

    public IndexViewModel IndexViewModel => _indexViewModel ??= _viewModelFactory.CreateIndexViewModel();

    public bool UpdatePassengerCount(int departureId, int passengerCount)
    {
        var result = false;

        var departure = IndexViewModel.Airport.Terminals.SelectMany(terminal =>
            terminal.Departures).FirstOrDefault(departure => departure.Id == departureId);

        if (departure != null)
        {
            departure.PassengerCount = passengerCount;
            _ = _updatePublisher.PublishPassengerCountChangedAsync(new PassengerCountChanged
                { DepartureId = departureId, PassengerCount = passengerCount });

            result = true;
        }

        return result;
    }

    public void UpdateAirportName(string airportName)
    {
        IndexViewModel.Airport.Name = airportName;

        _ = _updatePublisher.PublishAirportNameChangedAsync(airportName);
    }

    public void ResetAirport()
    {
        IndexViewModel.Airport.Dispose();
        IndexViewModel.Airport = _viewModelFactory.CreateAirportViewModel();

        _ = _updatePublisher.PublishAirportResetAsync(IndexViewModel.Airport);
    }
}