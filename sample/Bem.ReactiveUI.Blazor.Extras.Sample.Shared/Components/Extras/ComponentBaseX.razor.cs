// --------------------------------------
// <copyright file="ComponentBaseX.razor.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bem.ReactiveUI.Blazor.Extras.Sample.Components.Extras
{
    public abstract partial class ComponentBaseX<TViewModel> : AdvancedComponentBase<TViewModel>
        where TViewModel : class, INotifyPropertyChanged
    {
        private int RenderCount { get; set; } = 1;

        private DateTime? _renderStart;

        private TimeSpan? RenderTime { get; set; }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            _renderStart ??= DateTime.Now;
            return base.SetParametersAsync(parameters);
        }

        protected override bool ShouldRender()
        {
            _renderStart ??= DateTime.Now;

            return base.ShouldRender();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            RenderCount++;
            if (_renderStart.HasValue)
            {
                RenderTime = DateTime.Now - _renderStart;
            }

            _renderStart = default;
        }
    }
}
