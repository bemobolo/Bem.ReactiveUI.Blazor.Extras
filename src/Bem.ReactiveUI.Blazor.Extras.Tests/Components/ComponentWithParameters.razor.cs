// --------------------------------------
// <copyright file="ComponentWithParameters.razor.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.Tests.Components;

public partial class ComponentWithParameters
{
    [Parameter]
    public int IntegerParameter { get; set; }

    [Parameter]
    public object? ReferenceParameter { get; set; }

    private void ButtonClicked()
    {
        IntegerParameter++;
    }
}