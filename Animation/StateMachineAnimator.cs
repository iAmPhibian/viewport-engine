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
    private Func<Spritesheet.Animation> _animationGetter;

    private void SetAnimation(Func<Spritesheet.Animation> animationGetter)
    {
        _animationGetter = animationGetter;
        CurrentAnimation = animationGetter.Invoke();
        CurrentAnimation.Reset();
        CurrentAnimation.Start(Repeat.Mode.Loop);
    }

    public void UpdateAnimatorState()
    {
        SetAnimation(_animationGetter);
    }
    
    public StateMachineAnimator(StateMachine<T> stateMachine, Dictionary<T, Func<Spritesheet.Animation>> stateLinks)
    {
        foreach (var pair in stateLinks)
        {
            // If this pair represents the active state
            if (pair.Key.Equals(stateMachine.StateMode))
            {
                SetAnimation(pair.Value);
            }

            stateMachine.GetState(pair.Key).OnEnter += () => SetAnimation(pair.Value);
        }
    }

    public void Update(GameTime gameTime)
    {
        CurrentAnimation.Update(gameTime);
    }
}