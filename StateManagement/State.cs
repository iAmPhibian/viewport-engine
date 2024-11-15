using System;
using Microsoft.Xna.Framework;

namespace ViewportEngine.StateManagement;

/// <summary>
/// Represents a switchable state
/// </summary>
public abstract class State(GameServiceContainer services, string name) : IState
{
    public string Name { get; } = name;
    public bool IsActive { get; private set; }

    public virtual IState GetRunningState()
    {
        return this;
    }

    public event Action OnEnter;
    public event Action<GameTime> OnUpdate;
    public event Action OnExit;

    protected GameServiceContainer Services { get; private set; } = services;

    public override string ToString()
    {
        return $"(State \"{Name}\", {IsActive})";
    }
    
    public void SetActive(bool active)
    {
        IsActive = active;
        switch (active)
        {
            case true when !IsActive:
                Enter();
                break;
            case false when IsActive:
                Exit();
                break;
        }
    }

    public virtual void Enter()
    {
        OnEnter?.Invoke();
    }

    public virtual void Update(GameTime gameTime)
    {
        OnUpdate?.Invoke(gameTime);
    }

    public virtual void Exit()
    {
        OnExit?.Invoke();
    }
}