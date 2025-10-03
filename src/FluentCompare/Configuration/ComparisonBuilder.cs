using FluentCompare.Configuration;
using FluentCompare.Configuration.Models;

public class ComparisonBuilder
{
    private ComparisonConfiguration _configuration = new();

    /// <summary>
    /// Builds and executes the comparison for <paramref name="t"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ComparisonResult Compare<T>(params T[] t)
    {
        if (typeof(T) == typeof(object))
        {
            // TODO: Consider adding Build method to ComparisonBuilder
            // TODO: Consider creating ObjectsComparisonByReferenceEquality
            // and ObjectsComparisonByPropertyEquality classes
            return new ObjectsComparison(_configuration)
                .Compare(t);
        }
        if (typeof(T) == typeof(int))
        {
            return new IntComparison(_configuration)
                .Compare((int[])(object)t); // casting complexity O(1) according to chatGPT. TODO: To be confirmed
        }
        if (typeof(T) == typeof(int[]))
        {
            return new IntArrayComparison(_configuration)
                .Compare((int[][])(object)t); // casting complexity O(1) according to chatGPT. TODO: To be confirmed
        }

        throw new NotImplementedException();
    }

    /// <summary>
    /// Sets the <paramref name="comparisonType"/>. Default is <see cref="ComparisonType.EqualTo"/>.
    /// </summary>
    /// <param name="comparisonType"></param>
    /// <returns></returns>
    public ComparisonBuilder UseComparisonType(ComparisonType comparisonType)
    {
        _configuration.ComparisonType = comparisonType;
        return this;
    }

    /// <summary>
    /// Checks if objects are equivalent by comparing their properties. Default setting.
    /// </summary>
    /// <returns></returns>
    public ComparisonBuilder UsePropertyEquality()
    {
        _configuration.ComplexTypesComparisonMode = ComplexTypesComparisonMode.PropertyEquality;
        return this;
    }

    /// <summary>
    /// Checks if objects are equivalent by comparing their references.
    /// </summary>
    /// <returns></returns>
    public ComparisonBuilder UseReferenceEquality()
    {
        _configuration.ComplexTypesComparisonMode = ComplexTypesComparisonMode.ReferenceEquality;
        return this;
    }
}
