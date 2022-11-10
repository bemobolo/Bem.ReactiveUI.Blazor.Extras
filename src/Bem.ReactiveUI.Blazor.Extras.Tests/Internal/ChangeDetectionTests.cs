// --------------------------------------
// <copyright file="ChangeDetectionTests.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Collections.Immutable;
using Bem.ReactiveUI.Blazor.Extras.Internal;
using NUnit.Framework;

namespace Bem.ReactiveUI.Blazor.Extras.Tests.Internal
{
    public class ChangeDetectionTests
    {
        [Test]
        public void IsChanged_Detects_Equatable_Tree_Not_Changed()
        {
            Equatable[] equatables = { new("1string"), new("list") };
            Lookup<int, decimal> lookup = (Lookup<int, decimal>)Enumerable.Range(1, 3).ToLookup(i => i % 3, i => (decimal)i);
            var dictionary = ImmutableDictionary.Create<IImmutableList<Equatable?>, Lookup<int, decimal>>()
                .Add(ImmutableList.Create<Equatable?>(equatables), lookup);

            var obj1 =
                new Tuple<TimeSpan, Tuple<int, string>,
                    ImmutableDictionary<IImmutableList<Equatable?>, Lookup<int, decimal>>>(
                    TimeSpan.FromDays(10),
                    new Tuple<int, string>(3, "testString"),
                    dictionary);

            var obj2 =
                new Tuple<TimeSpan, Tuple<int, string>,
                    ImmutableDictionary<IImmutableList<Equatable?>, Lookup<int, decimal>>>(
                    TimeSpan.FromDays(10),
                    new Tuple<int, string>(3, "testString"),
                    dictionary);

            _ = ChangeDetection.IsChanged(obj1, obj2);
            var changed = ChangeDetection.IsChanged(obj1, obj2);

            Assert.That(changed, Is.False);
        }

        [Test]
        public void IsChanged_detect_immutable_tree_as_not_changed()
        {
            Immutable[] immutables = { new("1string"), new("list") };
            Lookup<int, decimal> lookup = (Lookup<int, decimal>)Enumerable.Range(1, 3).ToLookup(i => i % 3, i => (decimal)i);
            var dictionary = ImmutableDictionary.Create<IImmutableList<Immutable?>, Lookup<int, decimal>>()
                .Add(ImmutableList.Create<Immutable?>(immutables), lookup);

            var obj1 =
                new Tuple<TimeSpan, Tuple<int, string>,
                    ImmutableDictionary<IImmutableList<Immutable?>, Lookup<int, decimal>>>(
                    TimeSpan.FromDays(10),
                    new Tuple<int, string>(3, "testString"),
                    dictionary);

            var obj2 =
                new Tuple<TimeSpan, Tuple<int, string>,
                    ImmutableDictionary<IImmutableList<Immutable?>, Lookup<int, decimal>>>(
                    TimeSpan.FromDays(10),
                    new Tuple<int, string>(3, "testString"),
                    dictionary);

            _ = ChangeDetection.IsChanged(obj1, obj2);
            var changed = ChangeDetection.IsChanged(obj1, obj2);

            Assert.That(changed, Is.False);
        }

        [Test]
        public void IsChanged_detect_changes_in_object_tree_having_mutable_types()
        {
            Mutable[] mutableList = { new("1string"), new("list") };
            Lookup<int, decimal> lookup = (Lookup<int, decimal>)Enumerable.Range(1, 3).ToLookup(i => i % 3, i => (decimal)i);
            var dictionary = ImmutableDictionary.Create<IImmutableList<Mutable?>, Lookup<int, decimal>>()
                .Add(ImmutableList.Create<Mutable?>(mutableList), lookup);

            var obj1 =
                new Tuple<TimeSpan, Tuple<int, string>,
                    ImmutableDictionary<IImmutableList<Mutable?>, Lookup<int, decimal>>>(
                    TimeSpan.FromDays(10),
                    new Tuple<int, string>(3, "testString"),
                    dictionary);

            var obj2 =
                new Tuple<TimeSpan, Tuple<int, string>,
                    ImmutableDictionary<IImmutableList<Mutable?>, Lookup<int, decimal>>>(
                    TimeSpan.FromDays(10),
                    new Tuple<int, string>(3, "testString"),
                    dictionary);

            var changed = ChangeDetection.IsChanged(obj1, obj2);

            Assert.That(changed, Is.True);
        }
    }
}