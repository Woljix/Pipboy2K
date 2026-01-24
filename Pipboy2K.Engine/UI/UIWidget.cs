using Raylib_cs;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

using Pipboy2K.Engine.Core;
using Pipboy2K.Engine.UI.Internal;

namespace Pipboy2K.Engine.UI;

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


    public UIWidget widget { get; internal set; }



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

    // public Rectangle CalculateBoundingBox()
    // {
    //     Console.WriteLine("CalculateBoundingBox()");

    //     //var lol = familyTree.SelectMany(x => x.transform).ToList();

    //     //bb.Calculate(familyTree)

    //     if (HasChildren && !HasParent)
    //     {
    //         //var bb = new BB();

    //         var familyTree = WidgetExtensions.GetAllDescendants(this.widget).ToList();

    //         List<Rectangle> rectangles = new List<Rectangle>();

    //         Console.WriteLine("#####");

    //         familyTree.ForEach(x =>
    //         {
    //             var _rect = new Rectangle(x.renderer.GetSpritePosition, x.renderer.SpriteSize);

    //             rectangles.Add(_rect);
    //             Console.WriteLine(_rect.ToString());

    //         });

    //         rectangles.Add(new Rectangle(Position, widget.renderer.SpriteSize));

    //         var box = RectangleExtensions.GetBoundingBox(rectangles);
    //         //bb.Calculate(rectangles.ToArray());

    //         Console.WriteLine("#####");

    //         //var box = bb.GetBB();

    //         Console.WriteLine("BB: " + box.ToString());

    //         return box;
    //     }

    //     return new Rectangle(this.widget.renderer.GetSpritePosition, this.widget.renderer.GetRenderTransform.Size);
    // }



}

public static class WidgetExtensions
{
    public static IEnumerable<UIWidget> GetAllDescendants(this UIWidget widget)
    {
        if (widget.Children == null) yield break;

        // Use a stack for Depth-First Search (DFS)
        var stack = new Stack<UIWidget>(widget.Children);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            yield return current;

            // Add children to the stack to process them next
            foreach (var child in current.Children)
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
    public readonly Vector2 TopLeft = new Vector2(0.0f, 0.0f);
    public readonly Vector2 TopMiddle = new Vector2(0.5f, 0.0f);
    public readonly Vector2 TopRight = new Vector2(1f, 0.0f);

    public readonly Vector2 CenterLeft = new Vector2(0.0f, 0.5f);
    public readonly Vector2 CenterMiddle = new Vector2(0.5f, 0.5f);
    public readonly Vector2 CenterRight = new Vector2(1f, 0.5f);

    public readonly Vector2 BottomLeft = new Vector2(0f, 1f);
    public readonly Vector2 BottomMMiddle = new Vector2(0.5f, 1f);
    public readonly Vector2 BottomRight = new Vector2(1f, 1f);

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
        layoutStyle.AutoSize = LayoutStyle.AutoSizeType.Manual;

        layoutStyle.Width = size.X;
        layoutStyle.Height = size.Y;

        return this;
    }

    public LayoutStyleBuilder SizeManual(float width, float height)
    {
        return SizeManual(new Vector2(width, height));
    }

    public LayoutStyle Build()
    {
        return layoutStyle;
    }

    public static LayoutStyleBuilder Begin()
    {
        return new LayoutStyleBuilder();
    }

    public static LayoutStyleBuilder From(LayoutStyle initial)
    {
        LayoutStyleBuilder _lsb = new LayoutStyleBuilder()
        {
            layoutStyle = initial
        };

        return _lsb;
    }
}

public struct LayoutStyleAbsolutes
{
    public float MinWidth = float.NaN;
    public float MaxWidth = float.NaN;

    public float MinHeight = float.NaN;
    public float MaxHeight = float.NaN;

    public LayoutStyleAbsolutes()
    {

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
    public UIWidget? Parent { get; internal set; }
    public List<UIWidget> Children { get; internal set; }

    public WidgetRenderer renderer;
    public LayoutStyle Layout;

    // Add absolutes that the widgets can set themselves. (E.g if a widget needs to draw with a width of 40px, there is no need to set it any higher.)
    //public LayoutStyleAbsolutes LayoutAbsolute;

    // Will be initialized by the main UIManager, or by the parent.
    public UIManager UI { get; internal set; }

    #region Widget Defines
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
    public Vector2 GetPosition
    {
        get
        {
            //Vector2 _pos = (Parent != null ? Parent.Position : Vector2.Zero) + LocalPosition;

            //return _pos;

            Vector2 _pos = (Parent != null) ? Parent.renderer.GetRenderTransform.Position : Vector2.Zero;

            return _pos;
        }
    }
    public UIWidgetType WidgetType;

    public bool Enabled {get; set;} = true;

    public bool Visible {get; set;} = true;

    #endregion

    // TODO: Transform should be a struct (i think?) that only should be calculated once when needed.
    // If nothing changes - do not do expensive math.

    #region Transform
    /// <summary>
    /// Gets the bounds where the widget is allowed to render at.
    /// Will use parent's bound if available, else use screen bounds.
    /// </summary>
    // public Rectangle GetConstrainedBounds
    // {
    //     get
    //     {
    //         //return Parent != null ? Parent.renderer.GetRenderTransform.RenderBounds : new Rectangle(0, 0, GS.ScreenBounds);

    //         if (Parent != null)
    //         {
    //             if (Parent.renderer.IsInvalid)
    //             {
    //                 Parent.renderer.Invalidate(false);
    //             }

    //             return Parent.renderer.GetRenderTransform.RenderBounds;
    //         }

    //         return new Rectangle(0, 0, GS.ScreenBounds);
    //     }
    // }
    #endregion

    // Area that the widget occupies, and is allowed to draw in.
    //public Raylib_cs.Rectangle _bounds;

    public UIWidget()
    {
        this.WidgetType = UIWidgetType.Static;
        this.Layout = new LayoutStyle();

        this.renderer = new WidgetRenderer(this);
        this.renderer.OnPaint += Draw;

        this.Children = new List<UIWidget>();

        //Prepare();
        //Invalidate(this);
    }


    public virtual void Initialize() { }

    [Obsolete]
    public virtual void OnInvalidate() { }

    public virtual void ProcessEvent() { }

    public virtual void OnStart() { }

    public virtual void Update() { }

    protected virtual void OnDestroy() { }

    /// <summary>
    /// Will remove the widget as a child of the parent (if applicable), and will destory
    /// </summary>
    public void Destroy()
    {
        // Before or after removing references?
        OnDestroy();

        if (HasParent)
            Parent!.Children.Remove(this);

        if (HasChildren)
            Children.ForEach(x => x.Destroy());

    }

    //public virtual void Paint() { }

    protected abstract void Draw(WidgetRenderer e, RenderTransform transform);

    public void AddChild(UIWidget widget)
    {
        UI.AddWidget(widget, this);
        // Check for recursiveness or something..
        //widget.Parent = this;
        //widget.UI = this.UI;
        //Children.Add(widget);
    }

    public void PaintFamily()
    {
        if (Visible)
            renderer.AttemptDraw();

        foreach (var child in Children)
        {
            child.PaintFamily();
        }
    }
}