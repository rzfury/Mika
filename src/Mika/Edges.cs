using Microsoft.Xna.Framework;

namespace Mika
{
    public struct Edges
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public static Edges All(int all)
        {
            return new Edges { Left = all, Top = all, Right = all, Bottom = all };
        }

        public static Edges Axis(int x, int y)
        {
            return new Edges { Left = x, Top = y, Right = x, Bottom = y };
        }

        public int TotalX
        {
            get
            {
                return Left + Right;
            }
        }

        public int TotalY
        {
            get
            {
                return Top + Bottom;
            }
        }

        public Point TotalAll
        {
            get
            {
                return new Point(Left + Right, Top + Bottom);
            }
        }
    }
}
