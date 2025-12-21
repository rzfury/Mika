using Microsoft.Xna.Framework;
using System;

namespace Mika
{
    public partial class Context
    {
        public void Text(string text)
        {
            Text(text, Point.Zero, DefaultValues.Style);
        }

        public void Text(string text, Style style)
        {
            Text(text, Point.Zero, style);
        }

        public void Text(string text, Point size, Style style)
        {
            var id = GetId();

            var layout = PeekLayout();
            var pos = layout.Cursor;

            var font = style.Font ?? DefaultFont;
            var color = style.Color == DefaultValues.Style.Color ? Theme.TextColor : style.TextColor;
            var padding = style.Padding == DefaultValues.Style.Padding ? Theme.TextPadding : style.Padding;
            var origin = style.Origin == DefaultValues.Style.Origin ? Point.Zero : style.Origin;
            var rotation = style.Rotation == DefaultValues.Style.Rotation ? 0 : style.Rotation;
            var textAlign = style.TextAlign == DefaultValues.Style.TextAlign ? TextAlignment.Left : style.TextAlign;

            var textSize = Utils.Vec2ToPoint(font.MeasureString(text));

            var finalSize = new Point(
                Math.Max(size.X, textSize.X) + padding.TotalX,
                Math.Max(size.Y, textSize.Y) + padding.TotalY);

            if (textAlign == TextAlignment.Left)
                pos = new Point(pos.X, pos.Y);
            else if (textAlign == TextAlignment.Center)
                pos = new Point(pos.X + (finalSize.X / 2) - (textSize.X / 2), pos.Y);
            else if (textAlign == TextAlignment.Right)
                pos = new Point(pos.X + finalSize.X - textSize.X, pos.Y);

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawCommandType.String,
                Position = pos + layout.Offset,
                Size = finalSize,
                Color = color,
                Rotation = rotation,
                Origin = origin,
                Text = text,
                Font = font
            });

            ExpandLayout(finalSize);
        }
    }
}
