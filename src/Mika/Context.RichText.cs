using FontStashSharp;
using FontStashSharp.RichText;

namespace Mika
{
    public partial class Context
    {
        public void RichText(string text, SpriteFontBase font, Style style = default)
        {
            var id = GetId();

            var layout = PeekLayout();
            var pos = layout.Cursor;

            var textSize = Utils.Vec2ToPoint(font.MeasureString(text));

            if (!Caches.RTL.TryGetValue(id, out var rtl))
            {
                rtl = new RichTextLayout() { Font = font, Text = text };
                Caches.RTL.Add(id, rtl);
            }

            rtl.Text = text;

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawType.RTL,
                Position = pos,
                Size = textSize,
                Text = text,
                Font = font,
                RTL = rtl
            });

            ExpandLayout(textSize);
        }
    }
}