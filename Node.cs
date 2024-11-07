using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ViewportEngine.SceneManagement;

namespace ViewportEngine;

/// <summary>
/// The base class for all objects in the game scene.
/// </summary>
public class Node : IEnumerable<Node>
{
    public Scene Scene { get; private init; }
    public Node Parent { get; private set; }
    public Transform Transform { get; internal set; }
    public string Name { get; set; }
    public string SceneName => Parent != null ? Parent.SceneName + HIERARCHY_SEPARATOR + Name : Name;

    private readonly Dictionary<Guid, Node> _children;
    public int ChildCount => _children.Count;
    
    /// <summary>
    /// Globally unique node id.
    /// </summary>
    public Guid Id { get; private init; }

    private const string DEFAULT_NAME = "Node";
    private const char HIERARCHY_SEPARATOR = '.';
    
    /// <summary>
    /// <p>Constructor of Node should only be called during <see cref="Scene.Instantiate"/>.</p>
    /// <ul>
    /// <li>Override this to set up child node hierarchy and initialize fields.</li>
    /// <li>Code which is dependent on the node hierarchy should be executed in <see cref="Start"/>.</li>
    /// </ul>
    /// </summary>
    public Node(Scene scene)
    {
        this.Scene = scene;
        this.Name = DEFAULT_NAME;
        this.Id = Guid.NewGuid();
        _children = new Dictionary<Guid, Node>();
    }

    internal void AddChildReference(Node child)
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
        child.SetParent(null);
    }
    
    /// <summary>
    /// Assigns the parent of this <see cref="Node"/> to <paramref name="newParent"/>. 
    /// </summary>
    /// <param name="newParent"></param>
    public void SetParent(Node newParent)
    {
        Parent?.RemoveChild(this);
        Parent = newParent ?? Scene.Root;
    }

    public virtual void OnDestroyed()
    {
        Parent?.RemoveChild(this);
    }
    
    /// <summary>
    /// Creates a new instance of <see cref="Node"/> type <typeparamref name="T"/> within the scene this Node belongs to.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Instantiate<T>(string name, Node parent = null) where T : Node
    {
        var newNode = Scene.Instantiate<T>(parent);
        newNode.Name = name;
        return newNode;
    }

    public T AddChild<T>(string name) where T : Node => this.Instantiate<T>(name, this);

    /// <summary>
    /// Called when initialization is complete and the scene is starting.
    /// </summary>
    /// <param name="services"></param>
    public virtual void Start(GameServiceContainer services)
    {
        foreach (var child in this)
        {
            child.Start(services);
        }
    }
    
    /// <summary>
    /// Called when the GameObject is drawn.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="spriteBatch"></param>
    public virtual void Draw(GameServiceContainer services, SpriteBatch spriteBatch)
    {
        foreach (var child in this)
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
        foreach (var child in this)
        {
            child.Update(services, gameTime);
        }
    }

    public IEnumerator<Node> GetEnumerator()
    {
        return _children.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_children.Values).GetEnumerator();
    }

    public override string ToString()
    {
        return $"({GetType().ToString()}) {SceneName}";
    }

    /// <summary>
    /// Prints the tree structure of this Node and its children.
    /// </summary>
    public void PrintNode()
    {
        this.PrintNode(string.Empty, true);
    }
    
    private void PrintNode(string indent, bool isLast)
    {
        // Print the current node with the appropriate indent
        Console.Write(indent);
        Console.Write(isLast ? "└─ " : "├─ ");
        Console.WriteLine(ToString());

        // Increase the indent for children nodes
        indent += isLast ? "   " : "│  ";

        // Print each child node recursively
        var i = 0;
        foreach (var child in this)
        {
            child.PrintNode(indent, i == child.ChildCount - 1);
            i++;
        }
    }
}