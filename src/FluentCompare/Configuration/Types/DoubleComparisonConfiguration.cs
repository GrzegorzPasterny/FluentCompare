public class DoubleComparisonConfiguration
{
    public int RoundingPrecision { get; set; } = 15; // Default to max precision for double
    public double EpsilonPrecision { get; set; } = 1e-15;
    public DoubleToleranceMethods DoubleToleranceMethod { get; set; }

    // Another possible approach to be considered for the future:
    //private double _precision = double.Epsilon;
}
