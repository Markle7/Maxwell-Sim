using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maxwell_Sim
{
    public static class MyExtensions
    {
        public static RectangleF MapRectangle(this RectangleF rect, Vector2 multiplier)
        {
            return new RectangleF(rect.X * multiplier.X, rect.Y * multiplier.Y, rect.Width * multiplier.X, rect.Height * multiplier.Y);
        }
        public static RectangleF MapRectangle(this Rectangle rect, Vector2 multiplier)
        {
            return new RectangleF(rect.X * multiplier.X, rect.Y * multiplier.Y, rect.Width * multiplier.X, rect.Height * multiplier.Y);
        }
        public static RectangleF MapRectangle(this RectangleF rect, Point multiplier)
        {
            return new RectangleF(rect.X * multiplier.X, rect.Y * multiplier.Y, rect.Width * multiplier.X, rect.Height * multiplier.Y);
        }
        public static Vector2 Inverse(this Vector2 p)
        {
            return new Vector2(1.0f / p.X, 1.0f / p.Y);
        }

        public static Vector2 GetOrigin(this ButtonAlign align)
        {
            switch (align)
            {
                case ButtonAlign.TopLeft:
                    return new Vector2(0, 0);
                case ButtonAlign.TopRight:
                    return new Vector2(1, 0);
                case ButtonAlign.BottomLeft:
                    return new Vector2(0, 1);
                case ButtonAlign.BottomRight:
                    return new Vector2(1, 1);
                case ButtonAlign.Centered:
                    return new Vector2(0.5f, 0.5f);
                default:
                    return new Vector2(0, 0);
            }
        }
    }
}
