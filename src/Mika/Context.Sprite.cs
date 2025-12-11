using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mika
{
    public partial class Context
    {
        public void Sprite(Texture2D texture, Point size, Style style = default)
        {
            var id = GetId();

            var layout = PeekLayout();
            var pos = layout.Cursor;

            ExpandLayout(size);

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawType.Texture,
                Position = pos,
                Size = size,
                Texture = texture,
                Color = style.Color
            });
        }

        public void Sprite(Texture2D texture, Rectangle sourceRect, Point size, Style style = default)
        {
            var id = GetId();

            var layout = PeekLayout();
            var pos = layout.Cursor;

            ExpandLayout(size);

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawType.Texture,
                Position = pos,
                Size = size,
                Texture = texture,
                SourceRect = sourceRect
            });
        }
    }
}