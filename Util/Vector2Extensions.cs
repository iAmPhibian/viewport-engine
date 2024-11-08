using Microsoft.Xna.Framework;

namespace ViewportEngine.Util;

public static class Vector2Extensions
{
    /// <summary>
    /// Gets the directional vector of magnitude 1.0 that points from <paramref name="vector"/> to <paramref name="other"/>.
    /// </summary>
    /// <param name="vector">Source vector</param>
    /// <param name="other">Destination vector</param>
    /// <returns></returns>
    public static Vector2 DirectionVectorTo(this Vector2 vector, Vector2 other)
    {
        var diffVec = (other - vector);
        diffVec.Normalize();
        return diffVec;
    }
}