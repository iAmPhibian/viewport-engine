using System;
using Microsoft.Xna.Framework;
using ViewportEngine.Util;

namespace ViewportEngine.StateManagement;

/// <summary>
/// Interface implemented by all states
/// </summary>
public interface IState
{
    /// <summary>
    /// The human-readable name of this state.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Returns whether this state is active.
    /// </summary>
    /// <returns></returns>
    public bool IsActive { get; }

    /// <summary>
    /// Either enters or exits the state based on <paramref name="active"/>.
    /// </summary>
    /// <param name="active"></param>
    internal void SetActive(bool active);
        
    /// <summary>
    /// Called every frame while the state is active.
    /// </summary>
    /// <param name="gameTime"></param>
    internal void Update(GameTime gameTime);

    /// <summary>
    /// Returns the running state of this state. Should only be used when <see cref="IsActive"/> == true.
    /// </summary>
    /// <returns>this, or <see cref="IState"/> representing a substate of this state</returns>
    public IState GetRunningState();
}