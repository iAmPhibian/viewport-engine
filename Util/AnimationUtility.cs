using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ViewportEngine.Animation;
using ViewportEngine.StateManagement;
using ViewportGame;

namespace ViewportEngine.Util;

public static class AnimationUtility
{
    public static Dictionary<IState, Func<Spritesheet.Animation>> GetDirectionalAnimationDictionary<T>(StateBlender2D<T> stateBlender, DirectionalAnimations animations) where T : IState
    {
        return new Dictionary<IState, Func<Spritesheet.Animation>>()
        {
            { stateBlender.Left, () => animations.GetDirectionalAnimation(Direction4.Left) },
            { stateBlender.Right, () => animations.GetDirectionalAnimation(Direction4.Right) },
            { stateBlender.Up, () => animations.GetDirectionalAnimation(Direction4.Up) },
            { stateBlender.Down, () => animations.GetDirectionalAnimation(Direction4.Down) },
        };
    }
    
    /// <summary>
    /// Returns a <see cref="Direction4"/> animation direction given <paramref name="vecDirection"/>.
    /// </summary>
    /// <param name="vecDirection"></param>
    /// <returns></returns>
    public static Direction4 GetAnimationDirection(Vector2 vecDirection)
    {
        return vecDirection.X switch
        {
            > 0f => Direction4.Right,
            < 0f => Direction4.Left,
            _ => vecDirection.Y switch
            {
                < 0f => Direction4.Up,
                > 0f => Direction4.Down,
                _ => Direction4.Down
            }
        };
    }
}