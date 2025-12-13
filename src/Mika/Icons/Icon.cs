using Microsoft.Xna.Framework;

namespace Mika.Icons
{
    public struct Icon
    {
        public string Name;
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public Point Size { get { return new Point(Width, Height); } }

        public Rectangle SourceRect { get { return new Rectangle(X, Y, Width, Height); }  }
    }
}
