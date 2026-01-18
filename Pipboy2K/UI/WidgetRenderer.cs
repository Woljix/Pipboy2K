using System.Numerics;
using Pipboy2K.Engine.Core;
using Raylib_cs;

namespace Pipboy2K.UI;

/// <summary>
/// Output render transform.
/// </summary>
public struct RenderTransform
{
    public Vector2 Position;
    public Vector2 Size;

    public float X { get {return Position.X;}}
    public float Y { get {return Position.Y;}}

    public float Width { get { return Size.X;}}
    public float Height { get { return Size.Y;}}

    public Rectangle BoundingBox
    {
        get; set;
    }

    public Rectangle RenderBounds;

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
    UIWidget refWidget;

    private bool IsInvalid = true;

    private RenderTransform _renderTransform;

    public RenderTransform GetRenderTransform
    {
        get { return _renderTransform;}
    }

    public override Vector2 GetSpritePosition { get => _renderTransform.Position; set => _renderTransform.Position = value; }

    //public Vector2 SpritePosition { get => _renderTransform.Position; set => _renderTransform.Position = value; }

    public WidgetRenderer(UIWidget widget)
    {
        this.refWidget = widget;

        _renderTransform = new RenderTransform();
    }

    public void Invalidate()
    {
        IsInvalid = true;

        if (refWidget.transform.Children.Count > 0)
            refWidget.transform.Children.ForEach(x => x.renderer.Invalidate());

        // CALCULATE
    }

    public Action<WidgetRenderer, RenderTransform>? OnPaint;

    protected override void Draw()
    {
        if (IsInvalid)
        {
            IsInvalid = false;

            Console.WriteLine("REDRAWING!");

            LayoutStyle _layout = refWidget.Layout;

            RenderTransform rTransform = new RenderTransform();

            active = false; // Reset min & max (TODO: This does not have to happen every frame, only when something changes.)

            rTransform.RenderBounds = this.refWidget.transform.ScreenBounds;

            rTransform.BoundingBox = this.refWidget.transform.CalculateBoundingBox();

            switch (_layout.UsePositionType)
            {
                case LayoutStyle.PositionType.AnchorPoint:
                    var localPivot = SpriteSize * _layout.PivotPoint;

                    // Get Anchored Location                Offset with _position
                    var ancPos = (rTransform.RenderBounds.Size * _layout.AnchorPoint) - localPivot;

                    // Why the fuck do i need to do this? Did i accidentally invert the world and local positions?
                    //LocalPosition = ancPos;

                    rTransform.Position = ancPos;
                break;

                case LayoutStyle.PositionType.Manual:
                case LayoutStyle.PositionType.Auto:
                    rTransform.Position = refWidget.transform.Position;
                break;
            }

            switch (_layout.AutoSize)
            {
                case LayoutStyle.AutoSizeType.Auto:
                    rTransform.Size = new Vector2(this.SpriteSize.X, this.SpriteSize.Y);
                break;

                default:
                case LayoutStyle.AutoSizeType.Manual:
                    rTransform.Size = new Vector2(_layout.Width, _layout.Height);
                break;
            }
            _renderTransform = rTransform;

        }

        if (OnPaint != null)
            OnPaint.Invoke(this, this._renderTransform);

        if (GS.DebugMode)
            Raylib.DrawRectangleLinesEx(SpriteRect, 2, Color.Red);
    }
}