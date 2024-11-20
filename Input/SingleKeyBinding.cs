using System;
using Microsoft.Xna.Framework.Input;

namespace ViewportEngine.Input;

/// <summary>
/// Listens for the state changing of a single key.
/// </summary>
public class SingleKeyBinding : IKeyBinding
{
    private readonly Keys _key;

    public event Action OnPressed;
    public event Action OnReleased;

    public SingleKeyBinding(Keys key)
    {
        _key = key;
    }
    
    public IKeyBinding BindControls(IInputHandler handler)
    {
        handler.OnKeyPressed += context =>
        {
            if (context.Keys == _key) OnPressed?.Invoke();
        };
        handler.OnKeyReleased += context =>
        {
            if (context.Keys == _key) OnReleased?.Invoke();
        };
        return this;
    }
}