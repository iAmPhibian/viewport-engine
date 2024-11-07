using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ViewportEngine.Exceptions;

namespace ViewportEngine.SceneManagement;

public abstract class Scene
{
    public Node Root { get; }
    public GameServiceContainer Services { get; }
    protected ContentManager _content;
    
    private const string SCENE_DIRECTORY = "Content/Scenes";

    protected Scene(GameServiceContainer services)
    {
        Services = services;
        Root = new Node(services, this)
        {
            Name = "Root"
        };
        CreateTransformFor(Root);
    }

    /// <summary>
    /// Creates a new <see cref="Node"/> of type <typeparamref name="T"/> as a child of <paramref name="parent"/> in this scene.
    /// Note: <see cref="Transform"/> is not a scene-managed node, and as such will not have its own transform or be updated with the scene.
    /// </summary>
    /// <param name="parent">The parent of the new instantiated node</param>
    /// <typeparam name="T">The <see cref="Node"/> type of the instantiated node</typeparam>
    /// <returns></returns>
    public T Instantiate<T>(Node parent = null) where T : Node
    {
        parent ??= Root;

        var newNode = (T)Activator.CreateInstance(typeof(T), Services, parent.Scene);
        if (newNode == null)
        {
            throw new Exception($"Activator cannot create instance of type {typeof(T)}");
        }
        
        newNode.SetParent(parent);
        
        if (typeof(T) == typeof(Transform)) return newNode;
        
        // Non-transform nodes only:
        CreateTransformFor(newNode);
        parent.AddChildReference(newNode);
        
        return newNode;
    }

    private void CreateTransformFor(Node parent)
    {
        var transform = Instantiate<Transform>(parent);
        transform.Name = Transform.TRANSFORM_NAME;
        parent.Transform = transform;
    }

    public void Destroy(Node node)
    {
        if (node.GetType() == typeof(Transform))
        {
            throw new DestroyedTransformException();
        }

        node.OnDestroyed();
    }

    /// <summary>
    /// Loads the content within the scene.
    /// </summary>
    /// <param name="content"></param>
    public virtual void LoadContent(ContentManager content)
    {
        _content = new ContentManager(content.ServiceProvider, SCENE_DIRECTORY);
    }

    /// <summary>
    /// Unloads the content within the scene.
    /// </summary>
    public virtual void UnloadContent()
    {
        _content.Unload();
    }

    public virtual void Start()
    {
        Root.Start();
        Console.WriteLine(ToString());
    }

    /// <summary>
    /// Updates the scene every frame.
    /// </summary>
    /// <param name="gameTime"></param>
    public virtual void Update(GameTime gameTime)
    {
        // Update scene object loop
        Root.Update(gameTime);
    }

    /// <summary>
    /// Used for drawing content to the screen.
    /// </summary>
    public abstract void Draw();

    protected void RenderGameObjects()
    {
        Root.Draw();
    }
}