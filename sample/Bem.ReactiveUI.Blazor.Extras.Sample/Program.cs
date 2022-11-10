// --------------------------------------
// <copyright file="Program.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Bem.ReactiveUI.Blazor.Extras.Sample.Client;
using Bem.ReactiveUI.Blazor.Extras.Sample.Client.Pages;
using Bem.ReactiveUI.Blazor.Extras.Sample.Components;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;

namespace Bem.ReactiveUI.Blazor.Extras.Sample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services
                .AddControllersWithViews()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            builder.Services
                .AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services
                .AddBlazorise(options =>
                {
                    options.Immediate = false;
                })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();

            builder.Services.AddSignalR();

            builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.AddProfile(new ViewModelProfile(provider.GetRequiredService<IServiceProvider>()));
            }).CreateMapper());

            builder.Services.AddSingleton<IHubUrlHelper, LocalHubUrlHelper>();
            builder.Services.AddSingleton<IUpdatePublisher, UpdatePublisher>();
            builder.Services.AddSingleton<IUpdateReceiver, UpdateReceiver>();

            builder.Services.AddSingleton<ViewModelService>();
            builder.Services.AddSingleton<ViewModelFactory>();
            builder.Services.AddSingleton<IViewModelProvider, ViewModelProvider>();
            builder.Services.AddSingleton<IApiClient, LocalApiClient>();

            builder.Services.AddTransient<IndexViewModel>();
            builder.Services.AddTransient<AirportViewModel>();
            builder.Services.AddTransient<DepartureViewModel>();
            builder.Services.AddTransient<TerminalViewModel>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAntiforgery();
            app.MapControllers();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddAdditionalAssemblies(typeof(Home).Assembly);

            app.MapHub<UpdateHub>("/update");

            app.Run();
        }
    }
}
