using System.Runtime.CompilerServices;

public class ComparisonBuilder
{
    public ComparisonConfiguration Configuration = new();

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
            return new ObjectComparison(Configuration)
                .Compare(t);
        }
        if (typeof(T) == typeof(int))
        {
            return new IntComparison(Configuration)
                .Compare((int[])(object)t); // casting complexity O(1) according to chatGPT. TODO: To be confirmed
        }
        if (typeof(T) == typeof(int[]))
        {
            return new IntArrayComparison(Configuration)
                .Compare((int[][])(object)t); // casting complexity O(1) according to chatGPT. TODO: To be confirmed
        }
        if (typeof(T) == typeof(double))
        {
            return new DoubleComparison(Configuration)
                .Compare((double[])(object)t); // casting complexity O(1) according to chatGPT. TODO: To be confirmed
        }
        if (typeof(T) == typeof(double[]))
        {
            return new DoubleArrayComparison(Configuration)
                .Compare((double[][])(object)t); // casting complexity O(1) according to chatGPT. TODO: To be confirmed
        }

        throw new NotImplementedException();
    }

    /// <summary>
    /// Builds and executes the comparison for <paramref name="t"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t1">First array for comparison</param>
    /// <param name="t2">Second array for comparison</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ComparisonResult Compare<T>(T t1, T t2,
        [CallerArgumentExpression(nameof(t1))] string? t1Expr = null,
        [CallerArgumentExpression(nameof(t2))] string? t2Expr = null)
    {
        if (t1 is null)
            throw new ArgumentNullException(nameof(t1));

        if (t2 is null)
            throw new ArgumentNullException(nameof(t2));

        if (typeof(T) == typeof(object))
        {
            // TODO: Consider adding Build method to ComparisonBuilder
            // TODO: Consider creating ObjectsComparisonByReferenceEquality
            // and ObjectsComparisonByPropertyEquality classes
            string t1ExprName = t1Expr ?? "ObjectOne";
            string t2ExprName = t2Expr ?? "ObjectTwo";

            return new ObjectComparison(Configuration)
                .Compare(t1, t2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(object[]))
        {
            object[] oArr1 = Unsafe.As<T, object[]>(ref t1);
            object[] oArr2 = Unsafe.As<T, object[]>(ref t2);
            string t1ExprName = t1Expr ?? "ArrayOne";
            string t2ExprName = t2Expr ?? "ArrayTwo";

            return new ObjectArrayComparison(Configuration)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(int))
        {
            int o1 = Unsafe.As<T, int>(ref t1);
            int o2 = Unsafe.As<T, int>(ref t2);
            string t1ExprName = t1Expr ?? "IntOne";
            string t2ExprName = t2Expr ?? "IntTwo";

            return new IntComparison(Configuration)
                .Compare(o1, o2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(int[]))
        {
            int[] oArr1 = Unsafe.As<T, int[]>(ref t1);
            int[] oArr2 = Unsafe.As<T, int[]>(ref t2);
            string t1ExprName = t1Expr ?? "IntArrayOne";
            string t2ExprName = t2Expr ?? "IntArrayTwo";

            return new IntArrayComparison(Configuration)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(double))
        {
            var o1 = Unsafe.As<T, double>(ref t1);
            var o2 = Unsafe.As<T, double>(ref t2);
            string t1ExprName = t1Expr ?? "DoubleOne";
            string t2ExprName = t2Expr ?? "DoubleTwo";

            return new DoubleComparison(Configuration)
                .Compare(o1, o2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(double[]))
        {
            var oArr1 = Unsafe.As<T, double[]>(ref t1);
            var oArr2 = Unsafe.As<T, double[]>(ref t2);
            string t1ExprName = t1Expr ?? "DoubleArrayOne";
            string t2ExprName = t2Expr ?? "DoubleArrayTwo";

            return new DoubleArrayComparison(Configuration)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }

        throw new NotImplementedException();
    }

    public ComparisonBuilder UseConfiguration(ComparisonConfiguration configuration)
    {
        Configuration = configuration;
        return this;
    }

    public DoubleComparisonBuilder ForDouble()
    {
        return new DoubleComparisonBuilder(Configuration);
    }

    public ComparisonBuilder Configure(Action<ComparisonConfiguration> configure)
    {
        configure(Configuration);
        return this;
    }

    // TODO: All of the configuration methods can be in a partial class
    /// <summary>
    /// Sets the <paramref name="comparisonType"/>. Default is <see cref="ComparisonType.EqualTo"/>.
    /// </summary>
    /// <param name="comparisonType"></param>
    /// <returns></returns>
    public ComparisonBuilder UseComparisonType(ComparisonType comparisonType)
    {
        Configuration.ComparisonType = comparisonType;
        return this;
    }

    /// <summary>
    /// Checks if objects are equivalent by comparing their properties. Default setting.
    /// </summary>
    /// <returns></returns>
    public ComparisonBuilder UsePropertyEquality()
    {
        Configuration.ComplexTypesComparisonMode = ComplexTypesComparisonMode.PropertyEquality;
        return this;
    }

    /// <summary>
    /// Checks if objects are equivalent by comparing their references.
    /// </summary>
    /// <returns></returns>
    public ComparisonBuilder UseReferenceEquality()
    {
        Configuration.ComplexTypesComparisonMode = ComplexTypesComparisonMode.ReferenceEquality;
        return this;
    }
}
