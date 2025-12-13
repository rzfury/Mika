using FontStashSharp;
using Microsoft.Xna.Framework;
using System;

namespace Mika
{
    public partial class Context
    {
        public void Text(string text, Point size = default, Style style = default)
        {
            var id = GetId();

            var layout = PeekLayout();
            var pos = layout.Cursor;

            var font = style.Font ?? DefaultFont;
            var textSize = Utils.Vec2ToPoint(font.MeasureString(text));

            var finalSize = new Point(
                Math.Max(size.X, textSize.X) + style.Padding.TotalX,
                Math.Max(size.Y, textSize.Y) + style.Padding.TotalY);

            var origin = style.Origin;
            if (style.TextAlign == TextAlignment.Left)
                pos = new Point(pos.X, pos.Y);
            else if (style.TextAlign == TextAlignment.Center)
                pos = new Point(pos.X + (finalSize.X / 2) - (textSize.X / 2), pos.Y);
            else if (style.TextAlign == TextAlignment.Right)
                pos = new Point(pos.X + finalSize.X - textSize.X, pos.Y);

            ExpandLayout(finalSize);

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawType.String,
                Position = pos,
                Size = finalSize,
                Color = style.Color != default ? style.Color : Theme.TextColor,
                Rotation = style.Rotation,
                Origin = origin,
                Text = text,
                Font = font
            });
        }
    }
}
