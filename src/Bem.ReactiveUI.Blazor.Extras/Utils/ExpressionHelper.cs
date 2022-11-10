// --------------------------------------
// <copyright file="ExpressionHelper.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.Utils;

internal static class ExpressionHelper
{
    internal static Expression<Func<T, TValue?>> GetFieldValueExpression<T, TValue>(FieldInfo field)
    {
        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.Field(parameter, field);
        return Expression.Lambda<Func<T, TValue?>>(body, parameter);
    }
}