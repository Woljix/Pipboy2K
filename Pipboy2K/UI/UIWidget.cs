using Raylib_cs;
using Pipboy2K.Engine.Core;
using System.Numerics;

namespace Pipboy2K.UI;

public abstract class UIWidget : RawSprite
{
    public UIWidget? Parent { get; private set; }
    public List<UIWidget> Children { get; private set; }

    private Vector2 LocalPosition { get { return SpritePosition; } set { SpritePosition = value; } }

    public bool UseAnchorPoint = false;

    public Vector2 AnchorPoint = new Vector2(0.5f, 0.5f);
    public Vector2 PivotPoint = new Vector2(0, 1.0f);

    /// <summary>
    /// Global Position (Parent's Position + Local Position)
    /// </summary>
    public Vector2 Position
    {
        get
        {
            Vector2 _pos = (Parent != null ? Parent.Position : Vector2.Zero) + LocalPosition;

            return _pos;
        }
        set
        {
            if (Parent == null)
            {
                LocalPosition = value;
            }
            else
            {
                LocalPosition = Parent.Position - value;
            }
        }
    }

    public Rectangle Rect
    {
        get
        {
            return new Rectangle(Position, SpriteSize);
        }
    }

    // Area that the widget occupies, and is allowed to draw in.
    //public Raylib_cs.Rectangle _bounds;

    public UIWidget()
    {
        Children = new List<UIWidget>();

        Prepare();
    }

    public void GetEvent()
    {

    }

    // probably a temp function, to just check if something is not valid anymore.
    private void Invalidate()
    {
        if (UseAnchorPoint)
        {
            Rectangle _rect;

            if (Parent != null)
                _rect = Parent.Rect;
            else
                _rect = new Rectangle(0, 0, GC.ScreenBounds);

            var localPivot = SpriteSize * PivotPoint;

            // Get Anchored Location                Offset with _position
            var ancPos = (_rect.Size * AnchorPoint) - localPivot;

            // Why the fuck do i need to do this? Did i accidentally invert the world and local positions?
            //LocalPosition = ancPos;

            Position = ancPos;
        }
    }

    public virtual void ProcessEvent() { }

    public virtual void Update() { }

    public abstract void Paint();

    protected override void Draw()
    {
        Invalidate();
        Paint();

        foreach (var child in Children)
            child.Draw();
    }

    public void AddChild(UIWidget widget)
    {
        // Check for recursiveness or something..
        widget.Parent = this;
        Children.Add(widget);
    }
}