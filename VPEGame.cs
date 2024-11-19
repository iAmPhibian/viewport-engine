using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ViewportEngine.Util;

namespace ViewportEngine;

public abstract class VPEGame : Game, IWindowHandler
{
    protected GraphicsDeviceManager Graphics { get; private set; }
    protected SpriteBatch SpriteBatch { get; private set; }

    public int WindowWidth => Graphics.PreferredBackBufferWidth;
    public int WindowHeight => Graphics.PreferredBackBufferHeight;
    
    public abstract int PixelWidth { get; }
    public abstract int PixelHeight { get; }
    
    public int PixelScale { get; private set; }
    
    public Point Size => Window.ClientBounds.Size;
    public Point Position => Window.Position;
    public Point CenterPosition => Position + new Point(WindowWidth / 2, WindowHeight / 2);

    protected Rectangle WindowCenteredRect
    {
        get;
        private set;
    }

    protected VPEGame()
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        Window.ClientSizeChanged += OnResize;
        SpriteBatch = null;
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        
        base.LoadContent();
    }

    public void SetWindowSize(int width, int height)
    {
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.ApplyChanges();
        OnWindowSizeChanged(width, height);
    }
    
    private void OnResize(object obj, EventArgs args)
    {
        var newScreenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
        var newScreenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

        OnWindowSizeChanged(newScreenWidth, newScreenHeight);
    }
    
    private void OnWindowSizeChanged(int newWidth, int newHeight)
    {
        PixelScale = Math.Min(newWidth / PixelWidth, newHeight / PixelHeight);
        
        var frameWidth = PixelWidth * PixelScale;
        var frameHeight = PixelHeight * PixelScale;
        WindowCenteredRect = new Rectangle((newWidth - frameWidth) / 2, (newHeight - frameHeight) / 2, frameWidth, frameHeight);
    }

    protected static RenderTarget2D GetPixelPerfectRenderTarget(GraphicsDevice gd, int width, int height)
    {
        return new RenderTarget2D(gd, width, height, false,
            gd.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
    }

    public void SetWindowPosition(int x, int y)
    {
        Window.Position = new Point(x, y);
    }
}