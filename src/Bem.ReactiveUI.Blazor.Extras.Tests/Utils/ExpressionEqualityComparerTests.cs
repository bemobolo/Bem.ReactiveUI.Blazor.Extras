// Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com
//
// --------------------------------------
// <copyright file="ExpressionEqualityComparerTests.cs" company="Daniel Balogh">
//     Copyright (c) Daniel Balogh. All rights reserved.
//     Licensed under the GNU Generic Public License 3.0 license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------

using System.Linq.Expressions;
using Bem.ReactiveUI.Blazor.Extras.Utils;
using NUnit.Framework;

namespace Bem.ReactiveUI.Blazor.Extras.Tests.Utils
{
    public class ExpressionEqualityComparerTests
    {
        [Test]
        public void Compare_Expressions()
        {
            AssertAreEqual<int, int>(x => x, x => x);

            AssertAreNotEqual<int, int>(x => -x, x => x);

            AssertAreNotEqual<int, string>(x => string.Empty + x, x => "a" + x);

            AssertAreEqual<int, bool>(x => (x != 2) && (x < 3), x => (x != 2) && (x < 3));

            AssertAreNotEqual<int, bool>(x => (x != 3) && (x <= 3), x => (x != 3) && (x < 3));

            AssertAreEqual<TestClass, bool>(p => p.Id == 2, p => p.Id == 2);

            AssertAreNotEqual<TestClass, bool>(p => p.Id == 2, p => p.Pid == 2);

            AssertAreEqual<TestClass, string>(p => p.ToString(), p => p.ToString());

            AssertAreNotEqual<TestClass, string>(p => p.ToString(), p => p.ToNormalizedString());

            AssertAreNotEqual<TestClass, string>(p => ((ITestClass)p).ToString()!, p => p.ToString());

            AssertAreEqual<TestClass, bool>(p => p is TestClass, p => p is TestClass);

            AssertAreNotEqual<TestClass, bool>(p => p is ITestClass, p => p is TestClass);

            AssertAreEqual<TestClass, int>(p => ~p.Id, p => ~p.Id);

            AssertAreNotEqual<TestClass, int>(p => ~p.Id, p => -p.Id);

            AssertAreEqual(() => new TestClass(), () => new TestClass());

            AssertAreNotEqual(() => new BaseClass(), () => new TestClass());
        }

        private static void AssertAreEqual<T>(Expression<Func<T>> a, Expression<Func<T>> b)
        {
            AssertEqual(a, b);
        }

        private static void AssertAreEqual<T1, T2>(Expression<Func<T1, T2>> a, Expression<Func<T1, T2>> b)
        {
            AssertEqual(a, b);
        }

        private static void AssertAreNotEqual<T>(Expression<Func<T>> a, Expression<Func<T>> b)
        {
            AssertNotEqual(a, b);
        }

        private static void AssertAreNotEqual<T1, T2>(Expression<Func<T1, T2>> a, Expression<Func<T1, T2>> b)
        {
            AssertNotEqual(a, b);
        }

        private static void AssertEqual(Expression a, Expression b)
        {
            Assert.That(AreEqual(a, b), Is.True, $"'{a}' and '{b}' expected to be equal");

            AssertHashCodeEqual(a, b);
        }

        private static void AssertNotEqual(Expression a, Expression b)
        {
            Assert.That(AreEqual(a, b), Is.False, $"'{a}' and '{b}' expected to be not equal");

            AssertHashCodeNotEqual(a, b);
        }

        private static bool AreEqual(Expression a, Expression b)
        {
            return ExpressionEqualityComparer.Instance.Equals(a, b);
        }

        private static void AssertHashCodeEqual(Expression a, Expression b)
        {
            Assert.That(
                ExpressionEqualityComparer.Instance.GetHashCode(a) == ExpressionEqualityComparer.Instance.GetHashCode(b),
                Is.True,
                $"HashCode for '{a}' expected to be the same as for '{b}'");
        }

        private static void AssertHashCodeNotEqual(Expression a, Expression b)
        {
            Assert.That(
                ExpressionEqualityComparer.Instance.GetHashCode(a) == ExpressionEqualityComparer.Instance.GetHashCode(b),
                Is.False,
                $"HashCode for '{a}' expected to be the different as for '{b}'");
        }

        internal interface ITestClass
        {
            int Id { get; }

            int Pid { get; }
        }

        private class TestClass : ITestClass
        {
            public int Id { get; set; }

            public int Pid { get; }

            public override string ToString() => string.Empty;

            public string ToNormalizedString() => Id.ToString();
        }

        private class BaseClass : TestClass
        {
            public string Member { get; set; } = default!;
        }
    }
}