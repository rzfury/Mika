using Microsoft.Xna.Framework;
using System;

namespace Mika
{
    public partial class Context
    {
        #region Layout

        public void Layout(
            LayoutType direction = LayoutType.Vertical,
            Point size = default,
            Point maxSize = default,
            int spacing = default)
        {
            var cursor = new Point();
            if (LayoutStack.Count > 0)
            {
                var parent = LayoutStack.Peek();
                cursor = parent.Cursor;
            }

            LayoutStack.Push(new LayoutState
            {
                Type = direction,
                Cursor = cursor,
                Size = size,
                MaxSize = maxSize,
                Spacing = spacing
            });
        }

        public LayoutState PeekLayout()
        {
            if (LayoutStack.Count == 0)
                throw new Exception("No layout found in the stack! Are you forget to call Layout() or Begin()?");

            return LayoutStack.Peek();
        }

        public void ExpandLayout(Point addedSize)
        {
            if (LayoutStack.Count == 0) return;

            var layout = LayoutStack.Pop();

            layout.PrevLineCursor = layout.Cursor;
            layout.PrevLineSize = layout.CurrentLineSize;
            layout.CurrentLineSize = addedSize;

            if (layout.IsCursorSameLine || layout.IsCursorAligning)
            {
                layout.Cursor = new Point(layout.Cursor.X - layout.PrevLineSize.X, layout.Cursor.Y);
                layout.IsCursorSameLine = false;
                layout.IsCursorAligning = false;
            }

            switch (layout.Type)
            {
                case LayoutType.Horizontal:
                    layout.Size = new Point(
                        layout.Size.X + layout.Spacing + addedSize.X,
                        Math.Max(layout.Size.Y, addedSize.Y));
                    layout.Cursor = new Point(
                        layout.Cursor.X + layout.Spacing + addedSize.X,
                        layout.Cursor.Y);
                    break;

                case LayoutType.Vertical:
                    layout.Size = new Point(
                        Math.Max(layout.Size.X, addedSize.X),
                        layout.Size.Y + layout.Spacing + addedSize.Y);
                    layout.Cursor = new Point(
                        layout.Cursor.X,
                        layout.Cursor.Y + layout.Spacing + addedSize.Y);
                    break;

                case LayoutType.Absolute:
                    layout.Size = new Point(
                        Math.Max(layout.Size.X, layout.Cursor.X + addedSize.X),
                        Math.Max(layout.Size.Y, layout.Cursor.Y + addedSize.Y));
                    break;

                default: break;
            }

            LayoutStack.Push(layout);
        }

        public void CloseLayout()
        {
            var child = LayoutStack.Pop();
            if (LayoutStack.Count > 0)
                ExpandLayout(child.Size);
        }

        #endregion

        #region Layout Cursor

        public Point GetCursorPos()
        {
            if (LayoutStack.Count == 0) return Point.Zero;
            return LayoutStack.Peek().Cursor;
        }

        public void SetCursorPos(Point pos)
        {
            if (LayoutStack.Count == 0) return;
            var layout = LayoutStack.Pop();
            layout.Cursor = pos;
            LayoutStack.Push(layout);
        }

        public void SetCursorPos(int x)
        {
            if (LayoutStack.Count == 0) return;
            var layout = LayoutStack.Pop();
            layout.Cursor = new Point(x, layout.Cursor.Y);
            LayoutStack.Push(layout);
        }

        public void SetCursorPos(int x, int y)
        {
            if (LayoutStack.Count == 0) return;
            var layout = LayoutStack.Pop();
            layout.Cursor = new Point(x, y);
            LayoutStack.Push(layout);
        }

        public void AddCursorPos(Point pos)
        {
            if (LayoutStack.Count == 0) return;
            var layout = LayoutStack.Pop();
            layout.Cursor = new Point(
                layout.Cursor.X + pos.X,
                layout.Cursor.Y + pos.Y);
            LayoutStack.Push(layout);
        }

        public void SameLine(int spacing = 0)
        {
            if (LayoutStack.Count == 0) return;
            var layout = LayoutStack.Pop();
            layout.IsCursorSameLine = true;
            layout.Cursor = new Point(
                layout.PrevLineCursor.X + layout.PrevLineSize.X + spacing,
                layout.PrevLineCursor.Y);
            LayoutStack.Push(layout);
        }

        public void AlignCursor(Point pos)
        {
            if (LayoutStack.Count == 0) return;
            var layout = LayoutStack.Pop();
            layout.IsCursorAligning = true;
            layout.Cursor = pos;
            LayoutStack.Push(layout);
        }

        #endregion
    }
}