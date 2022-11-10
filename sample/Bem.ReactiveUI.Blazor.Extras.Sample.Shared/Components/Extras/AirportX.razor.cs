// --------------------------------------
// <copyright file="AirportX.razor.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Threading.Tasks;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.Components.Extras;

public partial class AirportX
{
    private Task UpdateAirportNameAsync(string name)
    {
        ViewModel!.Name = name;
        return ViewModel!.UpdateAirportNameAsync(name);
    }

    private Task ResetViewModelAsync()
    {
        return ViewModel!.ResetAsync();
    }
}