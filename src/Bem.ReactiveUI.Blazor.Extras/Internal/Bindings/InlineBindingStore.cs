// --------------------------------------
// <copyright file="InlineBindingStore.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Collections.Concurrent;
using Bem.ReactiveUI.Blazor.Extras.Components;
using Bem.ReactiveUI.Blazor.Extras.Utils;

namespace Bem.ReactiveUI.Blazor.Extras.Internal.Bindings;

internal sealed class InlineBindingStore : IDisposable
{
    private readonly ConcurrentDictionary<BindingKey, IInlineBinding> _bindings;

    internal InlineBindingStore()
    {
        _bindings = new ConcurrentDictionary<BindingKey, IInlineBinding>();
    }

    public void Dispose()
    {
        foreach (var kvp in _bindings)
        {
            kvp.Value.Dispose();
        }

        _bindings.Clear();
    }

    internal int Count => _bindings.Count;

    internal IInlineBinding Bind<TSender, TValue>(TSender sender, Expression<Func<TSender, TValue>> propertyExpression)
        where TSender : IAdvancedComponent
    {
        var key = new BindingKey(sender, propertyExpression);
        var binding = _bindings.AddOrUpdate(key, _ => new InlineBinding<TSender, TValue>(sender, propertyExpression), (_, binding) => binding);

        binding.KeepAlive = true;

        return binding;
    }

    internal void Sweep()
    {
        foreach (var kvp in _bindings.ToList())
        {
            if (!kvp.Value.KeepAlive)
            {
                if (_bindings.TryRemove(kvp.Key, out _))
                {
                    kvp.Value.Dispose();
                }
            }
            else
            {
                kvp.Value.KeepAlive = false;
            }
        }
    }

    private readonly struct BindingKey(object source, Expression propertyExpression) : IEquatable<BindingKey>
    {
        private readonly object _source = source;

        private readonly Expression _propertyExpression = propertyExpression;

        private readonly int _hashCode = HashCode.Combine(source.GetHashCode(), ExpressionEqualityComparer.Instance.GetHashCode(propertyExpression));

        public override bool Equals(object? obj)
        {
            return obj is BindingKey key &&
                   Equals(this, key);
        }

        public bool Equals(BindingKey other)
        {
            return EqualityComparer<object?>.Default.Equals(_source, other._source) &&
                ExpressionEqualityComparer.Instance.Equals(_propertyExpression, other._propertyExpression);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }
    }
}