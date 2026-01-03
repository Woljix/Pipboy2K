using Raylib_cs;

namespace Pipboy2K.UI;

public class UIManager
{
    private List<UIWidget> widgets = new List<UIWidget>();

    private GameContainer GC;

    public UIManager(GameContainer gameContainer)
    {
        GC = gameContainer;
    }

    public async void Initialize()
    {

    }

    public void AddWidget(UIWidget widget)
    {
        widgets.Add(widget);
    }

    public async void Update()
    {
        foreach (UIWidget widget in widgets)
        {
            widget.Update();
        }
    }

    public async void Render()
    {
        foreach (UIWidget widget in widgets)
        {
            widget.Render();
        }
    }
}

public class UIManagerBuilder
{
    private UIManager uiManager;

    public UIManagerBuilder(GameContainer gc)
    {
        uiManager = new UIManager(gc);
    }

    public UIManager Build()
    {
        return uiManager;
    }
}