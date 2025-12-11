using FontStashSharp;
using Microsoft.Xna.Framework;

namespace Mika
{
    public struct Style
    {
        public Point Origin;
        public float Rotation;
        public Edges Padding;
        public Edges Border;
        public int ZIndex;

        // Widgets Only

        public Color Color;
        public Color HoverColor;
        public Color FocusColor;
        public Color ActiveColor;

        public Color BorderColor;
        public Color HoverBorderColor;
        public Color FocusBorderColor;
        public Color ActiveBorderColor;

        // Text; RichText
        public Color TextColor;
        public Color HoverTextColor;
        public Color FocusTextColor;
        public Color ActiveTextColor;
        public TextAlignment TextAlign;
        public TextStyle TextStyle;

        // Slider; ScrollView
        public Point ScrollbarSize;
        public Color ScrollbarColor;
        public Color ScrollThumbColor;

        // Layout Only
        public int Spacing;
        public Point GridSpacing;
    }
}