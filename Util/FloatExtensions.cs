using System;

namespace ViewportEngine.Util;

public static class FloatExtensions
{
    public static bool IsWithinEpsilon(this float value)
    {
        return Math.Abs(value) < float.Epsilon;
    }
}