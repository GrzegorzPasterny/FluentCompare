public class DoubleComparisonConfiguration
{
    public int Precision { get; set; } = 15; // Default to max precision for double

    // Another possible approach to be considered for the future:
    //private double _precision = double.Epsilon;
}
