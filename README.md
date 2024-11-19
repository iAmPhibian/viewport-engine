# Viewport Engine

A framework in C# for building 2D video games. It is built upon MonoGame in C# using dotnet 8.0.

# ECS

Viewport Engine uses a currently recursive (unoptimized) ECS in which Entities (Nodes) contain at most 1 parent and any number of child nodes. This is sufficient for my current needs, but I will likely replace it with a data-oriented implementation that utilizes CPU caching and includes support for Systems as well as Nodes.

# Creating a Game

Creating a game with ViewportEngine is as simple as creating a class that inherits from `VPEGame` and making an instance of your child class.
e.g.

### MyGame.cs
~~~c#
using Microsoft.Xna.Framework;  
using ViewportEngine;  
  
public class MyGame : VPEGame  
{
	protected override void Initialize()  
	{  
		// Used for initializing game state
		
		base.Initialize();
	}
	
	protected override void LoadContent()  
	{  
		// Used for loading content
		
		base.LoadContent();  
	}
	
	protected override void UnloadContent()  
	{  
		// Used for unloading content
		
		base.UnloadContent();  
	}
	
	protected override void Update(GameTime gameTime)  
	{  
		// Used for updating game state  
		
		base.Update(gameTime);
	}
	
	protected override void Draw(GameTime gameTime)  
	{  
		// Used for drawing to SpriteBatch  
		
		base.Draw(gameTime);
	}  
}
~~~
### Program.cs
~~~c#
using var game = new MyGame();  
game.Run();
~~~

# The Node Class
`Node` is both the Component and the Entity in this ECS. A `Node` has many overridable functions, including an `Update(GameTime gameTime)` function, `Draw()` function, and `Start()` function, as well as a required `Node(GameServicesContainer services, Scene scene)` constructor.

In order to create different types of objects and different object components, one must create classes that derive from `Node`.

# The Scene Class
A `Scene` contains and manages a root `Node` which it can add children to, load, and unload. A `Scene` is loaded or unloaded using an `ISceneManager`, which can be added to `VPEGame.Services` during `Initialize()`
e.g.
~~~c#
using Microsoft.Xna.Framework;  
using ViewportEngine;  
  
public class MyGame : VPEGame  
{
	// Stored in field for easy use across functions
	private SceneManager _sceneManager;

	private const int DEFAULT_WIDTH = 640;
	private const int DEFAULT_HEIGHT = 480;

	protected override void Initialize()  
	{  
		// Create new default Scene
		// Note that GameScene would have to be a class that inherits from Scene
		Scene defaultScene = new GameScene(Services);
		
		// Create new SceneManager using the defaultScene  
		_sceneManager = new SceneManager(
			Services, defaultScene, DEFAULT_WIDTH, DEFAULT_HEIGHT);
			
		// Add the SceneManager as a service for future use in Nodes, etc. 	
		Services.AddService(typeof(ISceneManager), _sceneManager);
		
		base.Initialize();
	}
	
	protected override void LoadContent()  
	{  
		_sceneManager.LoadContent(Content);
		
		base.LoadContent();  
	}
	
	protected override void UnloadContent()  
	{  
		_sceneManager.UnloadContent();
		
		base.UnloadContent();  
	}
	
	protected override void Update(GameTime gameTime)  
	{  
		_sceneManager.Update(gameTime);
		
		base.Update(gameTime);
	}
	
	protected override void Draw(GameTime gameTime)  
	{  
		// SceneManager handles Begin() and End() for the SpriteBatch
		_sceneManager.Draw();
		
		base.Draw(gameTime);
	}  
}
~~~
