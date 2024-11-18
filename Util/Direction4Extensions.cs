using Microsoft.Xna.Framework;

namespace ViewportEngine.Util;

public static class Direction4Extensions
{
    public static Vector2 ToVector2(this Direction4 direction)
    {
        return direction switch
        {
            Direction4.Down => new Vector2(0, 1),
            Direction4.Up => new Vector2(0, -1),
            Direction4.Right => new Vector2(1, 0),
            Direction4.Left => new Vector2(-1, 0),
            _ => new Vector2(0, -1)
        };
    }
}