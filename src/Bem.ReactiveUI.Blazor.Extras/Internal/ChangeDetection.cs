// --------------------------------------
// <copyright file="ChangeDetection.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Collections.Immutable;
using Bem.ReactiveUI.Blazor.Extras.Extensions;

namespace Bem.ReactiveUI.Blazor.Extras.Internal;

internal static class ChangeDetection
{
    internal static bool IsChanged<T1, T2>(T1 oldValue, T2 newValue)
    {
        var changed = false;

        var oldIsNotNull = oldValue is not null;
        var newIsNotNull = newValue is not null;
        if (oldIsNotNull != newIsNotNull)
        {
            changed = true; // One's null and the other isn't, so different
        }
        else if (oldIsNotNull) // i.e., both are not null (considering previous check)
        {
            var oldValueType = oldValue!.GetType();
            var newValueType = newValue!.GetType();

            if (oldValueType != newValueType
                || CannotDetectChange(newValueType) // Maybe different
                || !oldValue.Equals(newValue)) // Definitely different
            {
                changed = true;
            }
        }

        // By now we know either both are null, or they are the same immutable type
        // and ThatType::Equals says the two values are equal.
        return changed;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool CannotDetectChange(Type? type) =>
        type != null && !typeof(INotifyPropertyChanged).IsAssignableFrom(type) && !IsKnownImmutableOrEquatableType(type);

    private static readonly HashSet<Type> _immutableGenericTypes = new(new[]
    {
        typeof(Lookup<,>), typeof(Nullable<>), typeof(Tuple<>), typeof(Tuple<,>), typeof(Tuple<,,>),
        typeof(Tuple<,,,>), typeof(Tuple<,,,,>), typeof(Tuple<,,,,,>), typeof(Tuple<,,,,,,>), typeof(Tuple<,,,,,,,>),
    });

    private static readonly Type[] _immutableCollectionInterfaces = new[]
    {
        typeof(IImmutableDictionary<,>), typeof(IImmutableSet<>), typeof(IImmutableList<>), typeof(IImmutableQueue<>), typeof(IImmutableStack<>),
    }.ToArray();

    private static readonly Dictionary<Type, bool> _knownImmutableOrEquatableTypes = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsImmutableOrEquatableGenericType(Type type)
    {
        var immutable = true;
        if (type.IsGenericType && (_immutableGenericTypes.Contains(type.GetGenericTypeDefinition()) ||
                                   Array.Exists(_immutableCollectionInterfaces, type.IsAssignableToGenericType)))
        {
            for (var i = 0; immutable && i < type.GenericTypeArguments.Length; i++)
            {
                if (!IsKnownImmutableOrEquatableType(type.GenericTypeArguments[i]))
                {
                    immutable = false;
                }
            }
        }
        else
        {
            immutable = false;
        }

        return immutable;
    }

    private static bool IsKnownImmutableOrEquatableType(Type type)
    {
        if (!_knownImmutableOrEquatableTypes.TryGetValue(type, out var immutableOrEquatable))
        {
            immutableOrEquatable =
                type.IsPrimitive ||
                (type.IsValueType &&
                    (type == typeof(decimal) ||
                     type == typeof(DateTime) ||
                     type == typeof(DateTimeOffset) ||
                     type == typeof(Guid) ||
                     type == typeof(TimeSpan))) ||
                 (!type.IsGenericType &&
                     (type == typeof(string) ||
                      type == typeof(Uri) ||
                      type == typeof(Type))) ||
                 (type.GetCustomAttribute<ImmutableObjectAttribute>()?.Immutable ?? false) ||
                type.GetInterface("IEquatable`1")?.GenericTypeArguments[0] == type ||
                IsImmutableOrEquatableGenericType(type);

            _knownImmutableOrEquatableTypes[type] = immutableOrEquatable;
        }

        return immutableOrEquatable;
    }
}