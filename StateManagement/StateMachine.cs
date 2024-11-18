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
    // TODO: Use T as an int index and create an array based off the size of T as an enum
    // - To take advantage of cache
    private readonly Dictionary<T, IState> _managedStates;
    public T StateMode { get; private set; }
    private IState _activeState;

    public event Action<IState> OnStateChangedTo;
    
    /// <summary>
    /// Creates a new <see cref="StateMachine{T}"/> which manages <paramref name="states"></paramref> and defaults to <paramref name="defaultStateEnum"/>.
    /// </summary>
    /// <param name="states"></param>
    /// <param name="defaultStateEnum"></param>
    internal StateMachine(Dictionary<T, IState> states, T defaultStateEnum)
    {
        _managedStates = states;
        UpdateStateTo(defaultStateEnum);
    }

    /// <summary>
    /// If <paramref name="newStateEnum"/> differs from the current state, enables <paramref name="newStateEnum"/> and disables all other states.
    /// <paramref name="newStateEnum"/> is not guaranteed to be a part of this state machine
    /// </summary>
    /// <param name="newStateEnum"></param>
    public void SetActiveState(T newStateEnum)
    {
        // Same as current state (taking into account substates): return
        var runningStateBehavior = GetRunningStateBehavior();
        var newState = GetStateFor(newStateEnum);
        if (newStateEnum.Equals(StateMode) && newState.GetRunningState() == runningStateBehavior) return;

        // Enable newState, disable active state
        UpdateStateTo(newStateEnum);
    }

    private void UpdateStateTo(T newState)
    {
        _activeState?.SetActive(false);
        var baseState = GetStateFor(newState);
        baseState.SetActive(true);
        _activeState = baseState.GetRunningState();
        StateMode = newState;
        OnStateChangedTo?.Invoke(baseState.GetRunningState());
    }

    /// <summary>
    /// Returns the managed <see cref="IState"/> for <paramref name="state"/>.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public IState GetStateFor(T state)
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
        return _activeState;
    }

    public IState GetRunningStateBehavior()
    {
        var state = _activeState;
        while (true)
        {
            var runningState = state.GetRunningState();
            if (state.Equals(runningState)) return state;
            state = runningState;
        }
    }

    public void UpdateMachine(GameTime gameTime)
    {
        // Update active state if it exists
        _activeState?.Update(gameTime);
    }
}