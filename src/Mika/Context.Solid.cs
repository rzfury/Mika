using Microsoft.Xna.Framework;

namespace Mika
{
    public partial class Context
    {
        /// <summary>
        /// Draw a solid rectangle. <see cref="DotTexture"/> must be defined first.
        /// </summary>
        public void SolidRect(Point size, Style style = default)
        {
            var id = GetId();

            var layout = PeekLayout();
            var pos = layout.Cursor;

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawType.Texture,
                Texture = DotTexture,
                Position = pos,
                Size = size,
                Color = style.Color != default ? style.Color : Theme.PrimaryColor,
            });

            ExpandLayout(size);
        }

        public void Divider(Style style = default)
        {
            var id = GetId();
            
            var layout = PeekLayout();
            var pos = layout.Cursor;

            Point size;
            if (layout.Type == LayoutType.Horizontal)
            {
                size = new Point(
                    style.Size != default ? style.Size.X : Theme.DividerSize,
                    layout.Size.Y - layout.Cursor.Y);

            }
            else // Vertical
            {
                size = new Point(
                    layout.Size.X - layout.Cursor.X,
                    style.Size != default ? style.Size.Y : Theme.DividerSize);
            }

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawType.Texture,
                Texture = DotTexture,
                Position = pos,
                Size = size,
                Color = style.Color != default ? style.Color : Theme.PrimaryColor,
            });

            ExpandLayout(size);
        }
    }
}