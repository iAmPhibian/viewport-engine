using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ViewportEngine.Util;

namespace ViewportEngine;

public abstract class VPEGame : Game, IWindowHandler
{
    protected GraphicsDeviceManager Graphics { get; private set; }
    protected SpriteBatch SpriteBatch { get; private set; }

    public int Width => Graphics.PreferredBackBufferWidth;
    public int Height => Graphics.PreferredBackBufferHeight;
    public Point Size => Window.ClientBounds.Size;
    public Point Position => Window.Position;

    protected VPEGame()
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        
        base.LoadContent();
    }

    protected SpriteBatch GetSpriteBatch()
    {
        return SpriteBatch;
    }

    public void SetWindowSize(int width, int height)
    {
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.ApplyChanges();
    }

    public void SetWindowPosition(int x, int y)
    {
        Window.Position = new Point(x, y);
    }
}