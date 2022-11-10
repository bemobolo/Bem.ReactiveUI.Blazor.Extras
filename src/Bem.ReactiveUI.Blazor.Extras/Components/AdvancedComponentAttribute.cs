// --------------------------------------
// <copyright file="AdvancedComponentAttribute.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.Components;

/// <summary>
/// Marker attribute for classes cannot be inherited from <see cref="AdvancedComponentBase{TViewModel}" /> but still be able to use the features of the library.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class AdvancedComponentAttribute : Attribute;