using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using ViewportEngine.StateManagement;

namespace ViewportEngine.Debug;

public static class DebugHelper
{
    public static BitmapFont DebugFont;
    
    [Conditional("DEBUG")]
    public static void DrawText(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, string debugText, Vector2 position, Color? color = null)
    {
        if (DebugFont == null)
        {
            throw new Exception("DebugFont is null! Must be set in LoadContent() of your VPEGame.");
        }
        color ??= Color.White;
        spriteBatch.DrawString(DebugFont, debugText, position, color.Value);
    }
}