public class FloatComparisonConfiguration
{
    public int RoundingPrecision { get; set; } = 15; // Default to max precision for double
    public double EpsilonPrecision { get; set; } = 1e-17;
    public DoubleToleranceMethods ToleranceMethod { get; set; }
}
