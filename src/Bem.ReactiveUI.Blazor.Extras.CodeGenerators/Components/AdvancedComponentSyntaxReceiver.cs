// --------------------------------------
// <copyright file="AdvancedComponentSyntaxReceiver.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.CodeGenerators.Components;

internal sealed class AdvancedComponentSyntaxReceiver : ISyntaxContextReceiver
{
    internal List<AdvancedComponentContext> ComponentClassContexts { get; } = new();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is ClassDeclarationSyntax { AttributeLists.Count: > 0 } classDeclaration)
        {
            var symbol = context.SemanticModel.GetDeclaredSymbol(context.Node);

            if (symbol is INamedTypeSymbol typeSymbol &&
                typeSymbol.GetAttributes().Any(ad => ad.AttributeClass is { Name: "AdvancedComponentAttribute" }))
            {
                ComponentClassContexts.Add(new AdvancedComponentContext(classDeclaration, typeSymbol));
            }
        }
    }
}