// --------------------------------------
// <copyright file="SymbolExtensions.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.CodeGenerators.Extensions;

internal static class SymbolExtensions
{
    internal static string GetFullMetadataName(this ISymbol symbol) => $"{symbol.ContainingNamespace}.{symbol.MetadataName}";

    internal static bool InheritsFrom(this ITypeSymbol @this, ISymbol type)
    {
        var result = false;
        var baseType = @this.BaseType;

        while (baseType != null)
        {
            if (!type.Equals(baseType, SymbolEqualityComparer.Default))
            {
                baseType = baseType.BaseType;
            }
            else
            {
                result = true;
                baseType = null;
            }
        }

        return result;
    }
}