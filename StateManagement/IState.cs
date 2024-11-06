using System;
using Microsoft.Xna.Framework;

namespace ViewportEngine.StateManagement;

/// <summary>
/// Interface implemented by all states
/// </summary>
public interface IState
{
    public string Name { get; }
    
    /// <summary>
    /// Sets up the state for future usage.
    /// </summary>
    public void InitGame();

    /// <summary>
    /// Either enables or disables the state based on <paramref name="active"/>.
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active);

    /// <summary>
    /// Returns whether this state is active.
    /// </summary>
    /// <returns></returns>
    public bool IsActive();
    
    /// <summary>
    /// Called when the state is entered.
    /// </summary>
    public void Enter();
        
    /// <summary>
    /// Called every frame while the state is active
    /// </summary>
    public void Update(GameTime gameTime);
        
    /// <summary>
    /// Called when the state is exited.
    /// </summary>
    public void Exit();
    
    public event Action OnEnter;
    public event Action OnExit;
}