using System;

namespace ViewportEngine.StateManagement;

/// <summary>
/// Creates a <see cref="StateMachine{T}"/>
/// </summary>
public interface IStateMachineBuilder<T> where T : Enum
{
    public IStateMachineBuilder<T> AddState(T state, IState stateRef);
    public StateMachine<T> Build();
}