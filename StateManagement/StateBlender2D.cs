using System;
using Microsoft.Xna.Framework;
using ViewportEngine.Util;

namespace ViewportEngine.StateManagement;

/// <summary>
/// Used as a multi-state. Blends between four states based on the sign of x and y values (set using <see cref="SetBlend"/>).
/// </summary>
/// <typeparam name="T"></typeparam>
public class StateBlender2D<T> : IState where T : IState
{
    public string Name { get; }
    public bool IsActive { get; private set; }
    /// <summary>
    /// The state for (-1.0, 0.0), (-1.0, 1.0), and (-1.0, -1.0).
    /// </summary>
    public T Left { get; set; }
    /// <summary>
    /// The state for (1.0, 0.0), (1.0, 1.0), and (1.0, -1.0).
    /// </summary>
    public T Right { get; set; }
    /// <summary>
    /// The state for (0.0, 1.0).
    /// </summary>
    public T Up { get; set; }
    /// <summary>
    /// The state for (0.0, -1.0).
    /// </summary>
    public T Down { get; set; }

    private T _active;

    public StateBlender2D(GameServiceContainer services, string name)
    {
        Name = name;
        
        Left = (T)Activator.CreateInstance(typeof(T), services, $"{typeof(T).Name}Left");
        Right = (T)Activator.CreateInstance(typeof(T), services, $"{typeof(T).Name}Right");
        Up = (T)Activator.CreateInstance(typeof(T), services, $"{typeof(T).Name}Up");
        Down = (T)Activator.CreateInstance(typeof(T), services, $"{typeof(T).Name}Down");

        _active = Down;
    }

    /// <summary>
    /// Sets the current animation based on the vector represented by <paramref name="x"/> and <paramref name="y"/>.
    /// Note: x gains precedence over y.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetBlend(float x, float y)
    {
        bool isXZero = x.IsWithinEpsilon();
        bool isYZero = y.IsWithinEpsilon();
        
        if (isXZero && isYZero) return;
        
        T newActive;
        if (isXZero)
        {
            newActive = y > 0 ? Up : Down;
        }
        else
        {
            newActive = x > 0 ? Right : Left;
        }

        if (newActive.Equals(_active)) return;
        
        _active?.Exit();
        newActive?.Enter();
        _active = newActive;
    }
    
    public void SetActive(bool active)
    {
        Left.SetActive(active);
        Up.SetActive(active);
        Down.SetActive(active);
        Right.SetActive(active);
        IsActive = active;
    }

    public void Enter()
    {
        _active.Enter();
        
        OnEnter?.Invoke();
    }

    public void Update(GameTime gameTime)
    {
        _active.Update(gameTime);
        
        OnUpdate?.Invoke(gameTime);
    }

    public void Exit()
    {
        _active.Exit();
        
        OnExit?.Invoke();
    }

    public IState GetRunningState()
    {
        return _active.GetRunningState();
    }

    public event Action OnEnter;
    public event Action<GameTime> OnUpdate;
    public event Action OnExit;
}