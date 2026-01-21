using System.Numerics;
using Raylib_cs;

namespace Pipboy2K.UI;

public class UIManager
{
    public List<UIWidget> Widgets { get; private set; } = new List<UIWidget>();

    public event Action<Vector2>? OnResize;

    private Queue<UIWidget> _widgetMarkedForDeletetion;

    public UIManager()
    {
        _widgetMarkedForDeletetion = new Queue<UIWidget>();
    }

    public T Instantiate<T>() where T : UIWidget, new()
    {
        T _widget = new T();
        //_widget.UI = this;
        Widgets.Add(_widget);
        return _widget;
    }

    public void AddWidget(UIWidget widget)
    {
        Widgets.Add(widget);
    }

    public void RemoveWidget(UIWidget widget)
    {
        if (Widgets.Contains(widget))
        {
            _widgetMarkedForDeletetion.Enqueue(widget);
        }
        else
        {
            Console.WriteLine($"{widget.GetType()} does not exist in Widgets!");
        }
    }

    public bool _ready = false;

    /// <summary>
    /// This is done so that invalidation can be done next frame.
    /// Hacky, but works.
    /// </summary>
    public bool _shouldInvalidate = false;

    public void Tick()
    {
        if (_widgetMarkedForDeletetion.Any())
        {
            while (_widgetMarkedForDeletetion.Count > 0)
            {
                var _delWidget = _widgetMarkedForDeletetion.Dequeue();

                this.Widgets.Remove(_delWidget);
            }
        }

        if (_shouldInvalidate)
        {
            Invalidate();
            _shouldInvalidate = false;
        }

        if (Raylib.IsWindowResized() || !_ready)
        {
            _ready = true;
            Console.WriteLine("WINDOW RESIZED!");

            RenderTransform.WINDOWSIZE = new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

            Invalidate();

            _shouldInvalidate = true;
        }

        foreach (UIWidget widget in Widgets)
        {
            if (!widget.Enabled)
                return;

            widget.Update();
        }

        foreach (UIWidget widget in Widgets)
        {
            if (!widget.Enabled)
                return;

            widget.PaintFamily();
        }

    }

    public void Invalidate()
    {
        foreach (UIWidget widget in Widgets)
        {
            widget.renderer.Invalidate();
        }
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