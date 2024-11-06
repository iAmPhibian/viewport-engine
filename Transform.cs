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
    public Vector2 GlobalPosition { get; internal set; }
    public float GlobalRotation { get; internal set; }
    public Vector2 GlobalScale { get; internal set; }

    public const string TRANSFORM_NAME = "Transform";

    internal override void UpdateGlobalTransform()
    {
        
    }
}