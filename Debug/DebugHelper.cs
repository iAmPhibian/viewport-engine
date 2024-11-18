using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using ViewportEngine.Util;

namespace ViewportEngine.Debug;

public static class DebugHelper
{
    public static BitmapFont DebugFont;
    
    [Conditional("DEBUG")]
    public static void DrawText(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, string debugText, Vector2 position, Vector2? pivot = null, Color? color = null)
    {
        if (DebugFont == null)
        {
            throw new Exception("DebugFont is null! Must be set in LoadContent() of your VPEGame.");
        }
        color ??= Color.White;
        pivot ??= Pivots.MiddleCenter;
        var size = DebugFont.MeasureString(debugText);
        spriteBatch.DrawString(DebugFont, debugText, position - pivot.Value * size, color.Value);
    }
}