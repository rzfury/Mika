using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mika
{
    public partial class Context
    {
        public void Sprite(Texture2D texture)
        {
            Sprite(texture, new Rectangle(0, 0, texture.Width, texture.Height), new Point(texture.Width, texture.Height), DefaultValues.Style);
        }

        public void Sprite(Texture2D texture, Style style)
        {
            Sprite(texture, new Rectangle(0, 0, texture.Width, texture.Height), new Point(texture.Width, texture.Height), style);
        }

        public void Sprite(Texture2D texture, Point size)
        {
            Sprite(texture, new Rectangle(0, 0, texture.Width, texture.Height), size, DefaultValues.Style);
        }

        public void Sprite(Texture2D texture, Point size, Style style)
        {
            Sprite(texture, new Rectangle(0, 0, texture.Width, texture.Height), size, style);
        }

        public void Sprite(Texture2D texture, Rectangle sourceRect, Point size)
        {
            Sprite(texture, sourceRect, size, DefaultValues.Style);
        }

        public void Sprite(Texture2D texture, Rectangle sourceRect, Point size, Style style)
        {
            var id = GetId();

            var layout = PeekLayout();
            var pos = layout.Cursor;

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawCommandType.Texture,
                Position = pos,
                Size = size,
                Texture = texture,
                SourceRect = sourceRect,
                Color = style.Color != DefaultValues.Style.Color ? style.Color : Color.White,
                Opacity = style.Opacity != DefaultValues.Style.Opacity ? style.Opacity : Theme.Opacity,
            });

            ExpandLayout(size);
        }
    }
}