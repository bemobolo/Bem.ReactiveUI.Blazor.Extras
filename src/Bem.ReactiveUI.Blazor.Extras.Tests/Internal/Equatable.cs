// --------------------------------------
// <copyright file="Equatable.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.Tests.Internal;

internal class Equatable : IEquatable<Equatable>
{
    public Equatable(string value)
    {
        Property = value;
    }

    public string? Property { get; init; }

    public bool Equals(Equatable? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Property == other.Property;
    }

    public override bool Equals(object? obj) => Equals(obj as Equatable);

    public override int GetHashCode() => Property != null ? Property.GetHashCode() : 0;
}