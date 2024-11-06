using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace ViewportEngine.StateManagement;

/// <summary>
/// Manages a set of states which implement <see cref="IState"/> using enum type <typeparamref name="T"/>.
/// </summary>
public class StateMachine<T> where T : Enum
{
    private readonly Dictionary<T, IState> _managedStates;
    public T StateMode { get; private set; }
    public IState ActiveState { get; private set; }
    
    /// <summary>
    /// Creates a new <see cref="StateMachine{T}"/> which manages <paramref name="states"></paramref> and defaults to <paramref name="defaultState"/>.
    /// </summary>
    /// <param name="states"></param>
    /// <param name="defaultState"></param>
    public StateMachine(Dictionary<T, IState> states, T defaultState)
    {
        _managedStates = states;
        foreach (var state in _managedStates)
        {
            state.Value.InitGame();
        }
        SetActiveState(defaultState);
    }

    /// <summary>
    /// If <paramref name="newStateEnum"/> is different from Enables <paramref name="newStateEnum"/> and disables all other states.
    /// <paramref name="newStateEnum"/> is not guaranteed to be a part of this state machine
    /// </summary>
    /// <param name="newStateEnum"></param>
    public void SetActiveState(T newStateEnum)
    {
        // Same as current state: return
        if (newStateEnum.Equals(StateMode)) return;
        
        // Disable current state if it exists
        ActiveState?.SetActive(false);
        // Update enum mode
        StateMode = newStateEnum;
        var newStateRef = GetState(newStateEnum);

        // Enable newState, disable every other state
        foreach (var state in _managedStates)
        {
            state.Value.SetActive(state.Key.Equals(StateMode));
        }
        
        ActiveState = newStateRef;
    }

    /// <summary>
    /// Returns the managed <see cref="IState"/> for <paramref name="state"/>.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public IState GetState(T state)
    {
        try
        {
            return _managedStates[state];
        }
        catch (KeyNotFoundException)
        {
            throw new KeyNotFoundException($"StateMachine has no managed state reference for state '{state.ToString()}'");
        }
    }

    /// <summary>
    /// Gets the active state of the machine.
    /// </summary>
    /// <returns></returns>
    public IState GetActiveState()
    {
        return ActiveState;
    }
    
    public void UpdateMachine(GameTime gameTime)
    {
        // Update active state if it exists
        ActiveState?.Update(gameTime);
    }
}