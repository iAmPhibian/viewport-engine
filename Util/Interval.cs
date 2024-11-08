using System;

namespace ViewportEngine.Util;

/// <summary>
/// Represents a range.
/// </summary>
/// <param name="min"></param>
/// <param name="max"></param>
public struct Interval(double min, double max)
{
    /// <summary>
    /// The low end of the range. (inclusive)
    /// </summary>
    public double Min = min;
    
    /// <summary>
    /// The high end of the range (exclusive)
    /// </summary>
    public double Max = max;

    /// <param name="random">Optional Random object used for generation</param>
    /// <returns>A random value between <see cref="Min"/> and <see cref="Max"/>, optionally using <paramref name="random"/> for seeding.</returns>
    public double Random(Random random = null)
    {
        random ??= new Random();
        return random.NextDouble(Min, Max);
    }
}