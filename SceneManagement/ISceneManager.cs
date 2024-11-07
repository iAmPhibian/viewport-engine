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
    public void Draw();
    /// <summary>
    /// Updates this SceneManager.
    /// </summary>
    /// <param name="gameTime"></param>
    public void Update(GameTime gameTime);
    /// <summary>
    /// Loads this SceneManager's content.
    /// </summary>
    /// <param name="content"></param>
    public void LoadContent(ContentManager content);
    /// <summary>
    /// Unloads the currently loaded content in this SceneManager.
    /// </summary>
    public void UnloadContent();
    /// <summary>
    /// Unloads current scene and loads <paramref name="scene"/>.
    /// </summary>
    /// <param name="scene"></param>
    public void LoadScene(Scene scene);
}