using System.Numerics;
using System.Runtime.InteropServices;

namespace FluentCompare.Execution.FloatingPointValues;

internal static class FloatingPointHelpers
{
    internal static T ToEpsilon<T>(double epsilon)
        where T : IFloatingPoint<T>
        => T.CreateChecked(epsilon);

    internal static int MaxRoundingDigits<T>()
        where T : IFloatingPoint<T>
    {
        if (typeof(T) == typeof(float))
            return 6;

        if (typeof(T) == typeof(double))
            return 15;

        if (typeof(T) == typeof(Half))
            return 3;

        if (typeof(T) == typeof(NFloat))
        {
            // nfloat is platform-dependent:
            // 32-bit -> float (~6 digits), 64-bit -> double (~15 digits)
            return IntPtr.Size == 4 ? 6 : 15;
        }

        // TODO: Handle gracefully
        throw new NotSupportedException($"Type {typeof(T).Name} is not supported floating point value type");
    }
}

