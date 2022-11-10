// --------------------------------------
// <copyright file="IInlineBinding.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.Internal.Bindings;

internal interface IInlineBinding : IDisposable
{
    bool KeepAlive { get; internal set; }
}

internal interface IInlineBinding<T> : IInlineBinding
{
    T? Value { get; set; }
}