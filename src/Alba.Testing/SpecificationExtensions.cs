﻿using System.Collections;
using System.Diagnostics;
using JasperFx.Core;
using Shouldly;

namespace Alba.Testing
{
    public static class SpecificationExtensions
    {
        public static void ShouldHaveTheSameElementsAs(this IList actual, IList expected)
        {
            try
            {
                actual.ShouldNotBeNull();
                expected.ShouldNotBeNull();

                actual.Count.ShouldBe(expected.Count);

                for (int i = 0; i < actual.Count; i++)
                {
                    actual[i].ShouldBe(expected[i]);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Actual values were:");
                actual.Each(x => Debug.WriteLine(x));
                throw;
            }
        }

        public static void ShouldHaveTheSameElementsAs<T>(this IEnumerable<T> actual, params T[] expected)
        {
            ShouldHaveTheSameElementsAs(actual, (IEnumerable<T>)expected);
        }

        public static void ShouldHaveTheSameElementsAs<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            IList actualList = (actual is IList) ? (IList)actual : actual.ToList();
            IList expectedList = (expected is IList) ? (IList)expected : expected.ToList();

            ShouldHaveTheSameElementsAs(actualList, expectedList);
        }

        public static void ShouldHaveTheSameElementKeysAs<ELEMENT, KEY>(this IEnumerable<ELEMENT> actual,
                                                                        IEnumerable expected,
                                                                        Func<ELEMENT, KEY> keySelector)
        {
            actual.ShouldNotBeNull();
            expected.ShouldNotBeNull();

            ELEMENT[] actualArray = actual.ToArray();
            object[] expectedArray = expected.Cast<object>().ToArray();

            actualArray.Length.ShouldBe(expectedArray.Length);

            for (int i = 0; i < actual.Count(); i++)
            {
                keySelector(actualArray[i]).ShouldBe(expectedArray[i]);
            }
        }

    }

    public static class Exception<T> where T : Exception
    {
        public static T ShouldBeThrownBy(Action action)
        {
            T exception = null;

            try
            {
                action();
            }
            catch (Exception e)
            {
                exception = e.ShouldBeOfType<T>();
            }

            if (exception == null) throw new Exception("An exception was expected, but not thrown by the given action.");

            return exception;
        }

        public static async Task<T> ShouldBeThrownBy(Func<Task> action)
        {
            T exception = null;

            try
            {
                await action();
            }
            catch (Exception e)
            {
                exception = e.ShouldBeOfType<T>();
            }

            if (exception == null) throw new Exception("An exception was expected, but not thrown by the given action.");

            return exception;
        }
    }
}