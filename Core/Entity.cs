using System.Drawing;
using System.Numerics;
using Raylib_cs;
using Rectangle = Raylib_cs.Rectangle;
using Color = Raylib_cs.Color;

namespace Pipboy2K.Core;

// Pseudo Sprite class
public class Entity
{
    /// <summary>
    /// Global Position
    /// </summary>
    public Vector2 Position = Vector2.Zero;

    /// <summary>
    /// The bounds of the drawn entity.
    /// </summary>
    public Rectangle Rect = new Rectangle(0, 0, 0, 0);

    // Might not need this.
    public Rectangle Bounds = new Rectangle(0, 0, 0, 0);

    public RenderTexture2D Texture;

    public Entity()
    {
        //Id = IdGenerator.GetNextId();
    }
}

public class EntityRenderer
{
    Vector2 min = Vector2.Zero;
    Vector2 max = Vector2.Zero;
    bool active = false;

    private EntityRenderer()
    {

    }

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