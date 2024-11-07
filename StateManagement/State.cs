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
    
    public event Action OnEnter;
    public event Action OnUpdate;
    public event Action OnExit;

    private GameServiceContainer _services = services;

    public override string ToString()
    {
        return $"(State \"{Name}\", {IsActive})";
    }
    
    public void SetActive(bool active)
    {
        switch (active)
        {
            case true when !IsActive:
                IsActive = true;
                Enter();
                break;
            case false when IsActive:
                IsActive = false;
                Exit();
                break;
        }
    }

    public virtual void InitGame()
    {
        
    }

    public virtual void Enter()
    {
        OnEnter?.Invoke();
    }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Exit()
    {
        OnExit?.Invoke();
    }
}