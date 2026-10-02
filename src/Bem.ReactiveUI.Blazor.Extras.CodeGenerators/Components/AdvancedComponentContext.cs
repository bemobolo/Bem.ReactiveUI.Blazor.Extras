// --------------------------------------
// <copyright file="AdvancedComponentContext.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.CodeGenerators.Components;

/// <summary>
/// Equatable model of a component collected by the incremental pipeline. It must not hold syntax nodes or symbols so that the pipeline can cache it between compilations.
/// </summary>
internal sealed record AdvancedComponentContext(
    string ClassName,
    string Namespace,
    string? TypeParameterName,
    DisposeMethods DisposeMethods,
    DiagnosticInfo? Diagnostic);

internal readonly record struct DisposeMethods(
    bool HasDispose,
    bool HasVirtualDispose,
    bool HasDisposing,
    bool HasVirtualDisposing);