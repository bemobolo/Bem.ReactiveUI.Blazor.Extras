// --------------------------------------
// <copyright file="ViewModelController.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.Controllers;

[ApiController]
public sealed class ViewModelController : ControllerBase
{
    private readonly ViewModelService _viewModelService;

    public ViewModelController(ViewModelService viewModelService)
    {
        _viewModelService = viewModelService;
    }

    [HttpGet("/index")]
    public IndexViewModel GetIndexViewModel()
    {
        return _viewModelService.IndexViewModel;
    }

    [HttpPut("/departures/{departureId:int}/{passengerCount:int}")]
    public IActionResult UpdatePassengerCount([FromRoute] [Required] int departureId, [FromRoute] [Required] int passengerCount)
    {
        var result = _viewModelService.UpdatePassengerCount(departureId, passengerCount);

        return !result
            ? Conflict()
            : Ok();
    }

    [HttpPut("/airport/{airportName}")]
    public IActionResult UpdateAirportName([FromRoute] [Required] string airportName)
    {
        _viewModelService.UpdateAirportName(airportName);

        return Ok();
    }

    [HttpPut("/airport/reset")]
    public IActionResult ResetAirportViewModel()
    {
        _viewModelService.ResetAirport();

        return Ok();
    }
}