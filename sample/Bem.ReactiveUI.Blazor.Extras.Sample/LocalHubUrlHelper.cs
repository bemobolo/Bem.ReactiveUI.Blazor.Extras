// --------------------------------------
// <copyright file="LocalHubUrlHelper.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Bem.ReactiveUI.Blazor.Extras.Sample;

internal class LocalHubUrlHelper : IHubUrlHelper
{
    private readonly Uri _baseUri;

    public LocalHubUrlHelper(IServer server)
    {
        var address = server.Features.Get<IServerAddressesFeature>()!.Addresses
            .First();

        _baseUri = new Uri(address, UriKind.Absolute);
    }

    public Uri ToAbsoluteUri(string hubPath)
    {
        return new Uri(_baseUri, hubPath);
    }
}