// --------------------------------------
// <copyright file="TestViewModel.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bem.ReactiveUI.Blazor.Extras.Tests.ViewModels;

public class TestViewModel : ReactiveObject
{
    [Reactive]
    public int Value { get; set; }

    [Reactive]
    public TestObject? TestObject { get; set; }

    [Reactive]
    public ObservableCollection<TestObject>? ObservableCollection { get; set; }
}