using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Mika
{
    public partial class Context
    {
        public void Slider<T>(T value, T min, T max)
        {
            Slider(value, min, max, DefaultValues.Style, DefaultValues.EventData);
        }

        public void Slider<T>(T value, T min, T max, EventData eventData)
        {
            Slider(value, min, max, DefaultValues.Style, eventData);
        }

        public void Slider<T>(T value, T min, T max, Style style, EventData eventData)
        {
            if (typeof(T) != typeof(float) && typeof(T) != typeof(int))
                throw new NotSupportedException(string.Format("Slider is currently unsupported for type '{0}'.", typeof(T).ToString()));

            var id = GetId();

            var layout = PeekLayout();
            var border = style.Border != DefaultValues.Style.Border ? style.Border : Theme.BorderSize;
            var pos = new Point(layout.Cursor.X + border.Left, layout.Cursor.Y + border.Right);

            var label = value.ToString();

            var font = style.Font ?? DefaultFont;
            var textSize = Utils.Vec2ToPoint(font.MeasureString(label));

            var finalSize = new Point(
                style.Size != DefaultValues.Style.Size ? style.Size.X : Theme.SliderSize.X,
                Math.Max(
                    style.Size != DefaultValues.Style.Size ? style.Size.Y : Theme.SliderSize.Y,
                    textSize.Y));
            var trackRect = new Rectangle(pos.X, pos.Y, finalSize.X, finalSize.Y);
            var thumbRect = new Rectangle(pos.X, pos.Y, Theme.SliderThumbWidth, finalSize.Y);
            var textPos = new Point(trackRect.X + (trackRect.Width / 2) - (textSize.X / 2), trackRect.Y);

            var isMouseOver = trackRect.Contains(Utils.Vec2ToPoint(MousePosition));
            if (isMouseOver && MouseState.LeftButton == ButtonState.Pressed)
                Active = id;

            if (PrevActive == id)
            {
                if (MouseState.LeftButton == ButtonState.Pressed) Active = id;
                else Events?.Invoke(EventType.OnClick, eventData, value);
            }

            T newValue = value;

            if (Active == id)
            {
                // TODO: Add implementation for keyboard

                if (MouseState.LeftButton == ButtonState.Released)
                {
                    Active = Hash.Empty;
                }
                else
                {
                    var t = (MousePosition.X - trackRect.X) / trackRect.Width;

                    if (typeof(T) == typeof(float))
                    {
                        var fMin = Convert.ToSingle(min);
                        var fMax = Convert.ToSingle(max);
                        var fNewValue = Utils.ClampF(Utils.Lerp(fMin, fMax, t), fMin, fMax);
                        newValue = (T)(object)fNewValue;
                    }
                    else if (typeof(T) == typeof(int))
                    {
                        var iMin = Convert.ToInt32(min);
                        var iMax = Convert.ToInt32(max);
                        var iNewValue = Utils.Clamp(Utils.LerpInt(iMin, iMax, t), iMin, iMax);
                        newValue = (T)(object)iNewValue;
                    }

                    if (!newValue.Equals(value))
                        Events?.Invoke(EventType.OnChange, eventData, newValue);
                }
            }

            float thumbT = (Convert.ToSingle(value) - Convert.ToSingle(min)) / (Convert.ToSingle(max) - Convert.ToSingle(min));
            thumbRect.X = Utils.LerpInt(trackRect.Left, trackRect.Right - thumbRect.Width, thumbT);

            // Border
            Commands.Add(new DrawCommand
            {
                Id = id,
                Hover = Hover == id,
                Focus = Focus == id,
                Active = Active == id,
                Type = DrawCommandType.Texture,
                Texture = DotTexture,
                Position = new Point(pos.X - border.Left, pos.Y - border.Right),
                Size = new Point(trackRect.Width, trackRect.Height) + border.TotalXY,
                Color = style.BorderColor != DefaultValues.Style.BorderColor ? style.BorderColor : Theme.BaseColor,
                HoverColor = style.BorderHoverColor != DefaultValues.Style.BorderHoverColor ? style.BorderHoverColor : Theme.BaseHoverColor,
                FocusColor = style.BorderFocusColor != DefaultValues.Style.BorderFocusColor ? style.BorderFocusColor : Theme.BaseHoverColor,
                ActiveColor = style.BorderActiveColor != DefaultValues.Style.BorderActiveColor ? style.BorderActiveColor : Theme.BaseActiveColor
            });

            // Track
            Commands.Add(new DrawCommand
            {
                Id = id,
                Hover = Hover == id,
                Focus = Focus == id,
                Active = Active == id,
                Type = DrawCommandType.Texture,
                Texture = DotTexture,
                Position = pos,
                Size = new Point(trackRect.Width, trackRect.Height),
                Color = style.Color != DefaultValues.Style.Color ? style.Color : Theme.BaseColor,
                HoverColor = style.HoverColor != DefaultValues.Style.HoverColor ? style.HoverColor : Theme.BaseHoverColor,
                FocusColor = style.FocusColor != DefaultValues.Style.FocusColor ? style.FocusColor : Theme.BaseHoverColor,
                ActiveColor = style.ActiveColor != DefaultValues.Style.ActiveColor ? style.ActiveColor : Theme.BaseActiveColor
            });

            // Thumb
            Commands.Add(new DrawCommand
            {
                Id = id,
                Hover = Hover == id,
                Focus = Focus == id,
                Active = Active == id,
                Type = DrawCommandType.Texture,
                Texture = DotTexture,
                Position = new Point(thumbRect.X, thumbRect.Y),
                Size = new Point(thumbRect.Width, thumbRect.Height),
                Color = style.SliderThumbColor != DefaultValues.Style.SliderThumbColor ? style.SliderThumbColor : Theme.PrimaryColor,
                HoverColor = style.SliderThumbHoverColor != DefaultValues.Style.SliderThumbHoverColor ? style.SliderThumbHoverColor : Theme.PrimaryHoverColor,
                FocusColor = style.SliderThumbFocusColor != DefaultValues.Style.SliderThumbFocusColor ? style.SliderThumbFocusColor : Theme.PrimaryHoverColor,
                ActiveColor = style.SliderThumbActiveColor != DefaultValues.Style.SliderThumbActiveColor ? style.SliderThumbActiveColor : Theme.PrimaryActiveColor,
            });

            // Slider Value
            Commands.Add(new DrawCommand
            {
                Id = id,
                Hover = Hover == id,
                Focus = Focus == id,
                Active = Active == id,
                Type = DrawCommandType.String,
                Position = textPos,
                Color = style.TextColor != DefaultValues.Style.TextColor ? style.TextColor : Theme.TextColor,
                HoverColor = style.TextHoverColor != DefaultValues.Style.TextHoverColor ? style.TextHoverColor : Theme.TextHoverColor,
                FocusColor = style.TextFocusColor != DefaultValues.Style.TextFocusColor ? style.TextFocusColor : Theme.TextHoverColor,
                ActiveColor = style.TextActiveColor != DefaultValues.Style.TextActiveColor ? style.TextActiveColor : Theme.TextActiveColor,
                Text = label,
                Font = font
            });

            ExpandLayout(finalSize + border.TotalXY);
        }
    }
}
