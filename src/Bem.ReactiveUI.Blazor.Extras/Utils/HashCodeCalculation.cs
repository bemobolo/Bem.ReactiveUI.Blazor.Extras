// Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com
// --------------------------------------
// <copyright file="HashCodeCalculation.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Bem.ReactiveUI.Blazor.Extras.Extensions;

namespace Bem.ReactiveUI.Blazor.Extras.Utils;

internal class HashCodeCalculation : ExpressionVisitor
{
    internal HashCodeCalculation(Expression expression)
    {
        _ = Visit(expression);
    }

    internal int HashCode { get; private set; }

    public override Expression? Visit(Expression? node)
    {
        if (node == null)
        {
            return null;
        }

        Add((int)node.NodeType);
        Add(node.Type.GetHashCode());

        return base.Visit(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (node.Value != null)
        {
            Add(node.Value.GetHashCode());
        }

        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        Add(node.Member.GetHashCode());

        return base.VisitMember(node);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        Add(node.Method.GetHashCode());

        return base.VisitMethodCall(node);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        Add(node.Name?.GetHashCode() ?? 1);

        return node;
    }

    protected override Expression VisitTypeBinary(TypeBinaryExpression node)
    {
        Add(node.TypeOperand.GetHashCode());

        return base.VisitTypeBinary(node);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (node.Method != null)
        {
            Add(node.Method.GetHashCode());
        }

        if (node.IsLifted)
        {
            Add(1);
        }

        if (node.IsLiftedToNull)
        {
            Add(1);
        }

        return base.VisitBinary(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        if (node.Method != null)
        {
            Add(node.Method.GetHashCode());
        }

        if (node.IsLifted)
        {
            Add(1);
        }

        if (node.IsLiftedToNull)
        {
            Add(1);
        }

        return base.VisitUnary(node);
    }

    protected override Expression VisitNew(NewExpression node)
    {
        Add(node.Constructor!.GetHashCode());

        var collection = node.Members.EmptyIfNull();
        Add(collection.Count);

        foreach (var memberInfo in collection)
        {
            Add(memberInfo.GetHashCode());
        }

        return base.VisitNew(node);
    }

    private void Add(int i)
    {
        HashCode *= 37;
        HashCode ^= i;
    }
}