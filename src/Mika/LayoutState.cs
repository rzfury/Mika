using Microsoft.Xna.Framework;

namespace Mika
{
    public struct LayoutState
    {
        public LayoutType Type;
        public Point Size;
        public int Spacing;

        #region Cursor and Layout Planning

        public LayoutSizingMode SizingMode;
        internal Point RealSize;
        internal Point Offset;

        internal Point Position;
        internal Point Anchor;
        public Point Cursor;
        internal Point PrevCursor;
        internal int PrevLineHeight;
        internal bool SameLineApplied;
        internal int SameLineXAdjustment;
        internal bool CursorDetached;
        internal Point CursorBeforeDetached;

        internal Alignment CurrentAlignment;
        internal Point AvailableSize;

        #endregion
    };
}