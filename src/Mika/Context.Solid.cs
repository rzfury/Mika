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

            ExpandLayout(size);

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawType.Texture,
                Texture = DotTexture,
                Position = pos,
                Size = size,
                Color = style.Color
            });
        }
    }
}