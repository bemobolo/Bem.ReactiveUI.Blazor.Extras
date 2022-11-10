// --------------------------------------
// <copyright file="HubUrlHelper.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Bem.ReactiveUI.Blazor.Extras.Sample.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.Client;

internal class HubUrlHelper : IHubUrlHelper
{
    private readonly Uri _baseUri;

    public HubUrlHelper(NavigationManager navigationManager)
    {
        _baseUri = new Uri(navigationManager.BaseUri);
    }

    public Uri ToAbsoluteUri(string hubPath)
    {
        return new Uri(_baseUri, hubPath);
    }
}
