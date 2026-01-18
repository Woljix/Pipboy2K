using Raylib_cs;
using Pipboy2K.Engine.Core;
using System.Numerics;
using Pipboy2K.Util;
using Lua;

namespace Pipboy2K.UI;

public enum UIWidgetType
{
    /// <summary>
    /// Does not need interaction. Will not be handed control over input.
    /// </summary>
    Static,
    /// <summary>
    /// Requires interaction. Will be given input
    /// </summary>
    Interactable
}

public abstract class Transform
{
    public Vector2 _localPosition;

    public Vector2 LocalPosition;

    public Vector2 GlobalPosition;

    public Transform()
    {

    }

    internal float lol()
    {
        return 0.0f;
    }
}


/*
 Born: TBD()
 GameLoop: Update() -> Invalidate() -> Paint()
 Die: TBD()
 */
public abstract class UIWidget : RawSprite
{
    public struct LayoutStyle
    {

    }

    internal UIManager UI;

    #region Widget Defines

    public UIWidgetType WidgetType;

    public UIWidget? Parent { get; protected set; }
    public List<UIWidget> Children { get; private set; }
    #endregion

    // TODO: Transform should be a struct (i think?) that only should be calculated once when needed.
    // If nothing changes - do not do expensive math.

    #region Transform
    private Vector2 LocalPosition { get { return SpritePosition; } set { SpritePosition = value; } }
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

    /// <summary>
    /// Returns the bounding box of the widget, and where it is on screen.
    /// </summary>
    public Rectangle BoundingBox
    {
        get
        {
            if (Children.Count > 0)
            {
                var bb = new BB();

                Children.ForEach(x => bb.Calculate([x.BoundingBox]));

                bb.Calculate([new Rectangle(Position, SpriteSize)]);

                return bb.GetBB();
            }

            return new Rectangle(Position, SpriteSize);
        }
    }

    /// <summary>
    /// Gets the bounds where the widget is allowed to render at.
    /// Will use parent's bound if available, else use screen bounds.
    /// </summary>
    public Rectangle ScreenBounds
    {
        get
        {
            return Parent != null ? Parent.BoundingBox : new Rectangle(0, 0, GS.ScreenBounds);
        }
    }

    #endregion

    #region Layout
    public bool UseAnchorPoint = false;

    /// <summary>
    /// Requires UseAnchorPoint = true
    /// </summary>
    public Vector2 AnchorPoint = new Vector2(0.5f, 0.5f);

    /// <summary>
    /// Requires UseAnchorPoint = true
    /// </summary>
    public Vector2 PivotPoint = new Vector2(0, 1.0f);

    #endregion



    // Area that the widget occupies, and is allowed to draw in.
    //public Raylib_cs.Rectangle _bounds;

    public UIWidget(UIWidgetType widgetType = UIWidgetType.Static)
    {
        this.WidgetType = widgetType;

        Children = new List<UIWidget>();

        Prepare();
        Invalidate(this);
    }

    public void GetEvent()
    {
        //Parent.Invalidate(this);
    }

    // probably a temp function, to just check if something is not valid anymore.
    protected void Invalidate(UIWidget widget)
    {
        active = false; // Reset min & max (TODO: This does not have to happen every frame, only when something changes.)

        if (UseAnchorPoint)
        {
            var localPivot = SpriteSize * PivotPoint;

            // Get Anchored Location                Offset with _position
            var ancPos = (ScreenBounds.Size * AnchorPoint) - localPivot;

            // Why the fuck do i need to do this? Did i accidentally invert the world and local positions?
            //LocalPosition = ancPos;

            Position = ancPos;
        }

        OnInvalidate();
        //Queue<string> lol = new Queue<string>();
    }

    public virtual void OnInvalidate() { }

    public virtual void ProcessEvent() { }

    public virtual void Update() { }

    public virtual void Paint() { }

    protected override void Draw()
    {
        Invalidate(this);
        Paint();

        foreach (var child in Children)
            child.Draw();

        if (GS.DebugMode)
            if (Children.Count > 0 && Parent == null)
                Raylib.DrawRectangleLinesEx(BoundingBox, 2, Color.Yellow);
    }

    public void AddChild(UIWidget widget)
    {
        // Check for recursiveness or something..
        widget.Parent = this;
        Children.Add(widget);
    }
}