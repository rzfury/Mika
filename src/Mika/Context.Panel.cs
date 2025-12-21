using Microsoft.Xna.Framework;

namespace Mika
{
    public partial class Context
    {
        public void Panel(
            LayoutType layoutType,
            Point size,
            Style style)
        {
            Panel(layoutType, LayoutSizingMode.Auto, size, style);
        }

        public void Panel(
            LayoutType layoutType,
            LayoutSizingMode sizingMode = default,
            Point size = default,
            Style style = default)
        {
            var id = GetId();
            var layout = PeekLayout();
            var startPos = layout.Cursor;
            var border = style.Border != default ? style.Border : Theme.BorderSize;
            var padding = style.Padding != default ? style.Padding : Theme.ContainerPadding;
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

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawCommandType.Texture,
                Texture = DotTexture,
                Position = default,
                Size = default,
                Color = style.BorderColor != default ? style.BorderColor : Theme.BorderColor
            });

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawCommandType.Texture,
                Texture = DotTexture,
                Position = default,
                Size = default,
                Color = style.Color != default ? style.Color : Theme.PanelColor
            });

            Layout(layoutType, sizingMode, size, spacing);
            var newLayout = LayoutStack.Pop();
            newLayout.Anchor = posOffset;
            newLayout.Cursor = posOffset;
            newLayout.Position = posOffset;
            LayoutStack.Push(newLayout);

            if (sizingMode == LayoutSizingMode.Fixed)
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
