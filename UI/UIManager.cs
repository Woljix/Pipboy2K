using Raylib_cs;

namespace Pipboy2K.UI;

public class UIManager
{
    private List<UIWidget> widgets = new List<UIWidget>();

    private Rectangle _screenBounds;

    public UIManager(ref Rectangle screenBounds)
    {
        _screenBounds = screenBounds;
    }

    public async void Initialize()
    {

    }

    public void AddWidget(UIWidget widget)
    {
        widgets.Add(widget);
    }

    public async void Process()
    {
        foreach (UIWidget widget in widgets)
        {
            widget.Update();
            widget.Render();
        }
    }
}

public class UIManagerBuilder
{
    private UIManager uiManager;

    public UIManagerBuilder(ref Rectangle screenBounds)
    {
        uiManager = new UIManager(ref screenBounds);
    }

    public UIManager Build()
    {
        return uiManager;
    }
}