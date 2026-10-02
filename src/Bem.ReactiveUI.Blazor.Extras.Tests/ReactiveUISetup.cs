// --------------------------------------
// <copyright file="ReactiveUISetup.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using NUnit.Framework;
using ReactiveUI.Builder;

namespace Bem.ReactiveUI.Blazor.Extras.Tests;

[SetUpFixture]
public class ReactiveUISetup
{
    [OneTimeSetUp]
    public void Initialize()
    {
        _ = RxAppBuilder.CreateReactiveUIBuilder()
            .WithBlazor()
            .BuildApp();
    }
}