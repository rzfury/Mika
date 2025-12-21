using FontStashSharp.RichText;

namespace Mika
{
    public partial class Context
    {
        public void RichText(RichTextLayout rtl, Style style = default)
        {
            if (rtl == null)
                throw new System.ArgumentNullException("'rtl' cannot be null.");

            var id = GetId();

            var layout = PeekLayout();
            var pos = layout.Cursor;

            var font = rtl.Font ?? DefaultFont;
            var textSize = Utils.Vec2ToPoint(rtl.Font.MeasureString(rtl.Text));

            Commands.Add(new DrawCommand
            {
                Id = id,
                Type = DrawCommandType.RTL,
                Position = pos,
                Size = textSize,
                Text = rtl.Text,
                Font = font,
                RTL = rtl
            });

            ExpandLayout(textSize);
        }

        public void RichText(string text, RichTextLayout rtl, Style style = default)
        {
            if (rtl == null)
                throw new System.ArgumentNullException("'rtl' cannot be null.");

            rtl.Text = text;

            RichText(rtl, style);
        }
    }
}