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

[assembly: SuppressMessage("Roslynator", "CA5394:Random is an insecure random number generator", Justification = "<Pending>", Scope = "type", Target = "~T:Bem.ReactiveUI.Blazor.Extras.Sample.Components.Pages.Weather")]