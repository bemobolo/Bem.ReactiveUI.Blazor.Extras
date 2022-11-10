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
public class AdvancedComponentGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new AdvancedComponentSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxContextReceiver is AdvancedComponentSyntaxReceiver { ComponentClassContexts.Count: > 0 } syntaxReceiver)
        {
            foreach (var componentClassContext in syntaxReceiver.ComponentClassContexts)
            {
                AddComponent(context, componentClassContext);
            }
        }
    }

    private static void AddComponent(
        in GeneratorExecutionContext context,
        AdvancedComponentContext componentContext)
    {
        var componentClass = componentContext.ClassDeclaration;

        if (componentClass.Modifiers.Any(SyntaxKind.PartialKeyword))
        {
            var componentBaseType =
                context.Compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Components.ComponentBase")!;

            if (!componentContext.ComponentSymbol.InheritsFrom(componentBaseType))
            {
                ReportDiagnostics(Errors.WrongBaseClassError, context, componentClass, componentClass.Identifier, componentBaseType.GetFullMetadataName());
            }
            else if (HasMutableParameters())
            {
                ReportDiagnostics(Errors.HasMutableParametersError, context, componentClass, componentClass.Identifier, componentBaseType.GetFullMetadataName());
            }
            else if (componentClass.TypeParameterList is not null &&
                    componentClass.TypeParameterList.Parameters.Count != 0)
            {
                AddGenericComponent(context, componentContext);
            }
            else
            {
                AddNonGenericComponent(context, componentContext);
            }
        }
        else
        {
            ReportDiagnostics(Errors.NotPartialError, context, componentClass, componentClass.Identifier);
        }

        bool HasMutableParameters()
        {
            return componentContext.ComponentSymbol.GetMembers()
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
    }

    private static void AddNonGenericComponent(in GeneratorExecutionContext context, AdvancedComponentContext componentContext)
    {
        var componentSourceText = GenerateComponentSource(componentContext);

        context.AddSource(componentContext.ClassDeclaration.Identifier + ".g.cs", componentSourceText);
    }

    private static void AddGenericComponent(in GeneratorExecutionContext context, AdvancedComponentContext componentContext)
    {
        var componentClass = componentContext.ClassDeclaration;
        var typeParameterName = componentClass.TypeParameterList!.Parameters[0].Identifier.ValueText;

        var componentSourceText = GenerateComponentSource(componentContext, typeParameterName);

        context.AddSource(componentClass.Identifier + "`1.g.cs", componentSourceText);
    }

    private static void ReportDiagnostics(DiagnosticDescriptor descriptor, in GeneratorExecutionContext context, in SyntaxNode componentClass, params object?[]? messageArgs)
    {
        context.ReportDiagnostic(
            Diagnostic.Create(
                descriptor,
                Location.Create(
                    componentClass.SyntaxTree,
                    TextSpan.FromBounds(componentClass.SpanStart, componentClass.SpanStart)),
                messageArgs));
    }

    private static string GetTemplateFileFromEmbeddedResource(string fileName)
    {
        using var stream = typeof(AdvancedComponentGenerator).Assembly
            .GetManifestResourceStream(typeof(AdvancedComponentGenerator), fileName)!;
        using var sr = new StreamReader(stream);

        return sr.ReadToEnd();
    }

    private static SourceText GenerateComponentSource(AdvancedComponentContext componentContext, string? typeParameterName = null)
    {
        var componentNamespace = componentContext.ComponentSymbol.ContainingNamespace;
        var componentClassName = componentContext.ClassDeclaration.Identifier;

        var template = GetTemplateFileFromEmbeddedResource("AdvancedComponentBaseTemplate.cs")
            .Replace(": ComponentBase, ", ": ")
            .Replace("AdvancedComponentBaseTemplate", componentClassName.ToString())
            .Replace("namespace Bem.ReactiveUI.Blazor.Extras.Components.Templates", $"namespace {componentNamespace}");

        if (typeParameterName == null)
        {
            template = template.Replace("<TViewModel>", string.Empty);
            template = Regex.Replace(template, @"\r?\n\s+where TViewModel[^\r\n]+", string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
        }
        else
        {
            template = template.Replace("TViewModel", typeParameterName);
        }

        template = AdjustDisposeMethods(componentContext, template);

        return SourceText.From(template, Encoding.UTF8);
    }

    private static string AdjustDisposeMethods(AdvancedComponentContext componentContext, string template)
    {
        var (hasDispose, hasVirtualDispose, hasDisposing, hasVirtualDisposing) = CheckBaseDisposeMethods();

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
            template = Regex.Replace(template, @"\r?\n\s+#region IDisposable[^#]+#endregion", string.Empty, RegexOptions.Multiline | RegexOptions.Compiled);
        }

        return template;

        (bool HasDispose, bool HasVirtualDispose, bool HasDisposing, bool HasVirtualDisposing) CheckBaseDisposeMethods()
        {
            var disposeMethods = componentContext.ComponentSymbol.GetMethods("Dispose")
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

            return (hasDispose, hasVirtualDispose, hasDisposing, hasVirtualDisposing);
        }

        string SetDisposePatternModifiers(string? disposeModifiers, string? disposingModifiers, bool removeBaseDisposeCall, bool removeBaseDisposingCall)
        {
            template = template
                .Replace("new void Dispose()", $"{disposeModifiers} void Dispose()")
                .Replace("new void Dispose(b", $"{disposingModifiers} void Dispose(b");

            if (removeBaseDisposeCall)
            {
                template = Regex.Replace(template, @"\r?\n\s+base\.Dispose\(\);", string.Empty, RegexOptions.Compiled);
            }

            if (removeBaseDisposingCall)
            {
                template = Regex.Replace(template, @"\r?\n\s+base\.Dispose\(disposing\);", string.Empty, RegexOptions.Compiled);
            }

            return template;
        }
    }
}