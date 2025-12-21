using Microsoft.Xna.Framework;
using System;

namespace Mika
{
    public partial class Context
    {
        #region Layout

        public void Layout(
            LayoutType direction = LayoutType.Vertical,
            LayoutSizingMode sizingMode = default,
            Point size = default,
            int spacing = -1)
        {
            var anchor = new Point();
            var cursor = new Point();
            var pos = new Point();
            if (LayoutStack.Count > 0)
            {
                var parent = LayoutStack.Peek();
                anchor = parent.Cursor;
                cursor = parent.Cursor;
                pos = parent.Cursor;
            }

            if (sizingMode == LayoutSizingMode.Fixed)
            {
                if (size.X <= 0 || size.Y <= 0)
                    throw new ArgumentException("Layout sizes cannot be less than equal to zero for LayoutSizingMode.Fixed");

                BeginClip(new Rectangle(anchor.X, anchor.Y, size.X, size.Y));
            }

            LayoutStack.Push(new LayoutState
            {
                Type = direction,
                Anchor = anchor,
                Cursor = cursor,
                Position = pos,
                Size = size,
                RealSize = new Point(),
                SizingMode = sizingMode,
                Spacing = spacing < 0 ? Theme.LayoutSpacing : spacing,
            });
        }

        public void CloseLayout()
        {
            var child = LayoutStack.Pop();

            if (child.SizingMode == LayoutSizingMode.Fixed)
                EndClip();

            if (LayoutStack.Count > 0)
                ExpandLayout(child.Size);
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

            layout.PrevCursor = layout.Cursor;

            switch (layout.Type)
            {
                case LayoutType.Horizontal:
                    layout.RealSize = new Point(
                        layout.RealSize.X + layout.Spacing + addedSize.X,
                        Math.Max(layout.RealSize.Y, addedSize.Y));
                    if (!layout.CursorDetached)
                    {
                        layout.Cursor = new Point(
                            layout.Cursor.X + layout.Spacing + addedSize.X,
                            layout.Cursor.Y);
                    }
                    break;

                case LayoutType.Vertical:
                    layout.RealSize = new Point(
                        Math.Max(layout.RealSize.X, addedSize.X + (layout.Cursor.X - layout.Anchor.X)),
                        layout.RealSize.Y + layout.Spacing + addedSize.Y);
                    if (!layout.CursorDetached)
                    {
                        if (layout.SameLineApplied)
                        {
                            layout.Cursor.X -= layout.SameLineXAdjustment;
                            var yHeight = Math.Max(layout.PrevLineHeight, addedSize.Y);
                            layout.Cursor.Y += yHeight + layout.Spacing;
                            layout.PrevLineHeight = yHeight;
                        }
                        else
                        {
                            layout.Cursor = new Point(
                                layout.Cursor.X,
                                layout.Cursor.Y + layout.Spacing + addedSize.Y);
                        }
                    }
                    break;

                case LayoutType.Absolute:
                    layout.RealSize = new Point(
                        Math.Max(layout.RealSize.X, layout.Cursor.X + addedSize.X),
                        Math.Max(layout.RealSize.Y, layout.Cursor.Y + addedSize.Y));
                    break;

                default: break;
            }

            if (layout.SizingMode == LayoutSizingMode.Auto)
                layout.Size = layout.RealSize;

            if (layout.SameLineApplied)
            {
                if (!layout.CursorDetached)
                    layout.Cursor.X = layout.Anchor.X;
                layout.SameLineXAdjustment = addedSize.X + layout.Spacing;
                layout.SameLineApplied = false;
            }

            if (layout.CursorDetached)
            {
                layout.Cursor = layout.CursorBeforeDetached;
                layout.CursorDetached = false;
            }

            LayoutStack.Push(layout);
        }

        public void OffsetLayout(Point offset)
        {
            var layout = LayoutStack.Pop();
            layout.Offset += offset;
            LayoutStack.Push(layout);
        }

        #endregion

        #region Cursor Operations

        public Point GetCursorPos()
        {
            if (LayoutStack.Count == 0) return Point.Zero;
            return LayoutStack.Peek().Cursor;
        }

        public void SetCursorPos(int x, int y)
        {
            SetCursorPos(new Point(x, y));
        }

        public void SetCursorPos(Point pos)
        {
            if (LayoutStack.Count == 0) return;
            var layout = LayoutStack.Pop();
            layout.Cursor = pos;
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

        public void DetachCursor()
        {
            if (LayoutStack.Count == 0) return;
            var layout = LayoutStack.Pop();
            layout.CursorBeforeDetached = layout.Cursor;
            layout.CursorDetached = true;
            LayoutStack.Push(layout);
        }

        public void SameLine(int minXOffset = -1)
        {
            SameLine(Point.Zero, minXOffset);
        }

        public void SameLine(Point offset, int minXOffset)
        {
            if (LayoutStack.Count == 0) return;
            if (PeekLayout().Type != LayoutType.Vertical) return;

            var layout = LayoutStack.Pop();

            layout.Cursor = layout.PrevCursor;
            layout.SameLineXAdjustment = Math.Max(layout.SameLineXAdjustment, minXOffset);
            layout.Cursor.X += layout.SameLineXAdjustment;
            layout.Cursor += offset;
            layout.SameLineApplied = true;

            LayoutStack.Push(layout);
        }

        public void Indent(int amount)
        {
            if (LayoutStack.Count == 0) return;

            var layout = LayoutStack.Pop();

            layout.Anchor.X += amount;
            layout.Cursor.X += amount;

            LayoutStack.Push(layout);
        }

        #endregion
    }
}