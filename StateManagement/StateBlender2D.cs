using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ViewportEngine.Util;

namespace ViewportEngine.StateManagement;

/// <summary>
/// Used as a multi-state. Blends between four states based on the sign of x and y values (set using <see cref="SetBlend"/>).
/// </summary>
/// <typeparam name="T"></typeparam>
public class StateBlender2D<T> : State, IEnumerable<T> where T : IState
{
    public new T RunningState => _activeState;
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
    
    public Vector2 Blend { get; private set; }

    private T _activeState;

    public StateBlender2D(GameServiceContainer services, string name) : base(services, name)
    {
        Left = (T)Activator.CreateInstance(typeof(T), services, $"{typeof(T).Name}Left");
        Right = (T)Activator.CreateInstance(typeof(T), services, $"{typeof(T).Name}Right");
        Up = (T)Activator.CreateInstance(typeof(T), services, $"{typeof(T).Name}Up");
        Down = (T)Activator.CreateInstance(typeof(T), services, $"{typeof(T).Name}Down");
    }

    /// <summary>
    /// Sets the currently active substate based on the vector represented by <paramref name="x"/> and <paramref name="y"/>.
    /// Note: x gains precedence over y.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetBlend(float x, float y)
    {
        // decide real x and y directions
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

        if (newActive.Equals(_activeState)) return;

        Blend = new Vector2(x, y);
        SetActiveSubState(newActive);
    }

    private void SetActiveSubState(T state)
    {
        _activeState?.Exit();
        _activeState = state;
        Left.SetActive(state.Equals(Left));
        Up.SetActive(state.Equals(Up));
        Down.SetActive(state.Equals(Down));
        Right.SetActive(state.Equals(Right));
        _activeState?.Enter();
    }

    public override void Enter()
    {
        base.Enter();
        var defaultState = _activeState;
        _activeState = default(T);
        SetActiveSubState(defaultState);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _activeState.Update(gameTime);
    }

    public override IState GetRunningState()
    {
        return _activeState;
    }

    public T GetRunningStateGeneric() => (T)_activeState.GetRunningState();
    
    public event Action OnEnter;
    public event Action<GameTime> OnUpdate;
    public event Action OnExit;

    public IEnumerator<T> GetEnumerator()
    {
        var directions = new List<T> { Left, Right, Up, Down };
        return directions.GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public override string ToString()
    {
        return $"(State \"{Name}\", {IsActive}, Blend={Blend.X},{Blend.Y})";
    }
}