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
        public float Opacity;
        public bool Hidden;
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

        public static Style New()
        {
            return DefaultValues.Style;
        }

        public static Style Default
        {
            get
            {
                return DefaultValues.Style;
            }
        }

        public static Style ButtonDefault
        {
            get
            {
                return new Style()
                {
                    Padding = Edges.LTRB(4, 0, 4, 4),
                    TextAlign = TextAlignment.Center
                };
            }
        }

        #region Builder-like Helpers

        public Style WithOrigin(Point origin)
        {
            Origin = origin;
            return this;
        }

        public Style WithOrigin(int x, int y)
        {
            Origin = new Point(x, y);
            return this;
        }

        public Style WithRotation(float rotation)
        {
            Rotation = rotation;
            return this;
        }

        public Style WithSize(Point size)
        {
            Size = size;
            return this;
        }

        public Style WithSize(int width, int height)
        {
            Size = new Point(width, height);
            return this;
        }

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

        public Style WithBorder(Edges border)
        {
            Border = border;
            return this;
        }

        public Style WithOpacity(float opacity)
        {
            Opacity = opacity;
            return this;
        }

        public Style WithHidden(bool hidden)
        {
            Hidden = hidden;
            return this;
        }

        public Style WithZIndex(int zIndex)
        {
            ZIndex = zIndex;
            return this;
        }

        public Style WithColor(Color color)
        {
            Color = color;
            return this;
        }

        public Style WithHoverColor(Color color)
        {
            HoverColor = color;
            return this;
        }

        public Style WithFocusColor(Color color)
        {
            FocusColor = color;
            return this;
        }

        public Style WithActiveColor(Color color)
        {
            ActiveColor = color;
            return this;
        }

        public Style WithBorderColor(Color color)
        {
            BorderColor = color;
            return this;
        }

        public Style WithBorderHoverColor(Color color)
        {
            BorderHoverColor = color;
            return this;
        }

        public Style WithBorderFocusColor(Color color)
        {
            BorderFocusColor = color;
            return this;
        }

        public Style WithBorderActiveColor(Color color)
        {
            BorderActiveColor = color;
            return this;
        }

        public Style WithFont(SpriteFontBase font)
        {
            Font = font;
            return this;
        }

        public Style WithTextColor(Color color)
        {
            TextColor = color;
            return this;
        }

        public Style WithTextHoverColor(Color color)
        {
            TextHoverColor = color;
            return this;
        }

        public Style WithTextFocusColor(Color color)
        {
            TextFocusColor = color;
            return this;
        }

        public Style WithTextActiveColor(Color color)
        {
            TextActiveColor = color;
            return this;
        }

        public Style WithTextAlign(TextAlignment align)
        {
            TextAlign = align;
            return this;
        }

        public Style WithTextStyle(TextStyle style)
        {
            TextStyle = style;
            return this;
        }

        public Style WithSliderWidth(int width)
        {
            SliderWidth = width;
            return this;
        }

        public Style WithSliderHeight(int height)
        {
            SliderHeight = height;
            return this;
        }

        public Style WithSliderThumbSize(int size)
        {
            SliderThumbSize = size;
            return this;
        }

        public Style WithSliderThumbColor(Color color)
        {
            SliderThumbColor = color;
            return this;
        }

        public Style WithSliderThumbHoverColor(Color color)
        {
            SliderThumbHoverColor = color;
            return this;
        }

        public Style WithSliderThumbFocusColor(Color color)
        {
            SliderThumbFocusColor = color;
            return this;
        }

        public Style WithSliderThumbActiveColor(Color color)
        {
            SliderThumbActiveColor = color;
            return this;
        }

        public Style WithSliderStep(float step)
        {
            SliderStep = step;
            return this;
        }

        public Style WithSpacing(int spacing)
        {
            Spacing = spacing;
            return this;
        }

        public Style WithGridSpacing(Point spacing)
        {
            GridSpacing = spacing;
            return this;
        }

        public Style WithGridSpacing(int x, int y)
        {
            GridSpacing = new Point(x, y);
            return this;
        }

        #endregion
    }
}