// --------------------------------------
// <copyright file="InlineBinding.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Collections.Specialized;
using System.Reactive.Linq;
using Bem.ReactiveUI.Blazor.Extras.Components;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace Bem.ReactiveUI.Blazor.Extras.Internal.Bindings
{
    internal sealed class InlineBinding<TSender, TValue> : IInlineBinding<TValue>
        where TSender : IAdvancedComponent
    {
        private readonly IDisposable _propertyChangeSubscription;
        private readonly bool _isReactiveCollection;
        private readonly Action<TSender, TValue?> _propertySetter;
        private TSender _source;
        private IDisposable? _collectionChangeSubscription;
        private Delegate? _collectionSubscriptionDelegate;
        private TValue? _value;

        internal InlineBinding(TSender source, Expression<Func<TSender, TValue>> propertyExpression)
        {
            _source = source;

            var propertyType = typeof(TValue);
            _isReactiveCollection = propertyType.GetInterface(typeof(INotifyCollectionChanged).FullName!) != null &&
                                    propertyType.GetInterface(typeof(IEnumerable<>).FullName!) != null;

            _propertyChangeSubscription = source
                .WhenAnyValue(propertyExpression)
                .Select((value, i) => (value, i))
                .Subscribe(UpdateValueAndRaiseStateHasChanged);

            var valueParameter = Expression.Parameter(typeof(TValue));
            _propertySetter = Expression.Lambda<Action<TSender, TValue?>>(
                    Expression.Assign(propertyExpression.Body, valueParameter),
                    propertyExpression.Parameters[0],
                    valueParameter)
                .Compile();
        }

        public TValue? Value
        {
            get
            {
                ThrowIfDisposed();
                return _value;
            }

            set
            {
                ThrowIfDisposed();
                _propertySetter(_source, value);
            }
        }

        public bool KeepAlive { get; set; }

        private bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                _source = default!;
                _value = default!;
                _propertyChangeSubscription.Dispose();
                _collectionChangeSubscription?.Dispose();
                IsDisposed = true;
            }
        }

        private static IDisposable SubscribeToCollectionChanges<TCollection, T>(TSender source, TCollection collection)
            where TCollection : INotifyCollectionChanged, IEnumerable<T>
            where T : INotifyPropertyChanged =>
            collection.ToObservableChangeSet<TCollection, T>()
                .AutoRefresh()
                .Select((changeSet, i) => (changeSet, i))
                .Subscribe(
                    tuple =>
                    {
                        if (tuple.i > 0)
                        {
                            source.StateHasChanged();
                        }
                    });

        private static Delegate CreateCollectionSubscriptionDelegate()
        {
            var collectionType = typeof(TValue);

            var itemType = collectionType.GetInterface(typeof(IEnumerable<>).FullName!)!.GenericTypeArguments[0];
            var valueParameter = Expression.Parameter(collectionType);
            var sourceParameter = Expression.Parameter(typeof(TSender));
            var lambdaExpression = Expression.Lambda(
                Expression.Call(
                    typeof(InlineBinding<TSender, TValue>),
                    "SubscribeToCollectionChanges",
                    [collectionType, itemType],
                    sourceParameter,
                    valueParameter),
                sourceParameter,
                valueParameter);
            return lambdaExpression.Compile();
        }

        private void UpdateCollectionSubscription(TValue value)
        {
            if (_isReactiveCollection)
            {
                _collectionChangeSubscription?.Dispose();

                if (value is not null)
                {
                    _collectionSubscriptionDelegate ??= CreateCollectionSubscriptionDelegate();

                    _collectionChangeSubscription =
                        (IDisposable)_collectionSubscriptionDelegate!.DynamicInvoke(_source, value)!;
                }
            }
        }

        private void UpdateValueAndRaiseStateHasChanged((TValue Value, int Index) tuple)
        {
            _value = tuple.Value;

            UpdateCollectionSubscription(tuple.Value);

            if (tuple.Index > 0)
            {
                _source.StateHasChanged();
            }
        }

        private void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(typeof(InlineBinding<TSender, TValue>).FullName!);
            }
        }
    }
}