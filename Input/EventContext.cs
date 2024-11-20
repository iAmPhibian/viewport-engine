using Microsoft.Xna.Framework.Input;

namespace ViewportEngine.Input;

public record EventContext
{
    public required IInputHandler InputHandler { get; init; }
    public required Keys Keys { get; init; }
};