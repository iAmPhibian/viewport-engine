using Microsoft.Xna.Framework;

namespace ViewportEngine.Util;

public interface IWindowHandler
{
    public int Width { get; }
    public int Height { get; }
    public Point Size { get; }

    public Point Position { get; }
    public Point CenterPosition { get; }
    
    void SetWindowSize(int width, int height);
    void SetWindowPosition(int x, int y);

    public void SetRenderScale(int multiplier);
}