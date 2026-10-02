// --------------------------------------
// <copyright file="DiagnosticInfo.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.CodeGenerators.Components;

/// <summary>
/// Equatable representation of a diagnostic to be reported by the incremental pipeline.
/// </summary>
internal sealed record DiagnosticInfo(
    DiagnosticDescriptor Descriptor,
    string FilePath,
    TextSpan TextSpan,
    LinePositionSpan LineSpan,
    string ComponentName,
    string BaseTypeName)
{
    internal static DiagnosticInfo Create(DiagnosticDescriptor descriptor, SyntaxNode node, string componentName, string baseTypeName)
    {
        var textSpan = TextSpan.FromBounds(node.SpanStart, node.SpanStart);
        var lineSpan = node.SyntaxTree.GetLineSpan(textSpan).Span;

        return new DiagnosticInfo(descriptor, node.SyntaxTree.FilePath, textSpan, lineSpan, componentName, baseTypeName);
    }

    internal Diagnostic ToDiagnostic() =>
        Diagnostic.Create(Descriptor, Location.Create(FilePath, TextSpan, LineSpan), ComponentName, BaseTypeName);
}