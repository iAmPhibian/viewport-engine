using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace ViewportEngine.Input;

public class InputHandler : IInputHandler
{
    public KeyboardState KeyboardState { get; private set; }
    public MouseState MouseState { get; private set; }
    
    private KeyboardState _oldKeyboardState;
    private MouseState _oldMouseState;

    public event Action<EventContext> OnKeyPressed;
    public event Action<EventContext> OnKeyReleased;
    public event Action<int, int> OnMouseMoved;
    public event Action<int, int> OnMouseButtonPressed;
    public event Action<int, int> OnMouseButtonReleased;

    public void Update()
    {
        KeyboardState = Keyboard.GetState();
        MouseState = Mouse.GetState();

        // Handle key presses
        foreach (var key in Enum.GetValues(typeof(Keys)).Cast<Keys>())
        {
            var context = new EventContext() { InputHandler = this, Keys = key };
            if (KeyboardState.IsKeyDown(key) && !_oldKeyboardState.IsKeyDown(key))
                OnKeyPressed?.Invoke(context);

            if (!KeyboardState.IsKeyDown(key) && _oldKeyboardState.IsKeyDown(key))
                OnKeyReleased?.Invoke(context);
        }

        // Handle mouse movement
        if (MouseState.X != _oldMouseState.X || MouseState.Y != _oldMouseState.Y)
            OnMouseMoved?.Invoke(MouseState.X, MouseState.Y);

        // Handle mouse buttons
        if (MouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
            OnMouseButtonPressed?.Invoke(MouseState.X, MouseState.Y);

        if (MouseState.LeftButton == ButtonState.Released && _oldMouseState.LeftButton == ButtonState.Pressed)
            OnMouseButtonReleased?.Invoke(MouseState.X, MouseState.Y);

        // Update previous states
        _oldKeyboardState = KeyboardState;
        _oldMouseState = MouseState;
    }
}