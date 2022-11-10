// --------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.Extensions;

internal static class DateTimeExtensions
{
    internal static DateTime RoundUpToHour(this DateTime @this) => RoundUp(@this, TimeSpan.FromHours(1));

    internal static DateTime RoundUp(this DateTime dt, TimeSpan @this) => new((dt.Ticks + @this.Ticks - 1) / @this.Ticks * @this.Ticks, dt.Kind);
}