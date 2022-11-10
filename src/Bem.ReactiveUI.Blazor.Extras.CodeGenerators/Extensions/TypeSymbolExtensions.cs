// --------------------------------------
// <copyright file="TypeSymbolExtensions.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.CodeGenerators.Extensions;

internal static class TypeSymbolExtensions
{
    public static IEnumerable<IMethodSymbol> GetMethods(this ITypeSymbol @this, string methodName, bool inherited = true)
    {
        if (@this.BaseType != null && inherited)
        {
            foreach (var member in @this.BaseType.GetMethods(methodName))
            {
                yield return member;
            }
        }

        foreach (var member in @this.GetMembers())
        {
            if (member is IMethodSymbol method && member.Name == methodName)
            {
                yield return method;
            }
        }
    }
}
