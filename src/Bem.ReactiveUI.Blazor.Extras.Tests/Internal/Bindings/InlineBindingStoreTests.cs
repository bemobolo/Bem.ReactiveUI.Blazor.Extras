// --------------------------------------
// <copyright file="InlineBindingStoreTests.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Bem.ReactiveUI.Blazor.Extras.Internal.Bindings;
using Bem.ReactiveUI.Blazor.Extras.Tests.Components;
using Bem.ReactiveUI.Blazor.Extras.Tests.ViewModels;
using Bunit;
using NUnit.Framework;

namespace Bem.ReactiveUI.Blazor.Extras.Tests.Internal.Bindings
{
    public class InlineBindingStoreTests : BunitTestContext
    {
        [Test]
        public void Dispose_Disposes_Bindings()
        {
            using var component = RenderComponent<ComponentWithSingleViewModel>(ComponentParameter.CreateParameter("ViewModel", new TestViewModel()));

            InlineBindingStore store;
            InlineBinding<ComponentWithSingleViewModel, int> binding;
            using (store = new InlineBindingStore())
            {
                binding = store.Bind(component.Instance, c => c.ViewModel!.Value).As<InlineBinding<ComponentWithSingleViewModel, int>>()!;

                Assert.That(1, Is.EqualTo(store.Count));
            }

            Assert.That(0, Is.EqualTo(store.Count));
            _ = Assert.Throws<ObjectDisposedException>(() => binding.Value = 2);
        }

        [Test]
        public void Sweep_Disposes_Bindings_Not_Used_Any_More()
        {
            using var component = RenderComponent<ComponentWithSingleViewModel>(ComponentParameter.CreateParameter("ViewModel", new TestViewModel()));

            using var store = new InlineBindingStore();

            using var binding = store.Bind(component.Instance, c => c.ViewModel!.Value).As<InlineBinding<ComponentWithSingleViewModel, int>>()!;

            Assert.That(1, Is.EqualTo(store.Count));

            binding.KeepAlive = false;
            store.Sweep();

            Assert.That(0, Is.EqualTo(store.Count));
            _ = Assert.Throws<ObjectDisposedException>(() => binding.Value = 2);
        }

        [Test]
        public void Sweep_Keeps_Active_Bindings()
        {
            using var component = RenderComponent<ComponentWithSingleViewModel>(ComponentParameter.CreateParameter("ViewModel", new TestViewModel()));

            using var store = new InlineBindingStore();

            using var binding = store.Bind(component.Instance, c => c.ViewModel!.Value).As<InlineBinding<ComponentWithSingleViewModel, int>>()!;

            Assert.That(1, Is.EqualTo(store.Count));

            store.Sweep();

            Assert.That(1, Is.EqualTo(store.Count));
            Assert.DoesNotThrow(() => binding.Value = 2);
        }
    }
}
