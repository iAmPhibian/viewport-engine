using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spritesheet;
using ViewportEngine.SceneManagement;

namespace ViewportEngine.Drawing;

/// <summary>
/// Displays a <see cref="Texture2D"/> at a <see cref="Transform"/>.
/// </summary>
public class Image(GameServiceContainer services, Scene scene) : Node(services, scene)
{
    public Color Color = Color.White;
    public float Alpha = 1.0f;
    
    private Texture2D _texture;
    private Rectangle? _sourceRect;
    private Vector2 _origin;
    private SpriteEffects _spriteEffects;

    public void SetImage(Texture2D texture, Rectangle? srcRect, SpriteEffects effects = SpriteEffects.None)
    {
        this._texture = texture;
        this._sourceRect = srcRect;
        this._spriteEffects = effects;
    }

    public void SetImage(Frame frame)
    {
        this.SetImage(frame.Texture, frame.Area, frame.Effects);
    }

    public override void Draw()
    {
        base.Draw();
        _origin = _sourceRect.HasValue ? 
            new Vector2(_sourceRect.Value.Width / 2f, _sourceRect.Value.Height / 2f) : 
            new Vector2(_texture.Width / 2f, _texture.Height / 2f);
        Services.GetService<SpriteBatch>().Draw(_texture, Transform.GlobalPosition, _sourceRect, Color * Alpha, Transform.GlobalRotation, _origin, Transform.GlobalScale, SpriteEffects.None, 0.0f);
    }
}