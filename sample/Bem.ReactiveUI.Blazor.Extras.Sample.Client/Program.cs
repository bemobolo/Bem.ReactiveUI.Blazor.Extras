// --------------------------------------
// <copyright file="Program.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ReactiveUI.Builder;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.Client;

public static class Program
{
    public static Task Main(string[] args)
    {
        _ = RxAppBuilder.CreateReactiveUIBuilder()
            .WithBlazorWasm()
            .BuildApp();

        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddSingleton(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddSingleton(provider => (IApiClient)new ApiClient(provider.GetRequiredService<HttpClient>()));

        AddServices(builder.Services);

        return builder.Build().RunAsync();
    }

    public static void AddServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IViewModelProvider, ViewModelProvider>();
        serviceCollection.AddSingleton<IUpdateReceiver, UpdateReceiver>();
        serviceCollection.AddSingleton<IHubUrlHelper, HubUrlHelper>();

        serviceCollection.AddTransient<IndexViewModel>();
        serviceCollection.AddTransient<AirportViewModel>();
        serviceCollection.AddTransient<DepartureViewModel>();
        serviceCollection.AddTransient<TerminalViewModel>();

        serviceCollection.AddSingleton(provider => new MapperConfiguration(
            cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.AddProfile(new ViewModelProfile(provider.GetRequiredService<IServiceProvider>()));
            },
            provider.GetRequiredService<ILoggerFactory>()).CreateMapper());
    }
}