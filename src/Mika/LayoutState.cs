using Microsoft.Xna.Framework;

namespace Mika
{
    public struct LayoutState
    {
        public LayoutType Type;
        public Point Cursor;
        public Point Size;
        public Point MaxSize;
        // public int GridAxisCellCount; // normalize column and row
        public int Spacing;

        internal bool IsCursorSameLine;
        internal bool IsCursorAligning;
        internal Point PrevLineCursor;
        internal Point PrevLineSize;
        internal Point CurrentLineSize;

        internal Point Anchor;
    };
}