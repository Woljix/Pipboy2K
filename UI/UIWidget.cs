using Raylib_cs;
namespace Pipboy2K.UI;

public abstract class UIWidget
{
    // Area that the widget occupies, and is allowed to draw in.
    public Raylib_cs.Rectangle _bounds;

    public UIWidget(ref Rectangle bounds)
    {
        _bounds = bounds;
    }

    // public UIWidget(Rectangle bounds)
    // {
    //     _bounds = bounds;
    // }

    public virtual void Update()
    {

    }

    public virtual void Prerender()
    {

    }

    public virtual void Render()
    {

    }

    public Rectangle Bounds
    {
        get { return _bounds; }
        set { _bounds = value; }
    }
}