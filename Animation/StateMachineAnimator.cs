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
    
    public StateMachineAnimator(StateMachine<T> stateMachine, Dictionary<IState, Func<Spritesheet.Animation>> stateLinks)
    {
        // Add listeners to all state OnEnter events to set their animations upon entry
        foreach (var pair in stateLinks)
        {
            pair.Key.OnEnter += () => SetAnimation(pair.Value);
        }
    }

    public void Update(GameTime gameTime)
    {
        CurrentAnimation.Update(gameTime);
    }
}