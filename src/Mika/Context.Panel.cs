using Microsoft.Xna.Framework;

namespace Mika
{
    public partial class Context
    {
        public void Panel()
        {
            Panel(LayoutType.Vertical, LayoutSizingMode.Auto, Point.Zero, DefaultValues.Style);
        }

        public void Panel(LayoutType layoutType)
        {
            Panel(layoutType, LayoutSizingMode.Auto, Point.Zero, DefaultValues.Style);
        }

        public void Panel(LayoutType layoutType, Style style)
        {
            Panel(layoutType, LayoutSizingMode.Auto, Point.Zero, style);
        }

        public void Panel(LayoutSizingMode layoutSizingMode, Point size)
        {
            Panel(LayoutType.Vertical, layoutSizingMode, size, DefaultValues.Style);
        }

        public void Panel(LayoutSizingMode layoutSizingMode, Point size, Style style)
        {
            Panel(LayoutType.Vertical, layoutSizingMode, size, style);
        }

        public void Panel(LayoutType layoutType, Point size)
        {
            Panel(layoutType, LayoutSizingMode.Auto, size, DefaultValues.Style);
        }

        public void Panel(LayoutType layoutType, Point size, Style style)
        {
            Panel(layoutType, LayoutSizingMode.Auto, size, style);
        }

        public void Panel(
            LayoutType layoutType,
            LayoutSizingMode layoutSizingMode,
            Point size,
            Style style)
        {
            var id = GetId();
            var layout = PeekLayout();
            var startPos = layout.Cursor;
            var border = style.Border != DefaultValues.Style.Border ? style.Border : Theme.BorderSize;
            var padding = style.Padding != DefaultValues.Style.Padding ? style.Padding : Theme.ContainerPadding;
            var spacing = style.Spacing > 0 ? style.Spacing : Theme.LayoutSpacing;
            var posOffset = new Point(
                startPos.X + padding.Left + border.Left,
                startPos.Y + padding.Top + border.Top);

            ContainerStack.Push(new ContainerState
            {
                DrawCommandIndex = Commands.Count,
                Size = size,
                Padding = padding,
                BorderSize = border,
                LayoutSpacing = spacing,
                Style = style,
                StartingCursor = startPos
            });

            // Border
            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawCommandType.Texture,
                Texture = DotTexture,
                Position = default,
                Size = default,
                Color = style.BorderColor != DefaultValues.Style.BorderColor ? style.BorderColor : Theme.BorderColor,
                Opacity = style.Opacity != DefaultValues.Style.Opacity ? style.Opacity : Theme.Opacity,
            });

            // Panel Background
            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawCommandType.Texture,
                Texture = DotTexture,
                Position = default,
                Size = default,
                Color = style.Color != DefaultValues.Style.Color ? style.Color : Theme.PanelColor,
                Opacity = style.Opacity != DefaultValues.Style.Opacity ? style.Opacity : Theme.Opacity,
            });

            Layout(layoutType, layoutSizingMode, size, spacing);
            var newLayout = LayoutStack.Pop();
            newLayout.Anchor = posOffset;
            newLayout.Cursor = posOffset;
            newLayout.Position = posOffset;
            LayoutStack.Push(newLayout);

            if (layoutSizingMode == LayoutSizingMode.Fixed)
            {
                _ = ClippingStack.Pop();
                BeginClip(Utils.RectFromPosAndSize(posOffset, size));
            }
        }

        public void ClosePanel()
        {
            var panel = ContainerStack.Pop();
            var layout = LayoutStack.Pop(); // Layout is closed by this

            var outerRectIndex = panel.DrawCommandIndex;
            var innerRectIndex = panel.DrawCommandIndex + 1;

            int sizeX = 0, sizeY = 0;
            if (layout.SizingMode == LayoutSizingMode.Fixed)
            {
                sizeX = layout.Size.X + panel.Padding.TotalX + panel.BorderSize.TotalX;
                sizeY = layout.Size.Y + panel.Padding.TotalY + panel.BorderSize.TotalY;
            }
            else
            {
                if (layout.Type == LayoutType.Vertical)
                {
                    sizeX = layout.Size.X + panel.Padding.TotalX + panel.BorderSize.TotalX;
                    sizeY = (layout.Cursor.Y - panel.StartingCursor.Y) + panel.Padding.Bottom + panel.BorderSize.Bottom - panel.LayoutSpacing;
                    if (sizeY < 0) sizeY = panel.Padding.TotalY + panel.BorderSize.TotalY;
                }
                else if (layout.Type == LayoutType.Horizontal)
                {
                    sizeX = (layout.Cursor.X - panel.StartingCursor.X) + panel.Padding.Right + panel.BorderSize.Right - panel.LayoutSpacing;
                    sizeY = layout.Size.Y + panel.Padding.TotalY + panel.BorderSize.TotalY;
                    if (sizeX < 0)
                        sizeX = panel.Padding.TotalX + panel.BorderSize.TotalX;
                }
            }

            var outerRect = Commands[outerRectIndex];
            outerRect.Position = panel.StartingCursor;
            outerRect.Size = new Point(sizeX, sizeY);
            Commands[outerRectIndex] = outerRect;

            var innerRect = Commands[innerRectIndex];
            innerRect.Position = new Point(
                panel.StartingCursor.X + panel.BorderSize.Left,
                panel.StartingCursor.Y + panel.BorderSize.Top);
            innerRect.Size = new Point(sizeX - panel.BorderSize.TotalX, sizeY - panel.BorderSize.TotalY);
            Commands[innerRectIndex] = innerRect;

            if (layout.SizingMode == LayoutSizingMode.Fixed)
                EndClip();

            ExpandLayout(new Point(sizeX, sizeY));
        }
    }
}
