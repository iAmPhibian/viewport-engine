using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ViewportEngine.SceneManagement;

namespace ViewportEngine;

/// <summary>
/// The base class for all objects in the game scene.
/// </summary>
public class Node : IEnumerable<Node>
{
    /// <summary>
    /// The Scene that this Node belongs to and resides within.
    /// </summary>
    public Scene Scene { get; private init; }
    /// <summary>
    /// The parent of this Node in the <see cref="Scene"/>.
    /// </summary>
    public Node Parent { get; private set; }
    /// <summary>
    /// The required Transform attached to this Node.
    /// </summary>
    public Transform Transform { get; internal set; }
    /// <summary>
    /// Globally unique node id.
    /// </summary>
    public Guid Id { get; }
    /// <summary>
    /// The human-readable name of the Node.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// The name of the Node concatenated with all of its parents.
    /// </summary>
    public string SceneName => Parent != null ? Parent.SceneName + HIERARCHY_SEPARATOR + Name : Name;
    /// <summary>
    /// A reference to the Game's <see cref="GameServiceContainer"/>.
    /// </summary>
    protected GameServiceContainer Services { get; private set; }

    private readonly Dictionary<Guid, Node> _children;
    public int ChildCount => _children.Count;
    
    private const string DEFAULT_NAME = "Node";
    private const char HIERARCHY_SEPARATOR = '.';
    
    /// <summary>
    /// <p>Constructor of Node should only be called during <see cref="Scene.Instantiate"/>.</p>
    /// <ul>
    /// <li>Override this to set up child node hierarchy and initialize fields.</li>
    /// <li>Code which is dependent on the node hierarchy should be executed in <see cref="Start"/>.</li>
    /// </ul>
    /// </summary>
    public Node(GameServiceContainer services, Scene scene)
    {
        Services = services;
        Scene = scene;
        Name = DEFAULT_NAME;
        Id = Guid.NewGuid();
        _children = new Dictionary<Guid, Node>();
    }

    #region NodeTree
    /// <summary>
    /// Instantiates a new child Node of type <typeparamref name="T"/> as a child of this Node.
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T AddChild<T>(string name) where T : Node => this.Instantiate<T>(name, this);
    
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
    #endregion
    
    #region Event Methods

    /// <summary>
    /// Called when scene is loading assets in preparation for starting the <see cref="Scene"/>.
    /// </summary>
    /// <param name="content"></param>
    public virtual void LoadContent(ContentManager content)
    {
        foreach (var child in this)
        {
            child.LoadContent(content);
        }
    }
    
    /// <summary>
    /// Called when initialization is complete and the scene is starting.
    /// </summary>
    /// <param name="services"></param>
    public virtual void Start()
    {
        foreach (var child in this)
        {
            child.Start();
        }
    }
    
    /// <summary>
    /// Called when the GameObject is drawn.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="spriteBatch"></param>
    public virtual void Draw()
    {
        foreach (var child in this)
        {
            child.Draw();
        }
    }
    
    /// <summary>
    /// Called for every <see cref="Node"/> on every frame.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="gameTime"></param>
    public virtual void Update(GameTime gameTime)
    {
        foreach (var child in this)
        {
            child.Update(gameTime);
        }
    }

    public virtual void OnDestroyed()
    {
        Parent?.RemoveChild(this);
    }
    
    #endregion
    
    #region IEnumerator
    public IEnumerator<Node> GetEnumerator()
    {
        return _children.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_children.Values).GetEnumerator();
    }

    #endregion
    
    #region Printing
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
        Console.Write(isLast ? "\u2514" : "\u251c");
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
    #endregion
    
    internal void AddChildReference(Node child)
    {
        _children.Add(child.Id, child);
    }
}