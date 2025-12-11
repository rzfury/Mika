using Microsoft.Xna.Framework;

namespace Mika
{
    public partial class Context
    {
        public void Panel(LayoutType layoutType, Point size = default, Style style = default)
        {
            var id = GetId();
            var layout = PeekLayout();
            var startPos = layout.Cursor;
            var posOffset = new Point(
                startPos.X + style.Padding.Left + style.Border.Left,
                startPos.Y + style.Padding.Top + style.Border.Top);

            ContainerStack.Push(new ContainerState
            {
                DrawCommandIndex = Commands.Count,
                Size = size,
                Style = style,
                StartingCursor = startPos
            });

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawType.Texture,
                Texture = DotTexture,
                Position = default,
                Size = default,
                Color = style.BorderColor
            });

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawType.Texture,
                Texture = DotTexture,
                Position = default,
                Size = default,
                Color = style.Color
            });

            Layout(layoutType, size: size, spacing: style.Spacing);
            SetCursorPos(posOffset);
        }

        public void ClosePanel()
        {
            var panel = ContainerStack.Pop();
            var layout = LayoutStack.Pop(); // Layout is closed by this

            var outerRectIndex = panel.DrawCommandIndex;
            var innerRectIndex = panel.DrawCommandIndex + 1;

            int sizeX = 0, sizeY = 0;
            if (layout.Type == LayoutType.Vertical)
            {
                sizeX = layout.Size.X + panel.Style.Padding.TotalX + panel.Style.Border.TotalX;
                sizeY = (layout.Cursor.Y - panel.StartingCursor.Y) + panel.Style.Padding.Bottom + panel.Style.Border.Bottom - panel.Style.Spacing;
                if (sizeY < 0) sizeY = panel.Style.Padding.TotalY + panel.Style.Border.TotalY;
            }
            else if (layout.Type == LayoutType.Horizontal)
            {
                sizeX = (layout.Cursor.X - panel.StartingCursor.X) + panel.Style.Padding.Right + panel.Style.Border.Right - panel.Style.Spacing;
                sizeY = layout.Size.Y + panel.Style.Padding.TotalY + panel.Style.Border.TotalY;
                if (sizeX < 0)
                    sizeX = panel.Style.Padding.TotalX + panel.Style.Border.TotalX;
            }

            var outerRect = Commands[outerRectIndex];
            outerRect.Position = panel.StartingCursor;
            outerRect.Size = new Point(sizeX, sizeY);
            Commands[outerRectIndex] = outerRect;

            var innerRect = Commands[innerRectIndex];
            innerRect.Position = new Point(
                panel.StartingCursor.X + panel.Style.Border.Left,
                panel.StartingCursor.Y + panel.Style.Border.Top);
            innerRect.Size = new Point(sizeX - panel.Style.Border.TotalX, sizeY - panel.Style.Border.TotalY);
            Commands[innerRectIndex] = innerRect;

            ExpandLayout(new Point(sizeX, sizeY));
        }
    }
}
