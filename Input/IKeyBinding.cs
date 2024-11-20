using System;
using Microsoft.Xna.Framework.Input;

namespace ViewportEngine.Input;

/// <summary>
/// Represents 1+ input listeners that can output events based on raw keyboard data.
/// </summary>
public interface IKeyBinding
{
    /// <summary>
    /// Used to set up listeners for various controls.
    /// </summary>
    /// <param name="handler"></param>
    public IKeyBinding BindControls(IInputHandler handler);
}