// Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com
// --------------------------------------
// <copyright file="ExpressionEnumeration.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Collections;

namespace Bem.ReactiveUI.Blazor.Extras.Utils;

internal class ExpressionEnumeration : ExpressionVisitor, IEnumerable<Expression>
{
    private readonly List<Expression> _expressions = [];

    internal ExpressionEnumeration(Expression? expression)
    {
        _ = Visit(expression);
    }

    public IEnumerator<Expression> GetEnumerator()
    {
        return _expressions.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override Expression? Visit(Expression? node)
    {
        if (node == null)
        {
            return null;
        }

        _expressions.Add(node);

        return base.Visit(node);
    }
}