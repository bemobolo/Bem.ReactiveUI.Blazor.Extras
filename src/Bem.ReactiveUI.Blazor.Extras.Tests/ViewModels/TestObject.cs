// --------------------------------------
// <copyright file="TestObject.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bem.ReactiveUI.Blazor.Extras.Tests.ViewModels;

public class TestObject : ReactiveObject
{
    public TestObject(int value)
    {
        Value = value;
    }

    [Reactive]
    public int Value { get; set; }
}