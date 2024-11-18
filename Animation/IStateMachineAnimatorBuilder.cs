using System;
using System.Collections.Generic;
using ViewportEngine.StateManagement;

namespace ViewportEngine.Animation;

/// <summary>
/// Creates a <see cref="StateMachineAnimator{T}"/> after being provided with a Dictionary of links from state to animation getter. 
/// </summary>
public interface IStateMachineAnimatorBuilder<T> where T : Enum
{
    public IStateMachineAnimatorBuilder<T> LinkState(IState state, Func<Spritesheet.Animation> getFunction);
    public IStateMachineAnimatorBuilder<T> LinkStates(Dictionary<IState, Func<Spritesheet.Animation>> dict);
    public StateMachineAnimator<T> Build(StateMachine<T> stateMachine);
}