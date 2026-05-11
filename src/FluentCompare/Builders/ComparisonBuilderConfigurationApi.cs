public partial class ComparisonBuilder
{
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
    /// Allows arrays of different lengths to be compared without producing an error or mismatch.
    /// </summary>
    /// <returns></returns>
    public ComparisonBuilder AllowArrayComparisonOfDifferentLengths()
    {
        Configuration.AllowArrayComparisonOfDifferentLengths = true;
        return this;
    }

    /// <summary>
    /// Disallows arrays of different lengths to be compared. Comparing arrays of different lengths will result in a mismatch.
    /// </summary>
    /// <returns></returns>
    public ComparisonBuilder DisallowArrayComparisonOfDifferentLengths()
    {
        Configuration.AllowArrayComparisonOfDifferentLengths = false;
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
}
