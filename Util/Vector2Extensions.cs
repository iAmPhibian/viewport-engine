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
    
    
    /// <summary>
    /// Returns a <see cref="Direction4"/> direction given <paramref name="vecDirection"/>.
    /// </summary>
    /// <param name="vecDirection"></param>
    /// <returns></returns>
    public static Direction4 ToDirection4(this Vector2 vecDirection)
    {
        return vecDirection.X switch
        {
            > 0f => Direction4.Right,
            < 0f => Direction4.Left,
            _ => vecDirection.Y switch
            {
                < 0f => Direction4.Up,
                > 0f => Direction4.Down,
                _ => Direction4.Down
            }
        };
    }
}