using System.Drawing;
using System.Numerics;
using Raylib_cs;
using Rectangle = Raylib_cs.Rectangle;
using Color = Raylib_cs.Color;

namespace Pipboy2K.Engine.Core;

// Pseudo Sprite class
/*
04.01.2026: As this is currently implemented, it is a "primitive renderer", if and when i have need for sprites, i don't want the sprites to function like this.
So it would probably be better to make subclasses based on Entity that handles the rendering a little bit differently. Also actual sprites should be able to keep track of animation frames, which is something that "primitives" probably wont need.
Update() function is also needed, that runs first, either at a constant set speed, or every frame.

*/
public abstract class RawSprite
{
    private Vector2 min = Vector2.Zero;
    private Vector2 max = Vector2.Zero;
    private bool active = false;
    private bool _ready = false;

    public Vector2 SpriteSize {get; private set; } = Vector2.Zero;
    public Vector2 SpritePosition = Vector2.Zero;

    public Rectangle SpriteRect { get { return new Rectangle(SpritePosition, SpriteSize); }}

    //public Rectangle SizeRect = new Rectangle();

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

        SpriteSize = max - min;
    }

    public void DrawLineEx(Vector2 startPos, Vector2 endPos, float thick, Color color)
    {
        Expand(startPos + new Vector2(-thick / 2, -thick / 2));
        Expand(endPos + new Vector2(-thick / 2, -thick / 2));
        if (_ready)
            Raylib.DrawLineEx(SpritePosition + startPos, SpritePosition + endPos, thick, color);
    }

    public void DrawRectangleRec(Rectangle rec, Color color)
    {
        Rectangle _rec = new Rectangle(SpritePosition + rec.Position, rec.Size);
        Expand(new Vector2(rec.X, rec.Y));
        Expand(new Vector2(rec.X + rec.Width, rec.Y + rec.Height));
        if (_ready)
            Raylib.DrawRectangleRec(_rec, color);
    }

    public void DrawRectangle(int x, int y, int width, int height, Color color)
    {
        // Don't add local position to this!
        this.DrawRectangleRec(new Rectangle(x, y, width, height), color);
    }

    public void DrawTextEx(Font font, string text, Vector2 position, float fontSize, float spacing, Color tint)
    {
        Vector2 textSize = Raylib.MeasureTextEx(font, text, fontSize, spacing);
        Expand(position);
        Expand(position + textSize);
        if (_ready)
            Raylib.DrawTextEx(font, text, SpritePosition + position, fontSize, spacing, tint);
    }

    public void DrawTextureRec(Texture2D texture, Rectangle source, Vector2 position, Color tint)
    {
        Expand(new (position.X, position.Y));
        Expand(new (position.X + source.Width, position.Y + source.Height));
        Raylib.DrawTextureRec(texture, source, SpritePosition + position, tint);
    }

    public void DrawTexture()
    {

    }

    public void DrawTextureEx(Texture2D texture, Vector2 position, float rotation, float scale, Color tint)
    {
        // TODO
        // Fuck i don't want to do that kind of math man..
        Raylib.DrawTextureEx(texture, position, rotation, scale, tint);
    }

    //public Rectangle RenderRect { get { return new Rectangle(RenderPosition, Gfx.Size);}}

   // private bool _isValidated = false;

    //private bool _renderTextureMade = false;

    //private RenderTexture2D renderTexture;

    public RawSprite() {}

    protected virtual void Predraw() { }

    protected abstract void Draw();

    public void AttemptDraw()
    {
        // Do something else here?
        Draw();

        if (GC.DebugMode)
            Raylib.DrawRectangleLinesEx(SpriteRect, 2, Color.Red);
    }

    // Might not be needed
    public void Prepare()
    {
        AttemptDraw();
        _ready = true;
    }

    /*

    /// <summary>
    /// WIP
    /// Draws the logic from Draw() onto a RenderTexture2D
    /// Will only draw if texture needs to be redrawn. E.g. if Entity state changes, or the physical screen size changes.
    /// </summary>

    // TODO 04.01.2026: Possibly split this into multiple functions? Screen changes arent handled yet.
    public void AttemptDraw()
    {
        if (!_isValidated || !_renderTextureMade)
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

        Raylib.ClearBackground(Color.Black);
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

        Rectangle _rect = new Rectangle(0, 0, renderTexture.Texture.Width, -renderTexture.Texture.Height);

        Raylib.DrawTextureRec(renderTexture.Texture, _rect, Position, Color.White);
        Raylib.DrawRectangleLines((int)Position.X, (int)Position.Y, (int)_rect.Width, (int)-_rect.Height, Color.Red);
    }

    public void DrawAndRender()
    {
        AttemptDraw();
        OutputRender();
    }

    */
}

/// <summary>
/// Allows the creation sprites on the fly.
/// </summary>
public class CanvasSprite : RawSprite
{
    private Action<CanvasSprite>? OnDraw;

    public CanvasSprite() { }

    public CanvasSprite(Action<CanvasSprite> OnDraw) => SetDraw(OnDraw);

    public void SetDraw(Action<CanvasSprite> OnDraw) => this.OnDraw = OnDraw;

    protected override void Draw()
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

    protected override void Draw()
    {

    }
}

// Alt name: Gfx?
public class EntityRenderer
{

}