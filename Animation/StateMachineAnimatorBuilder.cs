using System;
using System.Collections.Generic;
using ViewportEngine.StateManagement;

namespace ViewportEngine.Animation;

public class StateMachineAnimatorBuilder<T> : IStateMachineAnimatorBuilder<T> where T : Enum
{
    private readonly Dictionary<IState, Func<Spritesheet.Animation>> _stateAnimationLinks = new();

    public IStateMachineAnimatorBuilder<T> LinkState(IState state, Func<Spritesheet.Animation> getFunction)
    {
        _stateAnimationLinks.Add(state, getFunction);
        return this;
    }

    public IStateMachineAnimatorBuilder<T> LinkStates(Dictionary<IState, Func<Spritesheet.Animation>> dict)
    {
        foreach (var pair in dict)
        {
            LinkState(pair.Key, pair.Value);
        }

        return this;
    }

    public StateMachineAnimator<T> Build(StateMachine<T> stateMachine)
    {
        return new StateMachineAnimator<T>(stateMachine, _stateAnimationLinks);
    }
}