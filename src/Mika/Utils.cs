using Microsoft.Xna.Framework;

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
    }
}
