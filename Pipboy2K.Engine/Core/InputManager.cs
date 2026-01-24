using Raylib_cs;

namespace Pipboy2K.Engine.Core;

// TODO, this should be able to abstract Raylib's input system in order to incorporate external input systems such as GPIO buttons.
public class InputManager
{
    internal InputManager()
    {

    }
}

public interface IInputSystem
{
    public bool IsKeyDown();
    public bool IsKeyUp();
}