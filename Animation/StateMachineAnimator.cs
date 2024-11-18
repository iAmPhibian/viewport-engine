using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ViewportEngine.StateManagement;
using Repeat = Spritesheet.Repeat;

namespace ViewportEngine.Animation;

/// <summary>
/// Links state entries in a <see cref="StateMachine{T}"/> with <see cref="Animation"/>.
/// </summary>
public class StateMachineAnimator<T> where T : Enum
{
    public Spritesheet.Animation CurrentAnimation { get; private set; }

    private Dictionary<IState, Func<Spritesheet.Animation>> _stateAnimationLinks;

    internal StateMachineAnimator(StateMachine<T> stateMachine, Dictionary<IState, Func<Spritesheet.Animation>> stateLinks)
    {
        _stateAnimationLinks = stateLinks;
        stateMachine.OnStateChangedTo += OnStateChangedTo;
        SetAnimation(_stateAnimationLinks[stateMachine.GetRunningStateBehavior()]);
    }
    
    private void SetAnimation(Func<Spritesheet.Animation> animationGetter)
    {
        CurrentAnimation = animationGetter.Invoke();
        CurrentAnimation.Reset();
        CurrentAnimation.Start(Repeat.Mode.Loop);
    }

    public void OnStateChangedTo(IState newState)
    {
        if (!_stateAnimationLinks.TryGetValue(newState, out var animFunc))
        {
            throw new Exception($"Cannot find state '{newState}' in state machine animator");
        }

        SetAnimation(animFunc);
    }

    public void Update(GameTime gameTime)
    {
        CurrentAnimation.Update(gameTime);
    }
}