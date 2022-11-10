// Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com
// --------------------------------------
// <copyright file="ExpressionEqualityComparer.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.Utils;

internal class ExpressionEqualityComparer : IEqualityComparer<Expression>
{
    internal static readonly ExpressionEqualityComparer Instance = new();

    public bool Equals(Expression? x, Expression? y)
    {
        return new ExpressionComparison(x, y).AreEqual;
    }

    public int GetHashCode(Expression obj)
    {
        return new HashCodeCalculation(obj).HashCode;
    }
}