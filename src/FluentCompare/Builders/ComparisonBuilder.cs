using System.Runtime.CompilerServices;

/// <summary>
/// Builder object to configure and perform comparisons.
/// </summary>
public class ComparisonBuilder : IComparisonBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ComparisonBuilder"/> class with default settings.
    /// </summary>
    internal ComparisonBuilder() { }

    /// <summary>
    /// Configuration object for the comparison.
    /// </summary>
    public ComparisonConfiguration Configuration { get; private set; } = new();

    /// <summary>
    /// Starting point to begin configuring and performing comparisons.
    /// </summary>
    /// <returns>A new <see cref="ComparisonBuilder"/> instance.</returns>
    public static ComparisonBuilder Create() => new ComparisonBuilder();

    /// <summary>
    /// Builds and executes the comparison for <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <param name="t">The objects to compare.</param>
    /// <returns>A <see cref="ComparisonResult"/> representing the outcome of the comparison.</returns>
    /// <exception cref="NotImplementedException">Thrown if the type is not supported.</exception>
    public ComparisonResult Compare<T>(params T[]? t)
    {
        if (t is null)
        {
            ComparisonResult result = new();
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(T)));
            return result;
        }
        if (typeof(T).Name == typeof(Nullable<>).Name)
        {
            ComparisonResult result = new();
            result.AddError(ComparisonErrors.NullPassedAsArgument(typeof(T)));
            return result;
        }

        if (typeof(T) == typeof(string))
            return new StringComparison(Configuration)
                .Compare((string[])(object)t);

        if (typeof(T) == typeof(string[]))
            return new StringComparison(Configuration)
                .Compare((string[][])(object)t);

        if (typeof(T) == typeof(bool))
            return new BoolComparison(Configuration)
                .Compare((bool[])(object)t);

        if (typeof(T) == typeof(bool[]))
            return new BoolComparison(Configuration)
                .Compare((bool[][])(object)t);

        if (typeof(T) == typeof(byte))
            return new ByteComparison(Configuration)
                .Compare((byte[])(object)t);

        if (typeof(T) == typeof(byte[]))
            return new ByteComparison(Configuration)
                .Compare((byte[][])(object)t);

        if (typeof(T) == typeof(short))
            return new NumericComparison<short>(Configuration)
                .Compare((short[])(object)t);

        if (typeof(T) == typeof(short[]))
            return new NumericComparison<short>(Configuration)
                .Compare((short[][])(object)t);

        if (typeof(T) == typeof(int))
            return new NumericComparison<int>(Configuration)
                .Compare((int[])(object)t);

        if (typeof(T) == typeof(int[]))
            return new NumericComparison<int>(Configuration)
                .Compare((int[][])(object)t);

        if (typeof(T) == typeof(long))
            return new NumericComparison<long>(Configuration)
                .Compare((long[])(object)t);

        if (typeof(T) == typeof(long[]))
            return new NumericComparison<long>(Configuration)
                .Compare((long[][])(object)t);

        if (typeof(T) == typeof(float))
            return new FloatingPointComparison<float>(Configuration)
                .Compare((float[])(object)t);

        if (typeof(T) == typeof(float[]))
            return new FloatingPointComparison<float>(Configuration)
                .Compare((float[][])(object)t);

        if (typeof(T) == typeof(double))
            return new FloatingPointComparison<double>(Configuration)
                .Compare((double[])(object)t);

        if (typeof(T) == typeof(double[]))
            return new FloatingPointComparison<double>(Configuration)
                .Compare((double[][])(object)t);

        if (typeof(T) == typeof(object[]))
            return new ObjectComparison(Configuration)
                .Compare((object[][])(object)t);

        if (IsPrimitiveOrEnum(typeof(T)))
            throw new NotImplementedException(typeof(T).Name);

        return new ObjectComparison(Configuration)
            .Compare((object[])(object)t);
    }

    /// <summary>
    /// Builds and executes the comparison between <paramref name="t1"/> and <paramref name="t2"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <param name="t1">First object for comparison.</param>
    /// <param name="t2">Second object for comparison.</param>
    /// <param name="t1Expr">Caller argument expression for t1.</param>
    /// <param name="t2Expr">Caller argument expression for t2.</param>
    /// <returns>A <see cref="ComparisonResult"/> representing the outcome of the comparison.</returns>
    /// <exception cref="NotImplementedException">Thrown if the type is not supported.</exception>
    public ComparisonResult Compare<T>(T t1, T t2,
        [CallerArgumentExpression(nameof(t1))] string? t1Expr = null,
        [CallerArgumentExpression(nameof(t2))] string? t2Expr = null)
    {
        ComparisonResult comparisonResult = HandleNullability(t1, t2, t1Expr, t2Expr);

        // Check basic nullability results first
        if (comparisonResult.WasSuccessful == false)
        {
            return comparisonResult;
        }
        if (comparisonResult.AllMatched == false &&
            comparisonResult.Mismatches.First().Code == ComparisonMismatches.NullPassedAsArgumentCode)
        {
            return comparisonResult;
        }

        if (typeof(T) == typeof(string))
        {
            string s1 = Unsafe.As<T, string>(ref t1);
            string s2 = Unsafe.As<T, string>(ref t2);
            string t1ExprName = t1Expr ?? "StringOne";
            string t2ExprName = t2Expr ?? "StringTwo";

            return new StringComparison(Configuration, comparisonResult)
                .Compare(s1, s2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(string[]))
        {
            string[] sArr1 = Unsafe.As<T, string[]>(ref t1);
            string[] sArr2 = Unsafe.As<T, string[]>(ref t2);
            string t1ExprName = t1Expr ?? "StringArrayOne";
            string t2ExprName = t2Expr ?? "StringArrayTwo";

            return new StringComparison(Configuration, comparisonResult)
                .Compare(sArr1, sArr2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(bool))
        {
            bool s1 = Unsafe.As<T, bool>(ref t1);
            bool s2 = Unsafe.As<T, bool>(ref t2);
            string t1ExprName = t1Expr ?? "BoolOne";
            string t2ExprName = t2Expr ?? "BoolTwo";

            return new BoolComparison(Configuration, comparisonResult)
                .Compare(s1, s2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(bool[]))
        {
            bool[] s1 = Unsafe.As<T, bool[]>(ref t1);
            bool[] s2 = Unsafe.As<T, bool[]>(ref t2);
            string t1ExprName = t1Expr ?? "BoolArrayOne";
            string t2ExprName = t2Expr ?? "BoolArrayTwo";

            return new BoolComparison(Configuration, comparisonResult)
                .Compare(s1, s2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(byte))
        {
            byte s1 = Unsafe.As<T, byte>(ref t1);
            byte s2 = Unsafe.As<T, byte>(ref t2);
            string t1ExprName = t1Expr ?? "ByteOne";
            string t2ExprName = t2Expr ?? "ByteTwo";

            return new ByteComparison(Configuration, comparisonResult)
                .Compare(s1, s2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(byte[]))
        {
            byte[] s1 = Unsafe.As<T, byte[]>(ref t1);
            byte[] s2 = Unsafe.As<T, byte[]>(ref t2);
            string t1ExprName = t1Expr ?? "ByteArrayOne";
            string t2ExprName = t2Expr ?? "ByteArrayTwo";

            return new ByteComparison(Configuration, comparisonResult)
                .Compare(s1, s2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(short))
        {
            short o1 = Unsafe.As<T, short>(ref t1);
            short o2 = Unsafe.As<T, short>(ref t2);
            string t1ExprName = t1Expr ?? "ShortOne";
            string t2ExprName = t2Expr ?? "ShortTwo";

            return new NumericComparison<short>(Configuration, comparisonResult)
                .Compare(o1, o2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(short[]))
        {
            short[] oArr1 = Unsafe.As<T, short[]>(ref t1);
            short[] oArr2 = Unsafe.As<T, short[]>(ref t2);
            string t1ExprName = t1Expr ?? "ShortArrayOne";
            string t2ExprName = t2Expr ?? "ShortArrayTwo";

            return new NumericComparison<short>(Configuration, comparisonResult)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(int))
        {
            int o1 = Unsafe.As<T, int>(ref t1);
            int o2 = Unsafe.As<T, int>(ref t2);
            string t1ExprName = t1Expr ?? "IntOne";
            string t2ExprName = t2Expr ?? "IntTwo";

            return new NumericComparison<int>(Configuration, comparisonResult)
                .Compare(o1, o2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(int[]))
        {
            int[] oArr1 = Unsafe.As<T, int[]>(ref t1);
            int[] oArr2 = Unsafe.As<T, int[]>(ref t2);
            string t1ExprName = t1Expr ?? "IntArrayOne";
            string t2ExprName = t2Expr ?? "IntArrayTwo";

            return new NumericComparison<int>(Configuration, comparisonResult)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(long))
        {
            long o1 = Unsafe.As<T, long>(ref t1);
            long o2 = Unsafe.As<T, long>(ref t2);
            string t1ExprName = t1Expr ?? "LongOne";
            string t2ExprName = t2Expr ?? "LongTwo";

            return new NumericComparison<long>(Configuration, comparisonResult)
                .Compare(o1, o2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(long[]))
        {
            long[] oArr1 = Unsafe.As<T, long[]>(ref t1);
            long[] oArr2 = Unsafe.As<T, long[]>(ref t2);
            string t1ExprName = t1Expr ?? "LongArrayOne";
            string t2ExprName = t2Expr ?? "LongArrayTwo";

            return new NumericComparison<long>(Configuration, comparisonResult)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(float))
        {
            var o1 = Unsafe.As<T, float>(ref t1);
            var o2 = Unsafe.As<T, float>(ref t2);
            string t1ExprName = t1Expr ?? "FloatOne";
            string t2ExprName = t2Expr ?? "FloatTwo";

            return new FloatingPointComparison<float>(Configuration, comparisonResult)
                .Compare(o1, o2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(float[]))
        {
            var oArr1 = Unsafe.As<T, float[]>(ref t1);
            var oArr2 = Unsafe.As<T, float[]>(ref t2);
            string t1ExprName = t1Expr ?? "FloatArrayOne";
            string t2ExprName = t2Expr ?? "FloatArrayTwo";

            return new FloatingPointComparison<float>(Configuration, comparisonResult)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(double))
        {
            var o1 = Unsafe.As<T, double>(ref t1);
            var o2 = Unsafe.As<T, double>(ref t2);
            string t1ExprName = t1Expr ?? "DoubleOne";
            string t2ExprName = t2Expr ?? "DoubleTwo";

            return new FloatingPointComparison<double>(Configuration, comparisonResult)
                .Compare(o1, o2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(double[]))
        {
            var oArr1 = Unsafe.As<T, double[]>(ref t1);
            var oArr2 = Unsafe.As<T, double[]>(ref t2);
            string t1ExprName = t1Expr ?? "DoubleArrayOne";
            string t2ExprName = t2Expr ?? "DoubleArrayTwo";

            return new FloatingPointComparison<double>(Configuration, comparisonResult)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }
        if (typeof(T) == typeof(object[]))
        {
            object[] oArr1 = Unsafe.As<T, object[]>(ref t1);
            object[] oArr2 = Unsafe.As<T, object[]>(ref t2);
            string t1ExprName = t1Expr ?? "ArrayOne";
            string t2ExprName = t2Expr ?? "ArrayTwo";

            return new ObjectComparison(Configuration, comparisonResult)
                .Compare(oArr1, oArr2, t1ExprName, t2ExprName);
        }
        if (IsPrimitiveOrEnum(typeof(T)))
        {
            throw new NotImplementedException(typeof(T).Name);
        }
        else
        {
            string t1ExprName = t1Expr ?? "ObjectOne";
            string t2ExprName = t2Expr ?? "ObjectTwo";

            return new ObjectComparison(Configuration, comparisonResult)
                .Compare(t1!, t2!, t1ExprName, t2ExprName);
        }
    }

    /// <summary>
    /// Builds and executes the comparison between two objects.
    /// </summary>
    /// <param name="o1">First object for comparison.</param>
    /// <param name="o2">Second object for comparison.</param>
    /// <param name="o1Expr">Caller argument expression for o1.</param>
    /// <param name="o2Expr">Caller argument expression for o2.</param>
    /// <returns>A <see cref="ComparisonResult"/> representing the outcome of the comparison.</returns>
    public ComparisonResult Compare(object? o1, object? o2,
        [CallerArgumentExpression(nameof(o1))] string? o1Expr = null,
        [CallerArgumentExpression(nameof(o2))] string? o2Expr = null)
    {
        ComparisonResult comparisonResult = HandleNullability(o1, o2, o1Expr, o2Expr);

        // Check basic nullability results first
        if (comparisonResult.WasSuccessful == false || comparisonResult.AllMatched == false)
        {
            return comparisonResult;
        }

        Type type = o1!.GetType();
        string t1ExprName = o1Expr ?? "ObjectOne";
        string t2ExprName = o2Expr ?? "ObjectTwo";

        // Primitives
        if (type == typeof(string))
            return new StringComparison(Configuration, comparisonResult).Compare((string)o1, (string)o2!, t1ExprName, t2ExprName);

        if (type == typeof(bool))
            return new BoolComparison(Configuration, comparisonResult).Compare((bool)o1, (bool)o2!, t1ExprName, t2ExprName);

        if (type == typeof(byte))
            return new ByteComparison(Configuration, comparisonResult).Compare((byte)o1, (byte)o2!, t1ExprName, t2ExprName);

        if (type == typeof(short))
            return new NumericComparison<short>(Configuration, comparisonResult).Compare((short)o1, (short)o2!, t1ExprName, t2ExprName);

        if (type == typeof(int))
            return new NumericComparison<int>(Configuration, comparisonResult).Compare((int)o1, (int)o2!, t1ExprName, t2ExprName);

        if (type == typeof(long))
            return new NumericComparison<long>(Configuration, comparisonResult).Compare((long)o1, (long)o2!, t1ExprName, t2ExprName);

        if (type == typeof(float))
            return new FloatingPointComparison<float>(Configuration, comparisonResult).Compare((float)o1, (float)o2!, t1ExprName, t2ExprName);

        if (type == typeof(double))
            return new FloatingPointComparison<double>(Configuration, comparisonResult).Compare((double)o1, (double)o2!, t1ExprName, t2ExprName);

        // Array types
        if (type == typeof(string[]))
            return new StringComparison(Configuration, comparisonResult).Compare((string[])o1, (string[])o2!, t1ExprName, t2ExprName);

        if (type == typeof(bool[]))
            return new BoolComparison(Configuration, comparisonResult).Compare((bool[])o1, (bool[])o2!, t1ExprName, t2ExprName);

        if (type == typeof(byte[]))
            return new ByteComparison(Configuration, comparisonResult).Compare((byte[])o1, (byte[])o2!, t1ExprName, t2ExprName);

        if (type == typeof(short[]))
            return new NumericComparison<short>(Configuration, comparisonResult).Compare((short[])o1, (short[])o2!, t1ExprName, t2ExprName);

        if (type == typeof(int[]))
            return new NumericComparison<int>(Configuration, comparisonResult).Compare((int[])o1, (int[])o2!, t1ExprName, t2ExprName);

        if (type == typeof(long[]))
            return new NumericComparison<long>(Configuration, comparisonResult).Compare((long[])o1, (long[])o2!, t1ExprName, t2ExprName);

        if (type == typeof(float[]))
            return new FloatingPointComparison<float>(Configuration, comparisonResult).Compare((float[])o1, (float[])o2!, t1ExprName, t2ExprName);

        if (type == typeof(double[]))
            return new FloatingPointComparison<double>(Configuration, comparisonResult).Compare((double[])o1, (double[])o2!, t1ExprName, t2ExprName);


        if (type == typeof(object[]))
            return new ObjectComparison(Configuration, comparisonResult)
                .Compare((object[])o1, (object[])o2!, t1ExprName, t2ExprName);


        if (IsPrimitiveOrEnum(type))
            throw new NotImplementedException(type.Name);


        return new ObjectComparison(Configuration, comparisonResult)
            .Compare(o1, o2, t1ExprName, t2ExprName);
    }

    /// <summary>
    /// Builds and executes the comparison between two object arrays.
    /// </summary>
    /// <param name="o1Arr">First object array for comparison.</param>
    /// <param name="o2Arr">Second object array for comparison.</param>
    /// <param name="o1ArrExpr">Caller argument expression for o1Arr.</param>
    /// <param name="o2ArrExpr">Caller argument expression for o2Arr.</param>
    /// <returns>A <see cref="ComparisonResult"/> representing the outcome of the comparison.</returns>
    public ComparisonResult Compare(object[] o1Arr, object[] o2Arr,
        [CallerArgumentExpression(nameof(o1Arr))] string? o1ArrExpr = null,
        [CallerArgumentExpression(nameof(o2Arr))] string? o2ArrExpr = null)
    {
        ComparisonResult comparisonResult = HandleNullability(o1Arr, o2Arr, o1ArrExpr, o2ArrExpr);

        // Check basic nullability results first
        if (comparisonResult.WasSuccessful == false)
        {
            return comparisonResult;
        }
        if (comparisonResult.AllMatched == false &&
            comparisonResult.Mismatches.First().Code == ComparisonMismatches.NullPassedAsArgumentCode)
        {
            return comparisonResult;
        }

        string t1ExprName = o1ArrExpr ?? "ObjectArrayOne";
        string t2ExprName = o2ArrExpr ?? "ObjectArrayTwo";

        return new ObjectComparison(Configuration, comparisonResult)
            .Compare(o1Arr, o2Arr, t1ExprName, t2ExprName);
    }

    /// <summary>
    /// Sets the <paramref name="configuration"/> as a comparison configuration object.
    /// </summary>
    /// <param name="configuration">The configuration to use.</param>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder UseConfiguration(ComparisonConfiguration configuration)
    {
        Configuration = configuration;
        return this;
    }

    /// <summary>
    /// Configures the comparison using the provided <paramref name="configure"/> action.
    /// </summary>
    /// <param name="configure">The action to configure the comparison.</param>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder Configure(Action<ComparisonConfiguration> configure)
    {
        configure(Configuration);
        return this;
    }

    /// <summary>
    /// Sets the <paramref name="comparisonType"/>. Default is <see cref="ComparisonType.EqualTo"/>.
    /// </summary>
    /// <param name="comparisonType">The comparison type to use.</param>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder UseComparisonType(ComparisonType comparisonType)
    {
        Configuration.ComparisonType = comparisonType;
        return this;
    }

    /// <summary>
    /// Checks if objects are equivalent by comparing their properties. Default setting.
    /// </summary>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder UsePropertyEquality()
    {
        Configuration.ComplexTypesComparisonMode = ComplexTypesComparisonMode.PropertyEquality;
        return this;
    }

    /// <summary>
    /// Checks if objects are equivalent by comparing their references.
    /// </summary>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder UseReferenceEquality()
    {
        Configuration.ComplexTypesComparisonMode = ComplexTypesComparisonMode.ReferenceEquality;
        return this;
    }

    /// <summary>
    /// Sets the comparison mode for complex types (such as classes and structs).
    /// </summary>
    /// <param name="complexTypesComparisonMode">The mode to use when comparing complex types.</param>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder UseComplexTypeComparisonMode(ComplexTypesComparisonMode complexTypesComparisonMode)
    {
        Configuration.ComplexTypesComparisonMode = complexTypesComparisonMode;
        return this;
    }

    /// <summary>
    /// Sets the string comparison type. Default is <see cref="System.StringComparison.Ordinal"/>.
    /// </summary>
    /// <param name="stringComparison">The string comparison type to use.</param>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder UseStringComparisonType(System.StringComparison stringComparison)
    {
        Configuration.StringConfiguration.StringComparisonType = stringComparison;
        return this;
    }

    /// <summary>
    /// Sets the maximum depth for recursive comparison of complex types. Default is 5.
    /// </summary>
    /// <param name="depth">New comparison depth.</param>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder SetComparisonDepth(int depth)
    {
        Configuration.MaximumComparisonDepth = depth;
        return this;
    }

    /// <summary>
    /// Allows comparing nulls. If both values are null, they are considered equal. Default behavior.
    /// </summary>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder AllowNullComparison()
    {
        Configuration.AllowNullComparison = true;
        return this;
    }

    /// <summary>
    /// Disallows comparing nulls. Comparing nulls will result in a mismatch.
    /// </summary>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder DisallowNullComparison()
    {
        Configuration.AllowNullComparison = false;
        return this;
    }

    /// <summary>
    /// Allows nulls in the arguments to be compared. Default behavior.
    /// </summary>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder AllowNullsInArguments()
    {
        Configuration.AllowNullsInArguments = true;
        return this;
    }

    /// <summary>
    /// Disallows nulls in the arguments to be compared. Passing nulls will result in an error.
    /// </summary>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder DisallowNullsInArguments()
    {
        Configuration.AllowNullsInArguments = false;
        return this;
    }

    /// <summary>
    /// Configures the comparison to finish on the first mismatch found.
    /// </summary>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder FinishComparisonOnFirstMismatch()
    {
        Configuration.FinishComparisonOnFirstMismatch = true;
        return this;
    }

    /// <summary>
    /// Configures the comparison to collect all mismatches before finishing.
    /// </summary>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder FinishComparisonCollectingAllMismatches()
    {
        Configuration.FinishComparisonOnFirstMismatch = false;
        return this;
    }

    /// <summary>
    /// Applies a bitwise operation to byte comparisons.
    /// </summary>
    /// <param name="bitwiseOperation">The bitwise operation to apply.</param>
    /// <param name="mask">The mask value for the operation.</param>
    /// <param name="comparisonObjectIndexesToExclude">Indexes to exclude from the operation.</param>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder ApplyBitwiseOperation(BitwiseOperation bitwiseOperation, byte mask, params int[] comparisonObjectIndexesToExclude)
    {
        Configuration.ByteConfiguration.BitwiseOperations.Add(new BitwiseOperationModel
        {
            Operation = bitwiseOperation,
            Value = mask,
            ComparisonObjectIndexesToExclude = comparisonObjectIndexesToExclude.ToList()
        });
        return this;
    }

    /// <summary>
    /// Applies a bitwise operation to byte comparisons.
    /// </summary>
    /// <param name="bitwiseOperation">The bitwise operation to apply.</param>
    /// <param name="mask">The mask value for the operation.</param>
    /// <param name="comparisonObjectIndexToExclude">Index to exclude from the operation.</param>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder ApplyBitwiseOperation(BitwiseOperation bitwiseOperation, byte mask, int comparisonObjectIndexToExclude)
    {
        Configuration.ByteConfiguration.BitwiseOperations.Add(new BitwiseOperationModel
        {
            Operation = bitwiseOperation,
            Value = mask,
            ComparisonObjectIndexesToExclude = new() { comparisonObjectIndexToExclude }
        });
        return this;
    }

    /// <summary>
    /// Applies a bitwise operation to byte comparisons.
    /// </summary>
    /// <param name="bitwiseOperationModel">The bitwise operation model to apply.</param>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder ApplyBitwiseOperation(BitwiseOperationModel bitwiseOperationModel)
    {
        Configuration.ByteConfiguration.BitwiseOperations.Add(bitwiseOperationModel);
        return this;
    }

    /// <summary>
    /// Sets the double comparison to use rounding precision.
    /// </summary>
    /// <param name="roundingPrecision">The number of decimal places to round to.</param>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder WithDoublePrecision(int roundingPrecision)
    {
        Configuration.FloatConfiguration.RoundingPrecision = roundingPrecision;
        Configuration.FloatConfiguration.ToleranceMethod = DoubleToleranceMethods.Rounding;
        return this;
    }

    /// <summary>
    /// Sets the double comparison to use epsilon precision.
    /// </summary>
    /// <param name="epsilonPrecision">The epsilon value for comparison.</param>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder WithDoublePrecision(double epsilonPrecision)
    {
        Configuration.FloatConfiguration.EpsilonPrecision = epsilonPrecision;
        Configuration.FloatConfiguration.ToleranceMethod = DoubleToleranceMethods.Epsilon;
        return this;
    }

    /// <summary>
    /// Sets the double tolerance method for double comparisons.
    /// </summary>
    /// <param name="doubleToleranceMethod">The tolerance method to use.</param>
    /// <returns>The current <see cref="ComparisonBuilder"/> instance.</returns>
    public ComparisonBuilder UseDoubleToleranceMethod(DoubleToleranceMethods doubleToleranceMethod)
    {
        Configuration.FloatConfiguration.ToleranceMethod = doubleToleranceMethod;
        return this;
    }

    /// <summary>
    /// Handles nullability checks for object comparisons.
    /// </summary>
    /// <param name="o1">First object.</param>
    /// <param name="o2">Second object.</param>
    /// <param name="t1Expr">Caller argument expression for o1.</param>
    /// <param name="t2Expr">Caller argument expression for o2.</param>
    /// <returns>A <see cref="ComparisonResult"/> indicating nullability issues.</returns>
    private ComparisonResult HandleNullability(object? o1, object? o2, string? t1Expr, string? t2Expr)
    {
        var result = new ComparisonResult();

        if (o1 is null || o2 is null)
        {
            if (o1 is null && o2 is null)
            {
                if (Configuration.AllowNullComparison)
                {
                    result.AddWarning(ComparisonErrors.BothObjectsAreNull());
                    return result;
                }
                else
                {
                    result.AddError(ComparisonErrors.BothObjectsAreNull());
                    return result;
                }
            }

            if (o1 is null)
            {
                string t1ExprName = t1Expr ?? $"{o2!.GetType().Name}Two";

                if (Configuration.AllowNullComparison)
                {
                    result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(1, o2!.GetType()));
                    return result;
                }
                else
                {
                    result.AddError(ComparisonMismatches.NullPassedAsArgument(t1ExprName, o2!.GetType()));
                    return result;
                }
            }

            if (o2 is null)
            {
                string t2ExprName = t2Expr ?? $"{o1.GetType().Name}One";
                if (Configuration.AllowNullComparison)
                {
                    result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(2, o1!.GetType()));
                    return result;
                }
                else
                {
                    result.AddError(ComparisonMismatches.NullPassedAsArgument(t2ExprName, o1!.GetType()));
                    return result;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Handles nullability checks for generic type comparisons.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <param name="t1">First object.</param>
    /// <param name="t2">Second object.</param>
    /// <param name="t1Expr">Caller argument expression for t1.</param>
    /// <param name="t2Expr">Caller argument expression for t2.</param>
    /// <returns>A <see cref="ComparisonResult"/> indicating nullability issues.</returns>
    private ComparisonResult HandleNullability<T>(T t1, T t2, string? t1Expr, string? t2Expr)
    {
        ComparisonResult result = new();

        if (t1 is null || t2 is null)
        {
            if (t1 is null && t2 is null)
            {
                if (Configuration.AllowNullsInArguments && Configuration.AllowNullComparison)
                {
                    result.AddWarning(ComparisonErrors.BothObjectsAreNull());
                    return result;
                }

                result.AddError(ComparisonErrors.BothObjectsAreNull());
                return result;
            }

            string t1ExprName = t1Expr ?? $"{typeof(T).Name}One";
            string t2ExprName = t1Expr ?? $"{typeof(T).Name}Two";

            if (t1 is null || t2 is null)
            {
                if (Configuration.AllowNullsInArguments == false)
                {
                    result.AddError(ComparisonErrors.NullPassedAsArgument(t1ExprName, typeof(T)));
                    return result;
                }

                if (Configuration.AllowNullComparison == false)
                {
                    result.AddError(ComparisonErrors.OneOfTheObjectsIsNull(t1ExprName, t2ExprName));
                }

                result.AddMismatch(ComparisonMismatches.NullPassedAsArgument(t1ExprName, t2ExprName, typeof(T)));
                return result;
            }
        }

        return result;
    }

    /// <summary>
    /// Determines if the specified type is a primitive or enum.
    /// </summary>
    /// <param name="type1">The type to check.</param>
    /// <returns><c>true</c> if the type is primitive or enum; otherwise, <c>false</c>.</returns>
    private static bool IsPrimitiveOrEnum(Type type1) => type1.IsPrimitive || type1.IsEnum;
}
