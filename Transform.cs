using Microsoft.Xna.Framework;
using ViewportEngine.SceneManagement;

namespace ViewportEngine;

/// <summary>
/// A basic class containing position, rotation, and scale data.
/// </summary>
public class Transform(Scene scene) : Node(scene)
{
    // Local
    public Vector2 LocalPosition = Vector2.Zero;
    public float LocalRotation = 0f;
    public Vector2 LocalScale = Vector2.One;
    
    // Global
    public Vector2 GlobalPosition { get; private set; }
    public float GlobalRotation { get; private set; }
    public Vector2 GlobalScale { get; private set; }

    public const string TRANSFORM_NAME = "Transform";

    internal void UpdateGlobalTransform()
    {
        if (Parent.Parent == null)
        {
            GlobalPosition = LocalPosition;
            GlobalRotation = LocalRotation;
            GlobalScale = LocalScale;
        }
        else
        {
            GlobalPosition = Parent.Parent.Transform.GlobalPosition + LocalPosition;
            GlobalRotation = Parent.Parent.Transform.GlobalRotation + LocalRotation;
            GlobalScale = Parent.Parent.Transform.GlobalScale * LocalScale;
        }
        
        foreach (var child in Parent)
        {
            child.Transform.UpdateGlobalTransform();
        }
    }

    
    
    /// <summary>
    /// Overrides the automatically calculated <seealso cref="GlobalPosition"/> of this Transform.
    /// </summary>
    /// <param name="position"></param>
    public void SetGlobalPosition(Vector2 position)
    {
        this.GlobalPosition = position;
    }
    
    /// <summary>
    /// Overrides the automatically calculated <seealso cref="GlobalRotation"/> of this Transform.
    /// </summary>
    /// <param name="rotation"></param>
    public void SetGlobalRotation(float rotation)
    {
        this.GlobalRotation = rotation;
    }
    
    /// <summary>
    /// Overrides the automatically calculated <seealso cref="GlobalScale"/> of this Transform.
    /// </summary>
    /// <param name="scale"></param>
    public void SetGlobalScale(Vector2 scale)
    {
        this.GlobalScale = scale;
    }
}