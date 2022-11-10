// --------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S103:Lines should not be too long", Justification = "<Pending>")]
[assembly: SuppressMessage("Info Code Smell", "S1309:Track uses of in-source issue suppressions", Justification = "<Pending>")]
[assembly: SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP004:Don't ignore created IDisposable", Justification = "<Pending>", Scope = "member", Target = "~T:Bem.ReactiveUI.Blazor.Extras.Components.Templates.AdvancedComponentBaseTemplate`1")]
[assembly: SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Extensions.ParameterViewExtensions.Compare(Microsoft.AspNetCore.Components.ParameterView,Microsoft.AspNetCore.Components.ParameterView)~System.ValueTuple{System.Boolean,System.Boolean}")]
[assembly: SuppressMessage("Critical Code Smell", "S1541:Methods and properties should not be too complex", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Internal.ChangeDetection.IsKnownImmutableType(System.Type)~System.Boolean")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1205:Partial elements should declare access", Justification = "<Pending>", Scope = "type", Target = "~T:Bem.ReactiveUI.Blazor.Extras.Components.Templates.AdvancedComponentBaseTemplate`1")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "<Pending>", Scope = "namespaceanddescendants", Target = "~N:Bem.ReactiveUI.Blazor.Extras")]
[assembly: SuppressMessage("Minor Code Smell", "S4018:All type parameters should be used in the parameter list to enable type inference", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Utils.ExpressionHelper.GetFieldValueExpression``2(System.Reflection.FieldInfo)~System.Linq.Expressions.Expression{System.Func{``0,``1}}")]
[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1108:Block statements should not contain embedded comments", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Internal.ChangeDetection.IsChanged``2(``0,``1)~System.Boolean")]
[assembly: SuppressMessage("Critical Code Smell", "S126:\"if ... else if\" constructs should end with \"else\" clauses", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Internal.ChangeDetection.IsChanged``2(``0,``1)~System.Boolean")]
[assembly: SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Internal.Bindings.InlineBinding`2.SubscribeToCollectionChanges``2(`0,``0)~System.IDisposable")]
[assembly: SuppressMessage("Critical Code Smell", "S1541:Methods and properties should not be too complex", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Extensions.ParameterViewExtensions.Compare(Microsoft.AspNetCore.Components.ParameterView,Microsoft.AspNetCore.Components.ParameterView)~System.ValueTuple{System.Boolean,System.Boolean}")]
[assembly: SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Extensions.ParameterViewExtensions.Compare(Microsoft.AspNetCore.Components.ParameterView,Microsoft.AspNetCore.Components.ParameterView)~System.ValueTuple{System.Boolean,System.Boolean}")]
[assembly: SuppressMessage("Minor Code Smell", "S4018:All type parameters should be used in the parameter list to enable type inference", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Extensions.ObjectExtensions.As``1(System.Object)~``0")]
[assembly: SuppressMessage("Minor Code Smell", "S4018:All type parameters should be used in the parameter list to enable type inference", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Internal.Bindings.InlineBinding`2.SubscribeToCollectionChanges``2(`0,``0)~System.IDisposable")]
[assembly: SuppressMessage("Minor Code Smell", "S4225:Extension methods should not extend \"object\"", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Extensions.ObjectExtensions.As``1(System.Object)~``0")]
[assembly: SuppressMessage("Minor Code Smell", "S3242:Method parameters should be declared with base types", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Internal.Bindings.InlineBindingStore.Bind``2(``0,System.Linq.Expressions.Expression{System.Func{``0,``1}})~Bem.ReactiveUI.Blazor.Extras.Internal.Bindings.IInlineBinding")]
[assembly: SuppressMessage("Major Code Smell", "S109:Magic numbers should not be used", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Utils.HashCodeCalculation.Add(System.Int32)")]
[assembly: SuppressMessage("Roslynator", "RCS1213:Remove unused member declaration", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Internal.Bindings.InlineBinding`2.SubscribeToCollectionChanges``2(`0,``0)~System.IDisposable")]
[assembly: SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Utils.ExpressionComparison.CandidateFor``1(``0)~``0")]
[assembly: SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Utils.ExpressionComparison.CandidateFor``1(``0)~``0")]
[assembly: SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "<Pending>", Scope = "member", Target = "~F:Bem.ReactiveUI.Blazor.Extras.Utils.ExpressionEnumeration._expressions")]
[assembly: SuppressMessage("Minor Code Smell", "S1694:An abstract class should have both abstract and concrete methods", Justification = "<Pending>", Scope = "type", Target = "~T:Bem.ReactiveUI.Blazor.Extras.Components.Templates.AdvancedComponentBaseTemplate`1")]
[assembly: SuppressMessage("Minor Code Smell", "S1694:An abstract class should have both abstract and concrete methods", Justification = "<Pending>", Scope = "type", Target = "~T:Bem.ReactiveUI.Blazor.Extras.Components.AdvancedComponentBase`1")]
[assembly: SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "<Pending>", Scope = "member", Target = "~P:Bem.ReactiveUI.Blazor.Extras.Internal.Bindings.InlineBinding`2.Value")]
