// --------------------------------------
// <copyright file="TestViewModel.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Bem.ReactiveUI.Blazor.Extras.Tests.ViewModels;

public partial class TestViewModel : ReactiveObject
{
    [Reactive]
    public partial int Value { get; set; }

    [Reactive]
    public partial TestObject? TestObject { get; set; }

    [Reactive]
    public partial ObservableCollection<TestObject>? ObservableCollection { get; set; }
}