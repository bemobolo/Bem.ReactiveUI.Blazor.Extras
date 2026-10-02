// --------------------------------------
// <copyright file="AdvancedComponentGenerator.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Text.RegularExpressions;
using Bem.ReactiveUI.Blazor.Extras.CodeGenerators.Extensions;

namespace Bem.ReactiveUI.Blazor.Extras.CodeGenerators.Components;

[Generator]
public class AdvancedComponentGenerator : IIncrementalGenerator
{
    private const string AdvancedComponentAttributeName = "Bem.ReactiveUI.Blazor.Extras.Components.AdvancedComponentAttribute";
    private const string ComponentBaseName = "Microsoft.AspNetCore.Components.ComponentBase";

    private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(1);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var componentContexts = context.SyntaxProvider.ForAttributeWithMetadataName(
            AdvancedComponentAttributeName,
            static (node, _) => node is ClassDeclarationSyntax,
            static (attributeContext, _) => CreateComponentContext(attributeContext));

        context.RegisterSourceOutput(componentContexts, static (sourceContext, componentContext) => AddComponent(sourceContext, componentContext));
    }

    private static AdvancedComponentContext CreateComponentContext(GeneratorAttributeSyntaxContext attributeContext)
    {
        var componentClass = (ClassDeclarationSyntax)attributeContext.TargetNode;
        var componentSymbol = (INamedTypeSymbol)attributeContext.TargetSymbol;
        var componentClassName = componentClass.Identifier.ToString();
        var componentBaseType = attributeContext.SemanticModel.Compilation.GetTypeByMetadataName(ComponentBaseName)!;

        DiagnosticInfo? diagnostic = null;

        if (!componentClass.Modifiers.Any(SyntaxKind.PartialKeyword))
        {
            diagnostic = DiagnosticInfo.Create(Errors.NotPartialError, componentClass, componentClassName, string.Empty);
        }
        else if (!componentSymbol.InheritsFrom(componentBaseType))
        {
            diagnostic = DiagnosticInfo.Create(Errors.WrongBaseClassError, componentClass, componentClassName, componentBaseType.GetFullMetadataName());
        }
        else if (HasMutableParameters())
        {
            diagnostic = DiagnosticInfo.Create(Errors.HasMutableParametersError, componentClass, componentClassName, componentBaseType.GetFullMetadataName());
        }

        var typeParameterName = componentClass.TypeParameterList is { Parameters.Count: > 0 } typeParameterList
            ? typeParameterList.Parameters[0].Identifier.ValueText
            : null;

        return new AdvancedComponentContext(
            componentClassName,
            componentSymbol.ContainingNamespace.ToString(),
            typeParameterName,
            CheckBaseDisposeMethods(),
            diagnostic);

        bool HasMutableParameters()
        {
            return componentSymbol.GetMembers()
                .Any(
                    m =>
                    {
                        if (m.Kind == SymbolKind.Property && m.GetAttributes()
                                .Any(ad => ad.AttributeClass is { Name: "ParameterAttribute" }))
                        {
                            var property = (IPropertySymbol)m;
                            var propertyType = property.Type;

                            return propertyType.IsReferenceType &&
                                   !propertyType.AllInterfaces.Any(s => s.Name == "INotifyPropertyChanged");
                        }

                        return false;
                    });
        }

        DisposeMethods CheckBaseDisposeMethods()
        {
            var disposeMethods = componentSymbol.GetMethods("Dispose")
                .Where(
                    method => !method.IsStatic &&
                              method.DeclaredAccessibility != Accessibility.Internal &&
                              method.DeclaredAccessibility != Accessibility.Private)
                .ToArray();

            var disposeMethod = Array.Find(disposeMethods, ms => ms.Parameters.Length == 0);

            var disposingMethod = Array.Find(disposeMethods, ms => ms.Parameters.Length == 1 && ms.Parameters[0].Type.SpecialType == SpecialType.System_Boolean);

            var hasDispose = disposeMethod != null;
            var hasVirtualDispose = hasDispose && disposeMethod!.IsVirtual;
            var hasDisposing = disposingMethod != null;
            var hasVirtualDisposing = hasDisposing && disposingMethod!.IsVirtual;

            return new DisposeMethods(hasDispose, hasVirtualDispose, hasDisposing, hasVirtualDisposing);
        }
    }

    private static void AddComponent(SourceProductionContext context, AdvancedComponentContext componentContext)
    {
        if (componentContext.Diagnostic is not null)
        {
            context.ReportDiagnostic(componentContext.Diagnostic.ToDiagnostic());
            return;
        }

        var hintName = componentContext.TypeParameterName is null
            ? componentContext.ClassName + ".g.cs"
            : componentContext.ClassName + "`1.g.cs";

        context.AddSource(hintName, GenerateComponentSource(componentContext));
    }

    private static string GetTemplateFileFromEmbeddedResource(string fileName)
    {
        using var stream = typeof(AdvancedComponentGenerator).Assembly
            .GetManifestResourceStream(typeof(AdvancedComponentGenerator), fileName)!;
        using var sr = new StreamReader(stream);

        return sr.ReadToEnd();
    }

    private static SourceText GenerateComponentSource(AdvancedComponentContext componentContext)
    {
        var template = GetTemplateFileFromEmbeddedResource("AdvancedComponentBaseTemplate.cs")
            .Replace(": ComponentBase, ", ": ")
            .Replace("AdvancedComponentBaseTemplate", componentContext.ClassName)
            .Replace("namespace Bem.ReactiveUI.Blazor.Extras.Components.Templates", $"namespace {componentContext.Namespace}");

        if (componentContext.TypeParameterName == null)
        {
            template = template.Replace("<TViewModel>", string.Empty);
            template = Regex.Replace(template, @"\r?\n\s+where TViewModel[^\r\n]+", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline, RegexTimeout);
        }
        else
        {
            template = template.Replace("TViewModel", componentContext.TypeParameterName);
        }

        template = AdjustDisposeMethods(componentContext, template);

        return SourceText.From(template, Encoding.UTF8);
    }

    private static string AdjustDisposeMethods(AdvancedComponentContext componentContext, string template)
    {
        var (hasDispose, hasVirtualDispose, hasDisposing, hasVirtualDisposing) = componentContext.DisposeMethods;

        template = (hasDispose, hasVirtualDispose, hasDisposing, hasVirtualDisposing) switch
        {
            (_, _, _, true) => SetDisposePatternModifiers(null, "override", true, false),
            ( true, false,  true, false) => SetDisposePatternModifiers("new", "new virtual", false, true),
            ( true, false, false, false) => SetDisposePatternModifiers("new", "virtual", false, true),
            (false, false,  true, false) => SetDisposePatternModifiers(string.Empty, "new virtual", true, false),
            (false, false, false, false) => SetDisposePatternModifiers(string.Empty, "virtual", true, true),
            (false,  true,  true, false) => SetDisposePatternModifiers("override", "new virtual", false, true),
            (false,  true, false, false) => SetDisposePatternModifiers("override", "virtual", false, true),
            _ => throw new InvalidOperationException(),
        };

        if (hasVirtualDisposing)
        {
            template = Regex.Replace(template, @"\r?\n\s+#region IDisposable[^#]+#endregion", string.Empty, RegexOptions.Multiline | RegexOptions.Compiled, RegexTimeout);
        }

        return template;

        string SetDisposePatternModifiers(string? disposeModifiers, string? disposingModifiers, bool removeBaseDisposeCall, bool removeBaseDisposingCall)
        {
            template = template
                .Replace("new void Dispose()", $"{disposeModifiers} void Dispose()")
                .Replace("new void Dispose(b", $"{disposingModifiers} void Dispose(b");

            if (removeBaseDisposeCall)
            {
                template = Regex.Replace(template, @"\r?\n\s+base\.Dispose\(\);", string.Empty, RegexOptions.Compiled, RegexTimeout);
            }

            if (removeBaseDisposingCall)
            {
                template = Regex.Replace(template, @"\r?\n\s+base\.Dispose\(disposing\);", string.Empty, RegexOptions.Compiled, RegexTimeout);
            }

            return template;
        }
    }
}