// --------------------------------------
// <copyright file="Errors.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.CodeGenerators.Components;

internal static class Errors
{
    internal static readonly DiagnosticDescriptor NotPartialError = new(
        "RUIX1",
        "Component needs to be partial",
        "Component class '{0}' needs to be partial",
        "Bem.ReactiveUI.Blazor.Extras.CodeGenerators",
        DiagnosticSeverity.Error,
        true);

    internal static readonly DiagnosticDescriptor WrongBaseClassError = new(
        "RUIX2",
        "Missing component base class",
        "Component class '{0}' needs to be assignable to '{1}'",
        "Bem.ReactiveUI.Blazor.Extras.CodeGenerators",
        DiagnosticSeverity.Error,
        true);

    internal static readonly DiagnosticDescriptor HasMutableParametersError = new(
        "RUIX3",
        "Component has mutable parameters",
        "Component class '{0}' shouldn't have mutable [Parameter] properties as those eliminate all performance gains",
        "Bem.ReactiveUI.Blazor.Extras.CodeGenerators",
        DiagnosticSeverity.Warning,
        true);
}