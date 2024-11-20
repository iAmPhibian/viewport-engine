using System;
using Microsoft.Xna.Framework.Input;

namespace ViewportEngine.Input;

/// <summary>
/// Used for checking the current state of input
/// </summary>
public interface IInputHandler
{
    public KeyboardState KeyboardState { get; }
    public MouseState MouseState { get; }
    
    public event Action<EventContext> OnKeyPressed;
    public event Action<EventContext> OnKeyReleased;
    public event Action<int, int> OnMouseMoved;
    public event Action<int, int> OnMouseButtonPressed;
    public event Action<int, int> OnMouseButtonReleased;
    
    public void Update();
}