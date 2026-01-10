using Raylib_cs;

namespace Pipboy2K.Engine.Core;

/// <summary>
/// TODO: Game initialization class that also handles disposing.
/// </summary>
public class Game : IDisposable
{
    protected Game()
    {

    }

    public void Run()
    {
        Raylib.SetConfigFlags(ConfigFlags.InterlacedHint);

        Raylib.InitWindow(700, 700, "Pip-boy 2000 MK VI | In-dev");
        Raylib.SetTargetFPS(30);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    public void Dispose()
    {

    }
}