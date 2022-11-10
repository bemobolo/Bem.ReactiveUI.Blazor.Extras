// --------------------------------------
// <copyright file="TypeExtensions.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.Extensions;

internal static class TypeExtensions
{
    internal static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        if (givenType is null)
        {
            throw new ArgumentNullException(nameof(givenType));
        }

        var interfaceTypes = givenType.GetInterfaces();

        for (var i = 0; i < interfaceTypes.Length; i++)
        {
            var it = interfaceTypes[i];
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        var baseType = givenType.BaseType;
        if (baseType == null)
        {
            return false;
        }

        return baseType.IsAssignableToGenericType(genericType);
    }
}