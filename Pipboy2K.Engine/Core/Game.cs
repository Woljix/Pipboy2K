using Raylib_cs;

namespace Pipboy2K.Engine.Core;

/// <summary>
/// TODO: Game initialization class that also handles disposing.
/// </summary>
//public class Game<T> : IDisposable where T : IGameState
public class Game : IDisposable
{
    private Dictionary<Type, object> referenceDict = new Dictionary<Type, object>();

    private static Game? _gameInstance;

    public UIManager UI;

    private WindowInfo windowInfo = new WindowInfo();

    protected Game()
    {
        UI = RegisterComponent(new UIManager());
    }

    public void SetWindowInfo(WindowInfo windowInfo)
    {
        this.windowInfo = windowInfo;
    }

    public K RegisterComponent<K>(K reference) where K : notnull
    {
        referenceDict.Add(typeof(K), reference);

        return reference;
    }

    public K? GetComponent<K>() where K : notnull
    {
        if (referenceDict.TryGetValue(typeof(K), out var reference))
        {
            return (K)reference;
        }

        return default(K);
    }

    public void Run()
    {
        Raylib.SetConfigFlags(ConfigFlags.InterlacedHint);

        Raylib.InitWindow(this.windowInfo.Width, this.windowInfo.Height, this.windowInfo.Title);
        Raylib.SetTargetFPS(30);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();

        Random random = RegisterComponent(new Random());
        random.Next();
    }

    public static Game GetOrCreate()
    {
        if (_gameInstance != null)
            return _gameInstance;

        Game _game = new Game();
        _gameInstance = _game;

        return _game;
    }

    public void Dispose(){}
}

public struct WindowInfo
{
    public int Width;
    public int Height;

    public int TargetFPS;

    public string Title;

    public WindowInfo(int Width = 720, int Height = 720, int TargetFPS = 30, string Title = "Pipboy2K Engine")
    {
        this.Width = Width;
        this.Height = Height;
        this.TargetFPS = TargetFPS;
        this.Title = Title;
    }
}