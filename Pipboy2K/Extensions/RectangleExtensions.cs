using System;
using System.Collections.Generic;
using System.Numerics;

namespace Raylib_cs
{
    public static class RectangleExtensions
    {
        public static Rectangle GetBoundingBox(IEnumerable<Rectangle> rectangles)
        {
            // Initialize with extreme opposite values so the first rectangle overwrites them
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            bool isEmpty = true;

            foreach (var rect in rectangles)
            {
                isEmpty = false;

                // Find left-most and top-most points
                if (rect.X < minX) minX = rect.X;
                if (rect.Y < minY) minY = rect.Y;

                // Find right-most and bottom-most points
                float rectRight = rect.X + rect.Width;
                float rectBottom = rect.Y + rect.Height;

                if (rectRight > maxX) maxX = rectRight;
                if (rectBottom > maxY) maxY = rectBottom;
            }

            // Handle case where list was empty
            if (isEmpty) return new Rectangle(0, 0, 0, 0);

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        // Overload to merge just two rectangles
        public static Rectangle Union(this Rectangle r1, Rectangle r2)
        {
            float x = Math.Min(r1.X, r2.X);
            float y = Math.Min(r1.Y, r2.Y);
            float right = Math.Max(r1.X + r1.Width, r2.X + r2.Width);
            float bottom = Math.Max(r1.Y + r1.Height, r2.Y + r2.Height);

            return new Rectangle(x, y, right - x, bottom - y);
        }
    }
}