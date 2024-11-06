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
    private readonly Node _rootNode;
    protected ContentManager _content;
    private readonly Dictionary<Guid, Node> _managedNodes = [];
    
    private const string SCENE_DIRECTORY = "Content/Scenes";

    protected Scene()
    {
        _rootNode = new Node(this)
        {
            Name = "Root"
        };
        CreateTransformFor(_rootNode);
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
        parent ??= _rootNode;

        var newNode = (T)Activator.CreateInstance(typeof(T), parent.Scene);
        if (newNode == null)
        {
            throw new Exception($"Activator cannot create instance of type {typeof(T)}");
        }
        
        newNode.SetParent(parent);
        if (typeof(T) != typeof(Transform))
        {
            CreateTransformFor(newNode);
            if (parent == _rootNode)
            {
                _managedNodes.Add(newNode.Id, newNode);
            }
        }
        parent.AddChild(newNode);
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
        _managedNodes.Remove(node.Id);
        node.OnDestroyed();
    }

    /// <summary>
    /// Loads the content within the scene.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="content"></param>
    public virtual void LoadContent(GameServiceContainer services, ContentManager content)
    {
        _content = new ContentManager(content.ServiceProvider, SCENE_DIRECTORY);
    }

    /// <summary>
    /// Unloads the content within the scene.
    /// </summary>
    /// <param name="services"></param>
    public virtual void UnloadContent(GameServiceContainer services)
    {
        _content.Unload();
    }

    /// <summary>
    /// Updates the scene every frame.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="gameTime"></param>
    public virtual void Update(GameServiceContainer services, GameTime gameTime)
    {
        // Update scene object loop
        _rootNode.UpdateGlobalTransform();
        foreach (var keyPair in _managedNodes)
        {
            keyPair.Value.Update(services, gameTime);
        }
    }

    /// <summary>
    /// Used for drawing content to the <paramref name="spriteBatch"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="spriteBatch"></param>
    public abstract void Draw(GameServiceContainer services, SpriteBatch spriteBatch);

    protected void RenderGameObjects(GameServiceContainer services, SpriteBatch spriteBatch)
    {
        foreach (var keyPair in _managedNodes)
        {
            keyPair.Value.Draw(services, spriteBatch);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append('[');
        var i = 0;
        foreach (var pair in _managedNodes)
        {
            sb.Append(pair.Value.SceneName);
            if (i < _managedNodes.Count - 1)
            {
                sb.Append(", ");
            }
            i++;
        }

        sb.Append(']');
        return sb.ToString();
    }
}