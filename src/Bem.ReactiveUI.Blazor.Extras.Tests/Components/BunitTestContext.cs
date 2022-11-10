// --------------------------------------
// <copyright file="BunitTestContext.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using Bunit;
using NUnit.Framework;

namespace Bem.ReactiveUI.Blazor.Extras.Tests.Components
{
    /// <summary>
    /// Test context wrapper for bUnit.
    /// Read more about using <see cref="BunitTestContext"/> <seealso href="https://bunit.dev/docs/getting-started/writing-tests.html#remove-boilerplate-code-from-tests">here</seealso>.
    /// </summary>
    public abstract class BunitTestContext : TestContextWrapper
    {
        [SetUp]
        public virtual void Setup()
        {
            TestContext = new Bunit.TestContext();
        }

        [TearDown]
        public virtual void TearDown() => TestContext?.Dispose();
    }
}