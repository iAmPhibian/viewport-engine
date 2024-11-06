using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ViewportEngine;

public abstract class VPEGame : Game
{
    protected GraphicsDeviceManager Graphics { get; private set; }
    protected SpriteBatch SpriteBatch { get; private set; }
    
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
}