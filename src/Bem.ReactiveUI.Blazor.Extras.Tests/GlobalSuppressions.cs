// --------------------------------------
// <copyright file="GlobalSuppressions.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Unit Test", "NUnit2005:Consider using constraint model, Assert.That(...)")]
[assembly: SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP003:Dispose previous before re-assigning", Justification = "<Pending>", Scope = "namespaceanddescendants", Target = "~N:Bem.ReactiveUI.Blazor.Extras.Tests")]
[assembly: SuppressMessage("Minor Code Smell", "S4056:Overloads with a \"CultureInfo\" or an \"IFormatProvider\" parameter should be used", Justification = "<Pending>", Scope = "namespaceanddescendants", Target = "~N:Bem.ReactiveUI.Blazor.Extras.Tests")]
[assembly: SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "namespaceanddescendants", Target = "~N:Bem.ReactiveUI.Blazor.Extras.Tests.Components")]
[assembly: SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Tests.Components.ComponentWithTwoWayBindingTests.AssertComponent(Bunit.IRenderedComponent{Bem.ReactiveUI.Blazor.Extras.Tests.Components.ComponentWithTwoWayBinding},System.String,System.String,System.String,System.Int32,System.String,System.String,System.String)")]
[assembly: SuppressMessage("Major Code Smell", "S103:Lines should not be too long", Justification = "<Pending>")]
[assembly: SuppressMessage("Major Code Smell", "S4004:Collection properties should be readonly", Justification = "<Pending>", Scope = "member", Target = "~P:Bem.ReactiveUI.Blazor.Extras.Tests.ViewModels.TestViewModel.ObservableCollection")]
[assembly: SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>", Scope = "member", Target = "~P:Bem.ReactiveUI.Blazor.Extras.Tests.ViewModels.TestViewModel.ObservableCollection")]
[assembly: SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "<Pending>", Scope = "namespaceanddescendants", Target = "~N:Bem.ReactiveUI.Blazor.Extras.Tests")]
[assembly: SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "<Pending>", Scope = "type", Target = "~T:Bem.ReactiveUI.Blazor.Extras.Tests.Components.ComponentWithSingleViewModelTests")]
[assembly: SuppressMessage("Minor Code Smell", "S2219:Runtime type checking should be simplified", Justification = "<Pending>", Scope = "member", Target = "~M:Bem.ReactiveUI.Blazor.Extras.Tests.Utils.ExpressionEqualityComparerTests.Compare_Expressions")]
