using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ViewportEngine.Input;

/// <summary>
/// Listens for the state changing of 4 directional inputs and
/// triggers <see cref="OnUpdated"/> with a Vector2 argument corresponding to the overall directional input.
/// </summary>
/// <param name="right"></param>
/// <param name="left"></param>
/// <param name="up"></param>
/// <param name="down"></param>
public class Axis2DKeyBinding(Keys right, Keys left, Keys up, Keys down)
    : IKeyBinding
{
    public event Action<Vector2> OnUpdated;
    
    private AxisKeyBinding _horizontalAxis;
    private AxisKeyBinding _verticalAxis;

    private Vector2 _input;

    // Binds two 1-dimensional axis controls for vertical and horizontal
    public IKeyBinding BindControls(IInputHandler handler)
    {
        _horizontalAxis = new AxisKeyBinding(right, left);
        _horizontalAxis.OnUpdated += OnHorizontalAxisUpdated;
        _verticalAxis = new AxisKeyBinding(down, up);
        _verticalAxis.OnUpdated += OnVerticalAxisUpdated;
        
        _horizontalAxis.BindControls(handler);
        _verticalAxis.BindControls(handler);
        return this;
    }

    private void OnHorizontalAxisUpdated(float newValue)
    {
        _input = new Vector2(newValue, _input.Y);
        OnUpdated?.Invoke(_input);
    }
    
    private void OnVerticalAxisUpdated(float newValue)
    {
        _input = new Vector2(_input.X, newValue);
        OnUpdated?.Invoke(_input);
    }
}