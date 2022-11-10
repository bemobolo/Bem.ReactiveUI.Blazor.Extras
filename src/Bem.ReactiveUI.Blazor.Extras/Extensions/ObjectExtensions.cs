// --------------------------------------
// <copyright file="ObjectExtensions.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.Extensions
{
    internal static class ObjectExtensions
    {
        internal static T? As<T>(this object @this)
            where T : class => @this as T;
    }
}