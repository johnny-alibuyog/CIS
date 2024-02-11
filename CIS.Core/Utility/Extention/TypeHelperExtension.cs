using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace CIS.Core.Utility.Extention;

public static class TypeHelperExtension
{
    //public static void ForEach<T>(this IEnumerable<T> query, Action<T> method)
    //{
    //    foreach (T item in query)
    //    {
    //        method(item);
    //    }
    //}

    public static bool IsEnumerableOfT(this Type type)
    {
        if (type.IsGenericType)
        {
            return (object)type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        return false;
    }

    public static bool IsNullable(this Type type)
    {
        if (type.IsGenericType)
        {
            return (object)type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        return false;
    }

    public static bool IsNullableOrReference(this Type type)
    {
        if (type.IsValueType)
        {
            return type.IsNullable();
        }

        return true;
    }

    public static Type NullableOf(this Type type)
    {
        return type.GetGenericArguments()[0];
    }

    public static bool IsPrimitive(this Type type)
    {
        if (!type.IsValueType && !type.IsNullable())
        {
            return (object)type == typeof(string);
        }

        return true;
    }

    public static bool IsNonPrimitive(this Type type)
    {
        return !type.IsPrimitive();
    }

    public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int size)
    {
        if (size <= 0)
        {
            throw new ArgumentException("Chunk size must be greater than 0.", nameof(size));
        }

        using var enumerator = source.GetEnumerator();
        
        while (enumerator.MoveNext())
        {
            var currentChunk = new List<T>() { enumerator.Current };
            for (int i = 1; i < size && enumerator.MoveNext(); i++)
            {
                currentChunk.Add(enumerator.Current);
            }
            yield return currentChunk;
        }
    }

    public static DateTime ParseDate(this string value)
    {
        if (!DateTime.TryParse(value, out var result))
            return SqlDateTime.MinValue.Value;

        return result <= SqlDateTime.MinValue.Value
            ? SqlDateTime.MinValue.Value
            : result;
    }

    public static decimal ParseDecimal(this string value)
    {
        if (!decimal.TryParse(value, out var result))
            return 0;

        return result;
    }

    ////
    //// Summary:
    ////     Peform an unsafe cast to T (i.e. (T)source). This is only for syntactically more pleasing code.
    //public static T As<T>(this object source)
    //{
    //    return (T)source;
    //}

    internal static bool IsCollectionType(this Type type)
    {
        if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            return (object)type != typeof(string);
        }

        return false;
    }
}
