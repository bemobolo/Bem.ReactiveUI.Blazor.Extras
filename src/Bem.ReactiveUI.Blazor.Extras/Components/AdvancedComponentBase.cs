// --------------------------------------
// <copyright file="AdvancedComponentBase.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

namespace Bem.ReactiveUI.Blazor.Extras.Components
{
    /// <summary>
    /// Base component for user specified components to be able to use the capabilities of the library.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    [AdvancedComponent]
    public abstract partial class AdvancedComponentBase<TViewModel> : ReactiveComponentBase<TViewModel>
        where TViewModel : class, INotifyPropertyChanged;
}