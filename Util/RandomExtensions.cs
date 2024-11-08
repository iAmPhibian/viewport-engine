using System;

namespace ViewportEngine.Util;

public static class RandomExtensions
{
    /// <summary>
    /// Returns a random floating-point number that is greater than or equal to <paramref name="min"/>, and less than <paramref name="max"/>.
    /// </summary>
    /// <param name="random">The random object for seeded randomness</param>
    /// <param name="min">The minimum possible value (inclusive)</param>
    /// <param name="max">The maximum possible value (exclusive)</param>
    /// <returns>Random float between <paramref name="min"/> and <paramref name="max"/></returns>
    public static float NextSingle(this Random random, float min, float max)
    {
        return random.NextSingle() * (max - min) + min;
    }
    
    /// <summary>
    /// Returns a random floating-point number that is greater than or equal to <paramref name="min"/>, and less than <paramref name="max"/>.
    /// </summary>
    /// <param name="random">The random object for seeded randomness</param>
    /// <param name="min">The minimum possible value (inclusive)</param>
    /// <param name="max">The maximum possible value (exclusive)</param>
    /// <returns>Random double between <paramref name="min"/> and <paramref name="max"/></returns>
    public static double NextDouble(this Random random, double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }
}