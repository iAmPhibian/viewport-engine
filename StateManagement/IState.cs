using System;
using Microsoft.Xna.Framework;
using ViewportEngine.Util;

namespace ViewportEngine.StateManagement;

/// <summary>
/// Interface implemented by all states
/// </summary>
public interface IState
{
    public string Name { get; }
    
    /// <summary>
    /// Returns whether this state is active.
    /// </summary>
    /// <returns></returns>
    public bool IsActive { get; }

    /// <summary>
    /// Either enables or disables the state based on <paramref name="active"/>.
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active);
    
    /// <summary>
    /// Called when the state is entered.
    /// </summary>
    public void Enter();
        
    /// <summary>
    /// Called every frame while the state is active.
    /// </summary>
    /// <param name="gameTime"></param>
    public void Update(GameTime gameTime);
        
    /// <summary>
    /// Called when the state is exited.
    /// </summary>
    public void Exit();

    public IState GetRunningState();

    public event Action OnEnter;
    public event Action<GameTime> OnUpdate;
    public event Action OnExit;
}