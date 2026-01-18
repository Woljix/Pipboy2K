using System.Numerics;
using Raylib_cs;

namespace Pipboy2K.Util;

public static class UIUtil
{
    /// <summary>
    ///
    /// </summary>
    /// <returns>(min,max)</returns>

}

public class BB
{
    Vector2 min = Vector2.Zero;
    Vector2 max = Vector2.Zero;

    void Expand(Vector2 point)
    {

        min.X = MathF.Min(min.X, point.X);
        min.Y = MathF.Min(min.Y, point.Y);
        max.X = MathF.Max(max.X, point.X);
        max.Y = MathF.Max(max.Y, point.Y);
    }

    public void Calculate(Rectangle[] rects)
    {
        foreach (Rectangle _rect in rects)
        {
            Expand(_rect.Position);
            Expand(new(_rect.X + _rect.Width, _rect.Y + _rect.Height));
        }
    }

    public Rectangle GetBB()
    {
        return new Rectangle(min.X, min.Y, max - min);
    }
}