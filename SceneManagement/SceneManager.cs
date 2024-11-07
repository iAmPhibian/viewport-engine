using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ViewportEngine.SceneManagement;

public class SceneManager(GameServiceContainer services, Scene defaultScene, int defaultWidth, int defaultHeight) : ISceneManager
{
    // Interface
    public ContentManager Content { get; private set; }
    public GraphicsDevice GraphicsDevice { get; set; }
    public Vector2 Dimensions { get; private set; } = new(defaultWidth, defaultHeight);
    public Scene CurrentScene { get; private set; }

    private GameServiceContainer Services { get; } = services;

    public void LoadContent(ContentManager content)
    {
        this.Content = new ContentManager(content.ServiceProvider, "Content");
        LoadScene(defaultScene);
    }

    public void UnloadContent()
    {
        CurrentScene.UnloadContent();
    }

    public void Update(GameTime gameTime)
    {
        CurrentScene.Update(gameTime);
    }

    public void Draw()
    {
        CurrentScene.Draw();
    }
    
    public void LoadScene(Scene scene)
    {
        CurrentScene?.UnloadContent();
        CurrentScene = scene;
        scene.LoadContent(Content);
        scene.Start();
    }
}