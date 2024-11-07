using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ViewportEngine.SceneManagement;

namespace ViewportEngine;

/// <summary>
/// The base class for all objects in the game scene.
/// </summary>
public class Node
{
    public Scene Scene { get; private init; }
    public Node Parent { get; private set; }
    public Transform Transform { get; internal set; }
    public string Name { get; set; }
    public string SceneName => Parent != null ? Parent.SceneName + HIERARCHY_SEPARATOR + Name : Name;

    private Dictionary<Guid, Node> _children;
    
    /// <summary>
    /// Globally unique node id.
    /// </summary>
    public Guid Id { get; private init; }
    
    
    /// <summary>
    /// Invoked when the object is destroyed.
    /// </summary>
    public event Action OnDestruct;

    private const string DEFAULT_NAME = "Node";
    private const char HIERARCHY_SEPARATOR = '.';
    
    /// <summary>
    /// Constructor of Node should only be used during Scene.Instantiate().
    /// </summary>
    public Node(Scene scene)
    {
        this.Scene = scene;
        this.Name = DEFAULT_NAME;
        this.Id = Guid.NewGuid();
        _children = new Dictionary<Guid, Node>();
    }

    internal void AddChild(Node child)
    {
        _children.Add(child.Id, child);
    }

    /// <summary>
    /// Removes <paramref name="child"/> from this Node's children.
    /// </summary>
    /// <param name="child"></param>
    public void RemoveChild(Node child)
    {
        _children.Remove(child.Id);
    }
    
    /// <summary>
    /// Assigns the parent of this <see cref="Node"/> to <paramref name="newParent"/>. 
    /// </summary>
    /// <param name="newParent"></param>
    public void SetParent(Node newParent)
    {
        Parent = newParent;
    }

    public void OnDestroyed()
    {
        OnDestruct?.Invoke();
    }
    
    /// <summary>
    /// Creates a new instance of <see cref="Node"/> type <typeparamref name="T"/> within the scene this Node belongs to.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Instantiate<T>(string name, Node parent = null) where T : Node => Scene?.Instantiate<T>(parent);

    public T AddChild<T>(string name) where T : Node => this.Instantiate<T>(name, this);

    /// <summary>
    /// Called when the GameObject is drawn.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="spriteBatch"></param>
    public virtual void Draw(GameServiceContainer services, SpriteBatch spriteBatch)
    {
        foreach (var child in _children.Values)
        {
            child.Draw(services, spriteBatch);
        }
    }

    /// <summary>
    /// Called for every <see cref="Node"/> on every frame.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="gameTime"></param>
    public virtual void Update(GameServiceContainer services, GameTime gameTime)
    {
        foreach (var child in _children.Values)
        {
            child.Update(services, gameTime);
        }
    }

    internal virtual void UpdateGlobalTransform()
    {
        if (Parent == null)
        {
            Transform.GlobalPosition = Transform.LocalPosition;
        }
        else
        {
            Transform.GlobalPosition = Parent.Transform.GlobalPosition + Transform.LocalPosition;
        }
        
        foreach (var child in _children.Values)
        {
            child.UpdateGlobalTransform();
        }
    }
}