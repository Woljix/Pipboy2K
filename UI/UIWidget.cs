using Raylib_cs;
namespace Pipboy2K.UI;

public abstract class UIWidget
{
    // Area that the widget occupies, and is allowed to draw in.
    //public Raylib_cs.Rectangle _bounds;

    public GameContainer GC;

    public UIWidget(GameContainer gc)
    {
        GC = gc;
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
        get { return GC.Bounds;}
        set { GC.Bounds = value; }
    }
}