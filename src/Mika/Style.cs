using FontStashSharp;
using Microsoft.Xna.Framework;

namespace Mika
{
    public struct Style
    {
        public Point Origin;
        public float Rotation;
        public Point Size;
        public Edges Padding;
        public Edges Border;
        public int ZIndex;

        // Widgets Only

        public Color Color;
        public Color HoverColor;
        public Color FocusColor;
        public Color ActiveColor;

        public Color BorderColor;
        public Color BorderHoverColor;
        public Color BorderFocusColor;
        public Color BorderActiveColor;

        // Text; RichText
        public SpriteFontBase Font;
        public Color TextColor;
        public Color TextHoverColor;
        public Color TextFocusColor;
        public Color TextActiveColor;
        public TextAlignment TextAlign;
        public TextStyle TextStyle;


        // Slider;
        public int SliderWidth;
        public int SliderHeight;
        public int SliderThumbSize;
        public Color SliderThumbColor;
        public Color SliderThumbHoverColor;
        public Color SliderThumbFocusColor;
        public Color SliderThumbActiveColor;
        public float SliderStep;

        // Layout Only
        public int Spacing;
        public Point GridSpacing;

        public static Style Default { get { return new Style(); } }

        public Style WithPadding(int all)
        {
            Padding = Edges.All(all);
            return this;
        }

        public Style WithPadding(Edges padding)
        {
            Padding = padding;
            return this;
        }

        public Style WithBorder(int all)
        {
            Border = Edges.All(all);
            return this;
        }

        public static Style ButtonDefault = new Style()
        {
            Padding = Edges.LTRB(4, 0, 4, 4),
            TextAlign = TextAlignment.Center
        };
    }
}