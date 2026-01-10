using System.Numerics;
using Raylib_cs;

namespace Pipboy2K.UI;

public class UIManager
{
    public List<UIWidget> Widgets {get; private set; } = new List<UIWidget>();

    public event Action<Vector2>? OnResize;

    public UIManager() { }

    public void AddWidget(UIWidget widget)
    {
        Widgets.Add(widget);
    }

    public void Update()
    {
        foreach (UIWidget widget in Widgets)
        {
            widget.Update();
        }
    }

    public void Render()
    {
        foreach (UIWidget widget in Widgets)
        {
            widget.AttemptDraw();
        }
    }

    public void HandleResize()
    {
        //GC.Bounds = new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

        //foreach (var widget in widgets)
            //widget.Invalidate();
    }
}

public class UIManagerBuilder
{
    private UIManager uiManager;

    public UIManagerBuilder()
    {
        uiManager = new UIManager();
    }

    public UIManager Build()
    {
        return uiManager;
    }
}