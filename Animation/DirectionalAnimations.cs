using Microsoft.Xna.Framework.Graphics;
using Spritesheet;
using ViewportGame;
using ViewportGame.Util;

namespace ViewportEngine.Animation;

/// <summary>
/// Represents a set of Down, Right, Up, and Left animations
/// </summary>
public class DirectionalAnimations
{
    private Spritesheet.Animation _down, _right, _up, _left;

    /// <summary>
    /// Sets the directional animation specified by <paramref name="direction"/> to <paramref name="anim"/>.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="anim"></param>
    public void SetDirectionalAnimation(Direction4 direction, Spritesheet.Animation anim)
    {
        switch (direction)
        {
            case Direction4.Down:
                _down = anim;
                break;
            case Direction4.Right:
                _right = anim;
                break;
            case Direction4.Up:
                _up = anim;
                break;
            case Direction4.Left:
            default:
                _left = anim;
                break;
        }
    }
    
    /// <summary>
    /// Returns the directional animation specified by <paramref name="direction"/>.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Spritesheet.Animation GetDirectionalAnimation(Direction4 direction)
    {
        switch (direction)
        {
            case Direction4.Down:
                return _down;
            case Direction4.Right:
                return _right;
            case Direction4.Up:
                return _up;
            case Direction4.Left:
            default:
                return _left;
        }
    }

    /// <summary>
    /// Returns a new instance of <see cref="DirectionalAnimations"/> with animations set up from <paramref name="spritesheet"/>,
    /// based on the <paramref name="startCell"/>,
    /// the amount of cells in a row to use <paramref name="cellLength"/>,
    /// and frame durations <paramref name="frameDuration"/>.
    /// </summary>
    /// <param name="spritesheet"></param>
    /// <param name="startCell"></param>
    /// <param name="cellLength"></param>
    /// <param name="frameDuration"></param>
    /// <returns></returns>
    public static DirectionalAnimations CreateFrom(Spritesheet.Spritesheet spritesheet, (int x, int y) startCell, Layout directionalLayout, int cellLength, float frameDuration)
    {
        var newAnims = new DirectionalAnimations();
        foreach (var dir in EnumUtil.GetValues<Direction4>())
        {
            // These will all create the same animation: dir is never cast to int
            var frames = new Frame[cellLength];
            // the offset that iterates over 0, 1, 2, 3, for all 4 directions
            var animationCellOffset = (int)dir;
            for (var animationFrameOffset = 0; animationFrameOffset < cellLength; animationFrameOffset++)
            {
                // the offset that iterates from [0...cellLength-1] for each frame in the animation
                var frame = spritesheet.CreateFrame(
                    startCell.x + (directionalLayout == Layout.Horizontal ? animationCellOffset : animationFrameOffset), 
                    startCell.y + (directionalLayout == Layout.Horizontal ? animationFrameOffset : animationCellOffset), 
                    frameDuration, 
                    SpriteEffects.None
                    );
                frames[animationFrameOffset] = frame;
            }
            var anim = new Spritesheet.Animation(frames);
            newAnims.SetDirectionalAnimation(dir, anim);
        }

        return newAnims;
    }
}