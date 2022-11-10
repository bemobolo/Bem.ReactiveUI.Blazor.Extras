// --------------------------------------
// <copyright file="ViewModelProvider.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;

internal sealed class ViewModelProvider : IViewModelProvider
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;

    public ViewModelProvider(IApiClient apiClient, IMapper mapper, IUpdateReceiver updateReceiver)
    {
        _apiClient = apiClient;
        _mapper = mapper;
        AirportViewModels = updateReceiver.AirportReset.Select(mapper.Map<AirportViewModel>);
    }

    public IObservable<AirportViewModel> AirportViewModels { get; set; }

    public async Task<IndexViewModel> GetIndexViewModelAsync()
    {
        var vm = await _apiClient.GetIndexViewModelAsync();

        return _mapper.Map<IndexViewModel>(vm);
    }
}