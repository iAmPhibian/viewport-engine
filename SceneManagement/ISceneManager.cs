using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ViewportEngine.SceneManagement;

public interface ISceneManager
{
    public ContentManager Content { get; }
    public GraphicsDevice GraphicsDevice { set; get; }
    public Vector2 Dimensions { get; }
    public Scene CurrentScene { get; }

    /// <summary>
    /// Performs draw calls for this SceneManager.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="spriteBatch"></param>
    public void Draw(GameServiceContainer services, SpriteBatch spriteBatch);
    /// <summary>
    /// Updates this SceneManager.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="gameTime"></param>
    public void Update(GameServiceContainer services, GameTime gameTime);
    /// <summary>
    /// Loads this SceneManager's content.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="content"></param>
    public void LoadContent(GameServiceContainer services, ContentManager content);
    /// <summary>
    /// Unloads the currently loaded content in this SceneManager.
    /// </summary>
    /// <param name="services"></param>
    public void UnloadContent(GameServiceContainer services);
    /// <summary>
    /// Unloads current scene and loads <paramref name="scene"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="scene"></param>
    public void LoadScene(GameServiceContainer services, Scene scene);
}