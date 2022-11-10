// --------------------------------------
// <copyright file="ApiClient.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.Client;

internal sealed class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    internal ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        _serializerOptions.PropertyNamingPolicy = null;
        _serializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public async Task UpdatePassengerCountAsync(int departureId, int passengerCount)
    {
        var response = await _httpClient.PutAsync($"departures/{departureId}/{passengerCount}", null);

        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAirportNameAsync(string airportName)
    {
        var response = await _httpClient.PutAsync($"airport/{airportName}", null);

        response.EnsureSuccessStatusCode();
    }

    public async Task<IndexViewModel> GetIndexViewModelAsync()
    {
        var vm = (await _httpClient.GetFromJsonAsync<IndexViewModel>("index", _serializerOptions))!;

        return vm;
    }

    public async Task ResetAirportViewModelAsync()
    {
        var response = await _httpClient.PutAsync("airport/reset", null);

        response.EnsureSuccessStatusCode();
    }
}
