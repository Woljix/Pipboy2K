using Raylib_cs;
using Pipboy2K.Engine.Core;
using System.Numerics;
using Pipboy2K.Util;
using Lua;
using System.Security.Cryptography.X509Certificates;



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

public class Transform
{
    public UIWidget? Parent { get; protected set; }
    public List<UIWidget> Children { get; private set; }

    public UIWidget widget { get; internal set; }

    public bool HasParent
    {
        get
        {
            return Parent != null;
        }
    }

    public bool HasChildren
    {
        get
        {
            return Children.Count > 0;
        }
    }

    private Vector2 _localPosition = Vector2.Zero;

    private Vector2 LocalPosition { get { return _localPosition; } set { _localPosition = value; } }
    /// <summary>
    /// Global Position (Parent's Position + Local Position)
    /// </summary>
    public Vector2 Position
    {
        get
        {
            Vector2 _pos = (Parent != null ? Parent.transform.Position : Vector2.Zero) + LocalPosition;

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
                LocalPosition = Parent.transform.Position - value;
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
            return widget.renderer.GetRenderTransform.BoundingBox;
        }
    }

    /// <summary>
    /// Calculates Bounding Box (Expensive!)
    /// </summary>
    /// <returns></returns>

    public Rectangle CalculateBoundingBox()
    {


        //var lol = familyTree.SelectMany(x => x.transform).ToList();

        //bb.Calculate(familyTree)

        if (HasChildren && !HasParent)
        {
            var bb = new BB();

            var familyTree = WidgetExtensions.GetAllDescendants(this.widget).ToList();

            List<Rectangle> rectangles = new List<Rectangle>();

            familyTree.ForEach(x => rectangles.Add(new Rectangle(x.renderer.GetSpritePosition, x.renderer.SpriteSize)));

            bb.Calculate([new Rectangle(Position, widget.renderer.SpriteSize)]);

            var box = bb.GetBB();

            Console.WriteLine("BB: " + box.ToString());

            return box;
        }

        return new Rectangle(Position, widget.renderer.SpriteSize);
    }

    public List<UIWidget> GetFamilyTree()
    {
        return Children.SelectMany(x => x.transform.Children).ToList();
    }

    /// <summary>
    /// Gets the bounds where the widget is allowed to render at.
    /// Will use parent's bound if available, else use screen bounds.
    /// </summary>
    public Rectangle ScreenBounds
    {
        get
        {
            return Parent != null ? Parent.transform.BoundingBox : new Rectangle(0, 0, GS.ScreenBounds);
        }
    }

    public float Width;
    public float Height;

    public Transform(UIWidget widget)
    {
        this.widget = widget;

        Children = new List<UIWidget>();
    }

    public void AddChild(UIWidget widget)
    {
        // Check for recursiveness or something..
        widget.transform.Parent = this.widget;
        Children.Add(widget);
    }
}

public static class WidgetExtensions
{
    public static IEnumerable<UIWidget> GetAllDescendants(this UIWidget widget)
    {
        if (widget.transform.Children == null) yield break;

        // Use a stack for Depth-First Search (DFS)
        var stack = new Stack<UIWidget>(widget.transform.Children);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            yield return current;

            // Add children to the stack to process them next
            foreach (var child in current.transform.Children)
            {
                stack.Push(child);
            }
        }
    }
}


public struct LayoutStyle
{
    public enum AutoSizeType
    {
        Manual,
        Auto,
    }

    public enum PositionType
    {
        Manual,
        Auto,
        AnchorPoint
    }

    // SCALE

    public AutoSizeType AutoSize = AutoSizeType.Auto;

    public float Width;

    public float Height;

    // POSITION

    public PositionType UsePositionType = PositionType.Auto;

    /// <summary>
    /// Requires UseAnchorPoint = true
    /// </summary>
    public Vector2 AnchorPoint = new Vector2(0.5f, 0.5f);

    /// <summary>
    /// Requires UseAnchorPoint = true
    /// </summary>
    public Vector2 PivotPoint = new Vector2(0, 1.0f);

    public Vector2 Position;

    public LayoutStyle() { }

    public static LayoutStyle Auto()
    {
        return new LayoutStyle()
        {

        };
    }
}


/*
 Born: TBD()
 GameLoop: Update() -> Invalidate() -> Paint()
 Die: TBD()
 */
public abstract class UIWidget
{
    public Transform transform;
    public WidgetRenderer renderer;
    public LayoutStyle Layout;

    //internal UIManager UI;

    #region Widget Defines

    public UIWidgetType WidgetType;

    #endregion

    // TODO: Transform should be a struct (i think?) that only should be calculated once when needed.
    // If nothing changes - do not do expensive math.

    #region Transform

    #endregion

    // Area that the widget occupies, and is allowed to draw in.
    //public Raylib_cs.Rectangle _bounds;

    public UIWidget(UIWidgetType widgetType = UIWidgetType.Static)
    {
        this.WidgetType = widgetType;
        this.Layout = new LayoutStyle();

        this.transform = new Transform(this);

        this.renderer = new WidgetRenderer(this);
        this.renderer.OnPaint += Draw;

        //Prepare();
        //Invalidate(this);
    }

    public virtual void OnInvalidate() { }

    public virtual void ProcessEvent() { }

    public virtual void Update() { }

    public virtual void Paint() { }

    protected abstract void Draw(WidgetRenderer e, RenderTransform transform);

    public void PaintFamily()
    {
        renderer.AttemptDraw();

        foreach (var child in transform.Children)
        {
            child.PaintFamily();
        }
    }
}