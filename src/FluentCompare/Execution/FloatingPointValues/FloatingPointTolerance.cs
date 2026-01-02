using System.Numerics;

namespace FluentCompare.Execution.FloatingPointValues;

internal static class FloatingPointTolerance
{
    public static T ToEpsilon<T>(double epsilon)
        where T : IFloatingPoint<T>
        => T.CreateChecked(epsilon);
}

