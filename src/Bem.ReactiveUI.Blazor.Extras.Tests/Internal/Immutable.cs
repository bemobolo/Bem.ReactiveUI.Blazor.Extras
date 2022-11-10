// --------------------------------------
// <copyright file="Immutable.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.Tests.Internal;

[ImmutableObject(true)]
internal class Immutable
{
    public Immutable(string value)
    {
        Property = value;
    }

    public string? Property { get; init; }
}