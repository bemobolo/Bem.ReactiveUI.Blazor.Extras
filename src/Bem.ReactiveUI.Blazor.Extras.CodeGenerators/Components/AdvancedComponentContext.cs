// --------------------------------------
// <copyright file="AdvancedComponentContext.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.CodeGenerators.Components;

internal sealed class AdvancedComponentContext
{
    internal AdvancedComponentContext(ClassDeclarationSyntax classDeclaration, INamedTypeSymbol componentSymbol)
    {
        ClassDeclaration = classDeclaration;
        ComponentSymbol = componentSymbol;
    }

    internal ClassDeclarationSyntax ClassDeclaration { get; }

    internal INamedTypeSymbol ComponentSymbol { get; }
}