using Microsoft.Xna.Framework;
using ViewportEngine.SceneManagement;
using ViewportEngine.Util;

namespace ViewportEngine;

/// <summary>
/// A basic class containing position, rotation, and scale data.
/// </summary>
public class Transform(GameServiceContainer services, Scene scene) : Node(services, scene)
{
    // Local
    public Vector2 LocalPosition = Vector2.Zero;
    public float LocalRotation = 0f;
    public Vector2 LocalScale = Vector2.One;
    
    // Global
    private Matrix3x3 LocalMatrix
    {
        get
        {
            var translationMatrix = Matrix3x3.CreateTranslation(LocalPosition.X, LocalPosition.Y);
            var rotationMatrix = Matrix3x3.CreateRotation(LocalRotation);
            var scaleMatrix = Matrix3x3.CreateScale(LocalScale.X, LocalScale.Y);
            return translationMatrix * rotationMatrix * scaleMatrix;
        }
    }

    private Matrix3x3 GlobalMatrix => Parent.Parent == null ? LocalMatrix : Parent.Parent.Transform.GlobalMatrix * LocalMatrix;

    public Vector2 GlobalPosition => GlobalMatrix.Apply(Vector2.Zero);
    public float GlobalRotation => Parent.Parent == null ? LocalRotation : Parent.Parent.Transform.GlobalRotation + LocalRotation;
    public Vector2 GlobalScale => Parent.Parent == null ? LocalScale : Parent.Parent.Transform.GlobalScale * LocalScale;

    public const string TRANSFORM_NAME = "Transform";
}