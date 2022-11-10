// Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com
// --------------------------------------
// <copyright file="ExpressionComparison.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Collections.ObjectModel;

namespace Bem.ReactiveUI.Blazor.Extras.Utils;

internal class ExpressionComparison : ExpressionVisitor
{
    private readonly Queue<Expression> _candidates = default!;
    private Expression? _candidate;

    internal ExpressionComparison(Expression? a, Expression? b)
    {
        if (ReferenceEquals(a, b))
        {
            return;
        }

        if (a is null || b is null)
        {
            Stop();
            return;
        }

        _candidates = new Queue<Expression>(new ExpressionEnumeration(b));

        _ = Visit(a);

        if (_candidates.Count > 0)
        {
            Stop();
        }
    }

    internal bool AreEqual { get; private set; } = true;

    public override Expression? Visit(Expression? node)
    {
        if (node == null || !AreEqual)
        {
            return node;
        }

        _candidate = PeekCandidate();

        if (!CheckNotNull(_candidate) || !CheckAreOfSameType(_candidate!, node))
        {
            return node;
        }

        _ = PopCandidate();

        return base.Visit(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        var candidate = CandidateFor(node);

        _ = CheckEqual(node.Value, candidate.Value);

        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        var candidate = CandidateFor(node);

        if (CheckEqual(node.Member, candidate.Member))
        {
            return base.VisitMember(node);
        }

        return node;
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        var candidate = CandidateFor(node);
        if (!CheckEqual(node.Method, candidate.Method))
        {
            return node;
        }

        return base.VisitMethodCall(node);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        var candidate = CandidateFor(node);

        _ = CheckEqual(node.Name, candidate.Name);

        return node;
    }

    protected override Expression VisitTypeBinary(TypeBinaryExpression node)
    {
        var candidate = CandidateFor(node);

        if (!CheckEqual(node.TypeOperand, candidate.TypeOperand))
        {
            return node;
        }

        return base.VisitTypeBinary(node);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        var candidate = CandidateFor(node);

        if (!CheckEqual(node.Method, candidate.Method))
        {
            return node;
        }

        if (!CheckEqual(node.IsLifted, candidate.IsLifted))
        {
            return node;
        }

        if (!CheckEqual(node.IsLiftedToNull, candidate.IsLiftedToNull))
        {
            return node;
        }

        return base.VisitBinary(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        var candidate = CandidateFor(node);
        if (!CheckEqual(node.Method, candidate.Method))
        {
            return node;
        }

        if (!CheckEqual(node.IsLifted, candidate.IsLifted))
        {
            return node;
        }

        if (!CheckEqual(node.IsLiftedToNull, candidate.IsLiftedToNull))
        {
            return node;
        }

        return base.VisitUnary(node);
    }

    protected override Expression VisitNew(NewExpression node)
    {
        var candidate = CandidateFor(node);

        if (!CheckEqual(node.Constructor, candidate.Constructor))
        {
            return node;
        }

        CompareList(node.Members, candidate.Members);

        return base.VisitNew(node);
    }

    private Expression? PeekCandidate()
    {
        if (_candidates.Count == 0)
        {
            return null;
        }

        return _candidates.Peek();
    }

    private Expression PopCandidate()
    {
        return _candidates.Dequeue();
    }

    private bool CheckAreOfSameType(Expression candidate, Expression expression)
    {
        if (!CheckEqual(expression.NodeType, candidate.NodeType))
        {
            return false;
        }

        if (!CheckEqual(expression.Type, candidate.Type))
        {
            return false;
        }

        return true;
    }

    private void Stop()
    {
        AreEqual = false;
    }

    private T CandidateFor<T>(T _)
        where T : Expression
    {
        return (T)_candidate!;
    }

    private void CompareList<T>(ReadOnlyCollection<T>? collection, ReadOnlyCollection<T>? candidates)
    {
        if (ReferenceEquals(collection, candidates))
        {
            return;
        }

        if (collection is null || candidates is null)
        {
            Stop();
            return;
        }

        if (!CheckAreOfSameSize(collection, candidates))
        {
            return;
        }

        for (var i = 0; i < collection.Count; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(collection[i], candidates[i]))
            {
                Stop();
                return;
            }
        }
    }

    private bool CheckAreOfSameSize<T>(ICollection<T> collection, ICollection<T> candidate)
    {
        return CheckEqual(collection.Count, candidate.Count);
    }

    private bool CheckNotNull<T>(T? t)
        where T : class
    {
        if (t == null)
        {
            Stop();
            return false;
        }

        return true;
    }

    private bool CheckEqual<T>(T t, T candidate)
    {
        if (!EqualityComparer<T>.Default.Equals(t, candidate))
        {
            Stop();
            return false;
        }

        return true;
    }
}