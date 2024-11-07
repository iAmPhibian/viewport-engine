using System;
using Microsoft.Xna.Framework;

namespace ViewportEngine.Util;

public class Matrix3x3
{
    public readonly float[,] Matrix = new float[3, 3];

    public Matrix3x3()
    {
        for (var i = 0; i < 3; i++)
            Matrix[i, i] = 1;
    }

    public static Matrix3x3 CreateTranslation(float tx, float ty)
    {
        var result = new Matrix3x3();
        result.Matrix[0, 2] = tx;
        result.Matrix[1, 2] = ty;
        return result;
    }

    public static Matrix3x3 CreateRotation(float angle)
    {
        var result = new Matrix3x3();
        var radians = angle * (float)Math.PI / 180f;
        result.Matrix[0, 0] = (float)Math.Cos(radians);
        result.Matrix[0, 1] = -(float)Math.Sin(radians);
        result.Matrix[1, 0] = (float)Math.Sin(radians);
        result.Matrix[1, 1] = (float)Math.Cos(radians);
        return result;
    }

    public static Matrix3x3 CreateScale(float sx, float sy)
    {
        var result = new Matrix3x3();
        result.Matrix[0, 0] = sx;
        result.Matrix[1, 1] = sy;
        return result;
    }

    public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
    {
        var result = new Matrix3x3();
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                result.Matrix[i, j] = a.Matrix[i, 0] * b.Matrix[0, j] +
                                      a.Matrix[i, 1] * b.Matrix[1, j] +
                                      a.Matrix[i, 2] * b.Matrix[2, j];
            }
        }
        return result;
    }

    public Vector2 Apply(Vector2 point)
    {
        var x = Matrix[0, 0] * point.X + Matrix[0, 1] * point.Y + Matrix[0, 2];
        var y = Matrix[1, 0] * point.X + Matrix[1, 1] * point.Y + Matrix[1, 2];
        return new Vector2(x, y);
    }
}