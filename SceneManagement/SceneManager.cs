using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ViewportEngine.SceneManagement;

public class SceneManager(Scene defaultScene, int defaultWidth, int defaultHeight) : ISceneManager
{
    // Interface
    public ContentManager Content { get; private set; }
    public GraphicsDevice GraphicsDevice { get; set; }
    public Vector2 Dimensions { get; private set; } = new(defaultWidth, defaultHeight);
    public Scene CurrentScene { get; private set; }

    public void LoadContent(GameServiceContainer services, ContentManager content)
    {
        this.Content = new ContentManager(content.ServiceProvider, "Content");
        LoadScene(services, defaultScene);
    }

    public void UnloadContent(GameServiceContainer services)
    {
        CurrentScene.UnloadContent(services);
    }

    public void Update(GameServiceContainer services, GameTime gameTime)
    {
        CurrentScene.Update(services, gameTime);
    }

    public void Draw(GameServiceContainer services, SpriteBatch spriteBatch)
    {
        CurrentScene.Draw(services, spriteBatch);
    }
    
    public void LoadScene(GameServiceContainer services, Scene scene)
    {
        CurrentScene?.UnloadContent(services);
        CurrentScene = scene;
        scene.LoadContent(services, Content);
    }
}