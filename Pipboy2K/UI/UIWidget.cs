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

    // /// <summary>
    // /// Returns the bounding box of the widget, and where it is on screen.
    // /// </summary>
    // public Rectangle BoundingBox
    // {
    //     get
    //     {
    //         return widget.renderer.GetRenderTransform.BoundingBox;
    //     }
    // }

    /// <summary>
    /// Calculates Bounding Box (Expensive!)
    /// </summary>
    /// <returns></returns>

    public Rectangle CalculateBoundingBox()
    {
        Console.WriteLine("CalculateBoundingBox()");

        //var lol = familyTree.SelectMany(x => x.transform).ToList();

        //bb.Calculate(familyTree)

        if (HasChildren && !HasParent)
        {
            //var bb = new BB();

            var familyTree = WidgetExtensions.GetAllDescendants(this.widget).ToList();

            List<Rectangle> rectangles = new List<Rectangle>();

            Console.WriteLine("#####");

            familyTree.ForEach(x =>
            {
                var _rect = new Rectangle(x.renderer.GetSpritePosition, x.renderer.SpriteSize);

                rectangles.Add(_rect);
                Console.WriteLine(_rect.ToString());

            });

            rectangles.Add(new Rectangle(Position, widget.renderer.SpriteSize));

            var box = RectangleExtensions.GetBoundingBox(rectangles);
            //bb.Calculate(rectangles.ToArray());

            Console.WriteLine("#####");

            //var box = bb.GetBB();

            Console.WriteLine("BB: " + box.ToString());

            return box;
        }

        return new Rectangle(this.widget.renderer.GetSpritePosition, this.widget.renderer.GetRenderTransform.Size);
    }

    /// <summary>
    /// Gets the bounds where the widget is allowed to render at.
    /// Will use parent's bound if available, else use screen bounds.
    /// </summary>
    public Rectangle Bounds
    {
        get
        {
            return Parent != null ? Parent.renderer.GetRenderTransform.BoundingBox : new Rectangle(0, 0, GS.ScreenBounds);
        }
    }

    public Rectangle GetRect
    {
        get
        {
            return new Rectangle(Position, Size);
        }
    }

    public Vector2 Size
    {
        get
        {
            return widget.renderer.SpriteSize;
        }
    }

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


// TODO: Make a macrounit, that can define both size/pos either in either pixels, percentage, what-is-needed.
// I am having so much fun bascially creating my own UI engine from scratch :)     <--- smiley of eternal pain and suffering.
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

    #region AutoSize.Manual
    public float Width;

    public float Height;
    #endregion

    // POSITION

    public PositionType UsePositionType = PositionType.Auto;

    #region PositionType.AnchorPoint
    /// <summary>
    /// Requires UseAnchorPoint = true
    /// </summary>
    public Vector2 AnchorPoint = new Vector2(0.5f, 0.5f);

    /// <summary>
    /// Requires UseAnchorPoint = true
    /// </summary>
    public Vector2 PivotPoint = new Vector2(0, 1.0f);

    #endregion

    #region PositionType.Manual

    public Vector2 LocalPosition = Vector2.Zero;

    #endregion

    public LayoutStyle() { }

    public static LayoutStyle Auto()
    {
        return new LayoutStyle()
        {

        };
    }
}

public class LayoutStyleBuilder
{
    private LayoutStyle layoutStyle;

    private LayoutStyleBuilder()
    {
        layoutStyle = new LayoutStyle();
    }

    public LayoutStyleBuilder PositionManual(Vector2 position)
    {
        layoutStyle.UsePositionType = LayoutStyle.PositionType.Manual;
        layoutStyle.LocalPosition = position;

        return this;
    }

    public LayoutStyleBuilder PositionAnchorPoint(Vector2 anchorPoint, Vector2 pivotPoint)
    {
        layoutStyle.UsePositionType = LayoutStyle.PositionType.AnchorPoint;

        layoutStyle.AnchorPoint = anchorPoint;
        layoutStyle.PivotPoint = pivotPoint;

        return this;
    }

    [Obsolete("NOT IMPLEMENTED")]
    public LayoutStyleBuilder PositionAuto()
    {
        layoutStyle.UsePositionType = LayoutStyle.PositionType.Auto;

        return this;
    }

    public LayoutStyleBuilder SizeAuto()
    {
        layoutStyle.AutoSize = LayoutStyle.AutoSizeType.Auto;

        return this;
    }

    public LayoutStyleBuilder SizeManual(Vector2 size)
    {
        layoutStyle.Width = size.X;
        layoutStyle.Height = size.Y;

        return this;
    }

        public LayoutStyleBuilder SizeManual(float width, float height)
    {
        layoutStyle.Width = width;
        layoutStyle.Height = width;

        return this;
    }

    public LayoutStyle Build()
    {
        return layoutStyle;
    }

    public static LayoutStyleBuilder Begin()
    {
        return new LayoutStyleBuilder();
    }
}


/*
 Born: TBD()
 GameLoop: Update() -> Invalidate() -> Paint()
 Die: TBD()
 */

 /*
    Refractoring idea (19.01.2025)
    -Remove 'Transform' and put Parent/Children and all of the Widget stuff back into the main class - use layout for all of the layout-related variables instead.
    -RenderTransform will stay, as that is UI-engine outputs as the 'final-rendering-destination' during Invalidation()
    -Declutter/decouple everything, as right now i've lost track of which transform-related variable goes where, as they tend to reference each other (big headache). It should be just: Input = LayoutStyle => Output = RenderTransform. Thats it.
    -

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