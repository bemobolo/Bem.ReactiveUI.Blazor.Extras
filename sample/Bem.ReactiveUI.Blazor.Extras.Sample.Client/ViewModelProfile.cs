// --------------------------------------
// <copyright file="ViewModelProfile.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.Client;

public sealed class ViewModelProfile : Profile
{
    public ViewModelProfile(IServiceProvider serviceServiceProvider)
    {
        CreateMap<IndexViewModel, IndexViewModel>()
            .ConstructUsing((_, _) => serviceServiceProvider.GetRequiredService<IndexViewModel>());
        CreateMap<AirportViewModel, AirportViewModel>()
            .ConstructUsing((_, _) => serviceServiceProvider.GetRequiredService<AirportViewModel>())
            .EqualityComparison((a, b) => a.Name == b.Name);
        CreateMap<TerminalViewModel, TerminalViewModel>()
            .ConstructUsing((_, _) => serviceServiceProvider.GetRequiredService<TerminalViewModel>())
            .ForMember(x => x.Departures, o => o.UseDestinationValue())
            .EqualityComparison((a, b) => a.Name == b.Name);
        CreateMap<DepartureViewModel, DepartureViewModel>()
            .ConstructUsing((_, _) => serviceServiceProvider.GetRequiredService<DepartureViewModel>())
            .EqualityComparison((a, b) => a.Id == b.Id);
        CreateMap<ArrivalViewModel, ArrivalViewModel>()
            .EqualityComparison((a, b) => a.Id == b.Id);
    }
}