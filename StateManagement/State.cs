using System;
using Microsoft.Xna.Framework;

namespace ViewportEngine.StateManagement;

/// <summary>
/// Represents a switchable state
/// </summary>
public abstract class State(GameServiceContainer services, string name) : IState
{
    public string Name { get; } = name;
    public bool IsActive { get; set; }

    public virtual IState GetRunningState()
    {
        return this;
    }

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

    protected abstract void Enter();
    public abstract void Update(GameTime gameTime);
    protected abstract void Exit();
}