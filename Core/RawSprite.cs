using System.Drawing;
using System.Numerics;
using Raylib_cs;
using Rectangle = Raylib_cs.Rectangle;
using Color = Raylib_cs.Color;

namespace Pipboy2K.Core;

// Pseudo Sprite class
/*
04.01.2026: As this is currently implemented, it is a "primitive renderer", if and when i have need for sprites, i don't want the sprites to function like this.
So it would probably be better to make subclasses based on Entity that handles the rendering a little bit differently. Also actual sprites should be able to keep track of animation frames, which is something that "primitives" probably wont need.
Update() function is also needed, that runs first, either at a constant set speed, or every frame.

*/
public abstract class RawSprite
{
    // Purposely empty rendertexture, used a placeholder until a unique one can be produced.
    private static readonly RenderTexture2D _empty = Raylib.LoadRenderTexture(0, 0);

    private EntityRenderer _renderer = new EntityRenderer();

    public EntityRenderer GetRenderer
    {
        get
        {
            return _renderer;
        }
    }

    /// <summary>
    /// Global Position
    /// </summary>
    public Vector2 Position = Vector2.Zero;

    /// <summary>
    /// The bounds of the drawn entity.
    /// </summary>
    public Rectangle Rect { get { return new Rectangle(Position, GetRenderer.Size);}}

    private bool _isValidated = false;

    private bool _renderTextureMade = false;

    private RenderTexture2D renderTexture;

    public RawSprite() {}

    public abstract void Draw();

    /// <summary>
    /// WIP
    /// Draws the logic from OnDraw() onto a RenderTexture2D
    /// Will only draw if texture needs to be redrawn. E.g. if Entity state changes, or the physical screen size changes.
    /// </summary>

    // TODO 04.01.2026: Possibly split this into multiple functions? Screen changes arent handled yet.
    public virtual void AttemptDraw()
    {
        if (!_renderTextureMade)
        {
            _renderTextureMade = true;

            Console.WriteLine("Making a rendertexture");

            Raylib.BeginTextureMode(_empty);

            //Empty draw to calculate size.
            //OnDraw?.Invoke(_renderer);
            Draw();

            Raylib.EndTextureMode();

            renderTexture = Raylib.LoadRenderTexture((int)_renderer.Size.X, (int)_renderer.Size.Y);
        }

        if (_isValidated)
            return;

        _isValidated = true;

        Console.WriteLine("DRAWING!");

        Raylib.BeginTextureMode(renderTexture);

        Draw();

        Raylib.EndTextureMode();
    }

    /// <summary>
    /// Draws the RenderTexture to the screen.
    /// </summary>
    public void OutputRender()
    {
        if (!_renderTextureMade)
            return;

        Raylib.DrawTextureEx(renderTexture.Texture, this.Position, 0.0f, 1.0f, Color.White);
    }

    public void Invalidate() => _isValidated = false;
    public void SetParent(ref RawSprite parentSprite)
    {
        this.renderTexture = parentSprite.renderTexture;
        _renderTextureMade = true;
    }

    // BAD PRACTISE INCOMING
    public void DrawLineEx(Vector2 startPos, Vector2 endPos, float thick, Color color) => GetRenderer.DrawLineEx(startPos, endPos, thick, color);
    public void DrawRectangleRec(Rectangle rec, Color color) => GetRenderer.DrawRectangleRec(rec, color);
    public void DrawTextEx(Font font, string text, Vector2 position, float fontSize, float spacing, Color tint) => GetRenderer.DrawTextEx(font, text, position, fontSize, spacing, tint);
}

// Allows for making sprites on the fly.
public class CanvasSprite : RawSprite
{
    private Action<CanvasSprite>? OnDraw;

    public CanvasSprite() { }

    public CanvasSprite(Action<CanvasSprite> OnDraw) => SetDraw(OnDraw);

    public void SetDraw(Action<CanvasSprite> OnDraw) => this.OnDraw = OnDraw;

    public override void Draw()
    {
        if (OnDraw == null)
            return;

        OnDraw.Invoke(this);
    }
}

public class AnimatedSprite : RawSprite
{
    // TODO

    public AnimatedSprite()
    {

    }

    public override void Draw()
    {

    }
}

public class EntityRenderer
{
    Vector2 min = Vector2.Zero;
    Vector2 max = Vector2.Zero;
    bool active = false;

    public Vector2 Size = new Vector2(0,0);
    public Rectangle SizeRect = new Rectangle();

    public EntityRenderer(){ }

    private void Expand(Vector2 point)
    {
        if (!active)
        {
            active = true;
            min = point;
            max = point;
            return;
        }

        min.X = MathF.Min(min.X, point.X);
        min.Y = MathF.Min(min.Y, point.Y);
        max.X = MathF.Max(max.X, point.X);
        max.Y = MathF.Max(max.Y, point.Y);

        Size = max - min;
    }

    public void DrawLineEx(Vector2 startPos, Vector2 endPos, float thick, Color color)
    {
        Expand(startPos + new Vector2(-thick / 2, -thick / 2));
        Expand(endPos + new Vector2(-thick / 2, -thick / 2));
        Raylib.DrawLineEx(startPos, endPos, thick, color);
    }

    public void DrawRectangleRec(Rectangle rec, Color color)
    {
        Expand(new Vector2(rec.X, rec.Y));
        Expand(new Vector2(rec.X + rec.Width, rec.Y + rec.Height));
        Raylib.DrawRectangleRec(rec, color);
    }

    public void DrawTextEx(Font font, string text, Vector2 position, float fontSize, float spacing, Color tint)
    {
        Vector2 textSize = Raylib.MeasureTextEx(font, text, fontSize, spacing);
        Expand(position);
        Expand(position + textSize);
        Raylib.DrawTextEx(font, text, position, fontSize, spacing, tint);
    }
}