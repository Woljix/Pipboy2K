using System.Numerics;
using Raylib_cs;

namespace Pipboy2K.Engine.Core;

// TODO: This should ultimately at some point be a UIWidget (or equivelent) at some point. Would make it so that i don't have to add the same code multiple times.
public class UIManager
{
    //public IGameState GS { get; private set; }

    private List<UIWidget> Widgets { get; set; } = new List<UIWidget>();

    public System.Collections.ObjectModel.ReadOnlyCollection<UIWidget> GetWidgetsReadOnly { get { return Widgets.AsReadOnly(); }}

    public event Action<Vector2>? OnResize;

    // Widgets in the queue will get deleted on the next available tick (before Update() & Draw())
    private Queue<UIWidget> _widgetMarkedForDeletetion;

    private bool _started = false;

    #region Config
    public UIManagerConfig Config { get; private set; }
    public bool DebugMode = false;
    #endregion

    public UIManager(UIManagerConfig? config = null)
    {
        _widgetMarkedForDeletetion = new Queue<UIWidget>();

        Config = config ?? new UIManagerConfig();
        //GS = state;
    }

    // I am putting this on ice for a while..
    // public T Instantiate<T>() where T : UIWidget, new()
    // {
    //     T _widget = new T();
    //     //_widget.UI = this;
    //     AddWidget(_widget);
    //     return _widget;
    // }

    public UIWidget AddWidget(UIWidget widget, UIWidget? parentWidget = null)
    {
        widget.UI = this;

        //widget.Parent = this; // This will be added at some later date.

        if (parentWidget != null)
        {
            widget.Parent = parentWidget;
            parentWidget.Children.Add(widget);
        }
        else
        {
            Widgets.Add(widget);
        }

        widget.Initialize();

        // If UIManager is already running - call OnStart() on the new widget.
        if (_started)
            widget.OnStart();

        return widget;
    }

    public void RemoveWidget(UIWidget widget)
    {
        if (Widgets.Contains(widget))
        {
            if (_started)
            {

            }
            else
            {
                _widgetMarkedForDeletetion.Enqueue(widget);
            }


        }
        else
        {
            Console.WriteLine($"{widget.GetType()} does not exist in Widgets!");
        }
    }

    private void internalRemoveWidget(UIWidget widget)
    {
        widget.Destroy();
        this.Widgets.Remove(widget);
    }

    public bool _ready = false;

    /// <summary>
    /// This is done so that invalidation can be done next frame.
    /// Hacky, but works.
    /// </summary>
    public bool _shouldInvalidate = false;

    /// <summary>
    /// Currently only sends the OnStart() event to the widgets.
    /// </summary>
    public void Start()
    {
        Widgets.ForEach(x => x.OnStart());

        _started = true;
    }

    public void Tick()
    {
        if (!_started) return;

        if (_widgetMarkedForDeletetion.Any())
        {
            while (_widgetMarkedForDeletetion.Count > 0)
            {
                var _delWidget = _widgetMarkedForDeletetion.Dequeue();


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

public struct UIManagerConfig
{
    public Font DefaultFont { get; set; } = Raylib.GetFontDefault();
    public bool DebugMode { get; set; } = false;

    public UIManagerConfig()
    {

    }
}
/*

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
*/