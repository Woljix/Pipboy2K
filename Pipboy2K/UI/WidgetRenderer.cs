using System.Numerics;
using Pipboy2K.Engine.Core;
using Raylib_cs;

namespace Pipboy2K.UI;

/// <summary>
/// Output render transform.
/// This should represent where the Widget is actually drawn on screen.
/// </summary>
public struct RenderTransform
{
    public Vector2 Position;
    public Vector2 Size;

    public float X { get { return Position.X; } }
    public float Y { get { return Position.Y; } }

    public float Width { get { return Size.X; } }
    public float Height { get { return Size.Y; } }

    // public Rectangle BoundingBox
    // {
    //     get; set;
    // }

    public Rectangle RenderBounds { get { return new Rectangle(Position, Size); }}

    public static Vector2 WINDOWSIZE;

    public Vector2 GetWindowSize
    {
        get
        {
            return WINDOWSIZE;
        }
    }

    public RenderTransform(Vector2 Position, Vector2 Size)
    {
        this.Position = Position;
        this.Size = Size;
    }

    public RenderTransform()
    {
        Position = new Vector2();
        Size = new Vector2();
    }

    public override string ToString()
    {
        return $"X:{X} Y:{Y} Width:{Width} Height:{Height}";
    }
}

public class WidgetRenderer : RawSprite
{
    private UIWidget refWidget;

    public bool IsInvalid { get; private set; } = true;

    private RenderTransform _renderTransform;

    public RenderTransform GetRenderTransform
    {
        get
        {
            if (IsInvalid)
                Invalidate(false);

            return _renderTransform;
        }
    }

    public override Vector2 GetSpritePosition { get => _renderTransform.Position; set => _renderTransform.Position = value; }

    public override Vector2 GetSpriteSize { get => _renderTransform.Size; set => _renderTransform.Size = value; }

    //public Vector2 SpritePosition { get => _renderTransform.Position; set => _renderTransform.Position = value; }

    public WidgetRenderer(UIWidget widget)
    {
        this.refWidget = widget;

        _renderTransform = new RenderTransform();
    }

    public void Invalidate(bool includeChildren = true)
    {
        IsInvalid = true;

        //doInvalidate();

        if (!includeChildren)
            return;

        if (refWidget.Children.Count > 0)
            refWidget.Children.ForEach(x => x.renderer.Invalidate());
    }

    public Action<WidgetRenderer, RenderTransform>? OnPaint;

    private void doInvalidate()
    {
        LayoutStyle _layout = refWidget.Layout;

        RenderTransform rTransform = new RenderTransform();

        active = false; // Reset min & max (TODO: This does not have to happen every frame, only when something changes.)

        //Console.WriteLine($"Invalidating: {this.refWidget.GetType().ToString()}");

        Rectangle parentalBounds = refWidget.HasParent ? refWidget.Parent.renderer.GetRenderTransform.RenderBounds : new Rectangle(0,0, GS.ScreenBounds);

        //Console.WriteLine();

        //rTransform.BoundingBox = this.refWidget.transform.CalculateBoundingBox();

        switch (_layout.AutoSize)
        {
            case LayoutStyle.AutoSizeType.Auto:
                rTransform.Size = new Vector2(_localSpritePos.X, _localSpritePos.Y);
                break;

            default:
            case LayoutStyle.AutoSizeType.Manual:
                rTransform.Size = new Vector2(_layout.Width, _layout.Height);
                break;
        }

        switch (_layout.UsePositionType)
        {
            case LayoutStyle.PositionType.AnchorPoint:
                Vector2 localPivot = GetSpriteSize * _layout.PivotPoint;

                Vector2 offsetPos = (parentalBounds.Size * _layout.AnchorPoint) - localPivot;

                // Get Anchored Location                Offset with _position
                var ancPos = parentalBounds.Position + offsetPos;

                //Console.WriteLine($"Type:{refWidget.GetType().ToString()} (ConstrainedBound:{this.refWidget.GetConstrainedBounds.ToString()}) - LocalPivot: {localPivot}, OffsetPos: {offsetPos} WorldPos: {ancPos}");

                // Why the fuck do i need to do this? Did i accidentally invert the world and local positions?
                //LocalPosition = ancPos;

                rTransform.Position = ancPos;

                break;

            case LayoutStyle.PositionType.Manual:
                rTransform.Position = _layout.LocalPosition;
                break;
            case LayoutStyle.PositionType.Auto:
                rTransform.Position = parentalBounds.Position + _layout.LocalPosition;
                break;
        }

        _renderTransform = rTransform;
    }

    protected override void Draw()
    {
        if (IsInvalid)
        {
            IsInvalid = false;
            doInvalidate();
        }


        if (OnPaint != null)
            OnPaint.Invoke(this, this._renderTransform);

        if (GS.DebugMode)
        {
            Raylib.DrawRectangleLinesEx(GetRenderTransform.RenderBounds, 2, Color.Red);
        }

    }
}