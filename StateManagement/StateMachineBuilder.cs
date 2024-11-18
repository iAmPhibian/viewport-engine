using System;
using System.Collections.Generic;

namespace ViewportEngine.StateManagement;

/// <summary>
/// Builder pattern used to create a <see cref="StateMachine{T}"/>
/// </summary>
public class StateMachineBuilder<T> : IStateMachineBuilder<T> where T : Enum
{
    private readonly Dictionary<T, IState> _addedStates = new();

    public StateMachineBuilder<T> AddState(T state, IState stateRef)
    {
        _addedStates.Add(state, stateRef);
        return this;
    }
    
    public StateMachine<T> Build()
    {
        return new StateMachine<T>(_addedStates, default(T));
    }

    IStateMachineBuilder<T> IStateMachineBuilder<T>.AddState(T state, IState stateRef)
    {
        return AddState(state, stateRef);
    }
}