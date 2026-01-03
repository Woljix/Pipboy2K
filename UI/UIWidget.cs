using Raylib_cs;
namespace Pipboy2K.UI;

public abstract class UIWidget
{
    // Area that the widget occupies, and is allowed to draw in.
    //public Raylib_cs.Rectangle _bounds;

    public GameContainer GC;

    // TODO: Handle "Bounds" calculation once, then store it. Update it on resize events.

    public UIWidget(GameContainer gc)
    {
        GC = gc;
    }

    public virtual void Update() { }

    public virtual void Prerender() { }

    public virtual void Render() { }

    public Rectangle Bounds
    {
        get { return GC.Bounds;}
        set { GC.Bounds = value; }
    }
}