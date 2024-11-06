using System;
using Microsoft.Xna.Framework;

namespace ViewportEngine.StateManagement;

/// <summary>
/// Represents a switchable state
/// </summary>
public abstract class State(string name) : IState
{
    public string Name { get; } = name;
    private bool _active;
    
    public event Action OnEnter;
    public event Action OnUpdate;
    public event Action OnExit;

    public override string ToString()
    {
        return $"({Name}, {IsActive()})";
    }
    
    public void SetActive(bool active)
    {
        switch (active)
        {
            case true when !IsActive():
                _active = true;
                Enter();
                break;
            case false when IsActive():
                _active = false;
                Exit();
                break;
        }
    }

    public bool IsActive()
    {
        return _active;
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