using Microsoft.Xna.Framework;
using System;

namespace Mika
{
    public static class Utils
    {
        public static Point Vec2ToPoint(Vector2 input)
        {
            return new Point((int)input.X, (int)input.Y);
        }

        public static Vector2 PointToVec2(Point input)
        {
            return new Vector2(input.X, input.Y);
        }

        public static Rectangle RectFromPosAndSize(Point pos, Point size)
        {
            return new Rectangle(pos.X, pos.Y, size.X, size.Y);
        }

        public static Rectangle OffsetRect(Rectangle input, Point offset)
        {
            return new Rectangle(input.X + offset.X, input.Y + offset.Y, input.Width, input.Height);
        }

        public static float ClampF(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static int LerpInt(int a, int b, float t)
        {
            return a + (int)(((float)b - a) * t);
        }
    }
}
