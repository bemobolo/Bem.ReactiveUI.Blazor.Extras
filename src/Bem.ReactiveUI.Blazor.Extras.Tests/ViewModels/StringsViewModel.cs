// --------------------------------------
// <copyright file="StringsViewModel.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Bem.ReactiveUI.Blazor.Extras.Tests.ViewModels;

public partial class StringsViewModel : ReactiveObject
{
    [Reactive]
    public partial string? First { get; set; }

    [Reactive]
    public partial string? Last { get; set; }

    [Reactive]
    public partial string? City { get; set; }
}