using Microsoft.Xna.Framework;

namespace Mika
{
    public partial class Context
    {
        /// <summary>
        /// Draw a solid rectangle. <see cref="DotTexture"/> must be defined first.
        /// </summary>
        public void SolidRect(Point size)
        {
            SolidRect(size, DefaultValues.Style);
        }

        /// <summary>
        /// Draw a solid rectangle. <see cref="DotTexture"/> must be defined first.
        /// </summary>
        public void SolidRect(Point size, Style style)
        {
            var id = GetId();

            var layout = PeekLayout();
            var pos = layout.Cursor;

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawCommandType.Texture,
                Texture = DotTexture,
                Position = pos,
                Size = size,
                Color = style.Color != DefaultValues.Style.Color ? style.Color : Theme.PrimaryColor,
            });

            ExpandLayout(size);
        }

        public void Divider()
        {
            Divider(DefaultValues.Style);
        }

        public void Divider(Style style)
        {
            var id = GetId();
            
            var layout = PeekLayout();
            var pos = layout.Cursor;

            Point size;
            if (layout.Type == LayoutType.Horizontal)
            {
                size = new Point(
                    style.Size != DefaultValues.Style.Size ? style.Size.X : Theme.DividerSize,
                    layout.Size.Y - layout.Cursor.Y);

            }
            else // Vertical
            {
                size = new Point(
                    layout.Size.X - layout.Cursor.X,
                    style.Size != DefaultValues.Style.Size ? style.Size.Y : Theme.DividerSize);
            }

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawCommandType.Texture,
                Texture = DotTexture,
                Position = pos,
                Size = size,
                Color = style.Color != DefaultValues.Style.Color ? style.Color : Theme.PrimaryColor,
            });

            ExpandLayout(size);
        }
    }
}