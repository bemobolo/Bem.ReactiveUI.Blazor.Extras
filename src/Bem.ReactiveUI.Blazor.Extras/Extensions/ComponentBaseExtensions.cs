// --------------------------------------
// <copyright file="ComponentBaseExtensions.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.Extensions;

internal static class ComponentBaseExtensions
{
    private static readonly Func<ComponentBase, EventCallbackWorkItem, object?, Task> _handleEventAsyncDelegate =
        CreateBaseHandleEventAsyncDelegate();

    internal static Task HandleEventAsync(this ComponentBase @this, in EventCallbackWorkItem workItem, object? obj)
    {
        return _handleEventAsyncDelegate(@this, workItem, obj);
    }

    private static Func<ComponentBase, EventCallbackWorkItem, object?, Task> CreateBaseHandleEventAsyncDelegate()
    {
        var componentParam = Expression.Parameter(typeof(ComponentBase));
        var workItemParam = Expression.Parameter(typeof(EventCallbackWorkItem));
        var objectParam = Expression.Parameter(typeof(object));

        return Expression.Lambda<Func<ComponentBase, EventCallbackWorkItem, object?, Task>>(
                Expression.Call(componentParam, typeof(ComponentBase).GetMethod($"{typeof(IHandleEvent).FullName}.{nameof(IHandleEvent.HandleEventAsync)}", BindingFlags.Instance | BindingFlags.NonPublic)!, workItemParam, objectParam), componentParam, workItemParam, objectParam)
            .Compile();
    }
}