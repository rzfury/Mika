using Microsoft.Xna.Framework;
using System;

namespace Mika
{
    public struct Edges : IEquatable<Edges>
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

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

        public Point TotalXY
        {
            get
            {
                return new Point(Left + Right, Top + Bottom);
            }
        }

        public static Edges LTRB(int l = 0, int t = 0, int r = 0, int b = 0)
        {
            return new Edges { Left = l, Top = t, Right = r, Bottom = b };
        }

        public static Edges All(int all)
        {
            return new Edges { Left = all, Top = all, Right = all, Bottom = all };
        }

        public static Edges Axis(int x, int y)
        {
            return new Edges { Left = x, Top = y, Right = x, Bottom = y };
        }

        public static Edges Default { get { return DefaultValues.Edges; } }

        public static bool operator ==(Edges a, Edges b)
        {
            return a.Left == b.Left && a.Top == b.Top && a.Right == b.Right && a.Bottom == b.Bottom;
        }

        public static bool operator !=(Edges a, Edges b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return (obj is Edges) && Equals((Edges)obj);
        }

        public bool Equals(Edges obj)
        {
            return this == obj;
        }

        public override int GetHashCode()
        {
            return Left.GetHashCode() ^ Top.GetHashCode() ^ Right.GetHashCode() ^ Bottom.GetHashCode();
        }
    }
}
