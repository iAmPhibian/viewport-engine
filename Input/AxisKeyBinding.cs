using System;
using Microsoft.Xna.Framework.Input;

namespace ViewportEngine.Input;

/// <summary>
/// Listens for the state changing of 2 keys,
/// and triggers <see cref="OnUpdated"/> with -1, 0, or 1 depending on the state of <paramref name="positive"/> and <paramref name="negative"/>.
/// </summary>
/// <param name="positive"></param>
/// <param name="negative"></param>
public class AxisKeyBinding(Keys positive, Keys negative)
    : IKeyBinding
{
    public event Action<float> OnUpdated;

    public IKeyBinding BindControls(IInputHandler handler)
    {
        handler.OnKeyPressed += OnKeyEvent;
        handler.OnKeyReleased += OnKeyEvent;
        return this;
    }

    internal void OnKeyEvent(EventContext context)
    {
        if (context.Keys != positive && context.Keys != negative) return;
        var kState = context.InputHandler.KeyboardState;
        var positivePressed = kState.IsKeyDown(positive) ? 1 : 0;
        var negativePressed = kState.IsKeyDown(negative) ? 1 : 0;
        var axis = positivePressed - negativePressed;
        OnUpdated?.Invoke(axis);
    }
}