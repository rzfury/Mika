using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Mika
{
    public partial class Context
    {
        public void Button(string label)
        {
            Button(label, Point.Zero, DefaultValues.Style, DefaultValues.EventData);
        }

        public void Button(string label, EventData eventData)
        {
            Button(label, Point.Zero, DefaultValues.Style, eventData);
        }

        public void Button(string label, Point size, EventData eventData)
        {
            Button(label, size, DefaultValues.Style, eventData);
        }

        public void Button(string label, Style style, EventData eventData)
        {
            Button(label, Point.Zero, style, eventData);
        }

        public void Button(
            string label,
            Point size,
            Style style,
            EventData eventData)
        {
            if (TextController == null)
                throw new Exception("Cannot draw Button widget as it need TextController and it has not been initialized.");

            var specificId = !string.IsNullOrEmpty(eventData.Name) ? $"{label}##{eventData.Name}" : label;
            var id = GetId(specificId);

            var layout = PeekLayout();
            var pos = layout.Cursor;
            var font = TextController.GetFont();
            var border = style.Border != DefaultValues.Style.Border ? style.Border : Theme.BorderSize;
            var padding = style.Padding != DefaultValues.Style.Padding ? style.Padding : Theme.ContainerPadding;

            var textPos = new Point(
                pos.X + border.Left + padding.Left,
                pos.Y + border.Top + padding.Top);
            var textSize = TextController.MeasureString(label);

            var innerPos = new Point(
                pos.X + border.Left,
                pos.Y + border.Top);
            var innerSize = new Point(
                Math.Max(size.X, textSize.X) + padding.TotalX,
                Math.Max(size.Y, textSize.Y) + padding.TotalY);

            var outerSize = new Point(
                innerSize.X + border.TotalX,
                innerSize.Y + border.TotalY);

            var origin = style.Origin;
            if (style.TextAlign == TextAlignment.Left)
                textPos = new Point(textPos.X, textPos.Y);
            else if (style.TextAlign == TextAlignment.Center)
                textPos = new Point(textPos.X + ((innerSize.X - padding.TotalX) / 2) - (textSize.X / 2), textPos.Y);
            else if (style.TextAlign == TextAlignment.Right)
                textPos = new Point(textPos.X + outerSize.X - padding.TotalX - border.TotalX - textSize.X, textPos.Y);

            var rect = new Rectangle(pos.X, pos.Y, outerSize.X, outerSize.Y);
            var isMouseOver = rect.Contains(Utils.Vec2ToPoint(MousePosition));

            if (ClippingStack.Count > 0)
            {
                var clipping = ClippingStack.Peek();
                var visible = rect.Intersects(clipping.Rect);
                isMouseOver = isMouseOver && visible && clipping.Rect.Contains(Utils.Vec2ToPoint(MousePosition));
            }

            if (isMouseOver)
            {
                Hover = id;

                if (eventData.DetectLeftMouse && MouseState.LeftButton == ButtonState.Pressed
                    || eventData.DetectRightMouse && MouseState.RightButton == ButtonState.Pressed
                    || eventData.DetectMiddleMouse && MouseState.MiddleButton == ButtonState.Pressed)
                {
                    Active = id;
                }
            }

            var hover = Hover == id;
            var prevHover = PrevHover == id;
            var focus = Focus == id;
            var prevFocus = PrevFocus == id;
            var active = Active == id;
            var prevActive = PrevActive == id;

            if (!eventData.Equals(DefaultValues.EventData))
            {
                if ((!prevHover && hover || !prevFocus && focus) || NextEventTargetName == eventData.Name)
                    CurrentEventTarget = eventData;

                if (!prevHover && hover) Events?.Invoke(EventType.OnMouseEnter, eventData, null);
                if (prevHover && !hover) Events?.Invoke(EventType.OnMouseLeave, eventData, null);
                if (!prevFocus && focus) Events?.Invoke(EventType.OnFocus, eventData, null);
                if (prevFocus && !focus) Events?.Invoke(EventType.OnLostFocus, eventData, null);

                if (eventData.DetectLeftMouse && MouseLeftJustPressed() && prevHover && hover)
                    Events?.Invoke(EventType.OnPress, eventData, null);
                else if (eventData.DetectRightMouse && MouseRightJustPressed() && prevHover && hover)
                    Events?.Invoke(EventType.OnRightPress, eventData, null);
                else if (eventData.DetectMiddleMouse && MouseMiddleJustPressed() && prevHover && hover)
                    Events?.Invoke(EventType.OnMiddlePress, eventData, null);

                if (eventData.DetectLeftMouse && MouseLeftJustReleased() && prevHover && hover)
                {
                    Events?.Invoke(EventType.OnClick, eventData, null);
                    Active = Hash.Empty;
                }
                else if (eventData.DetectRightMouse && MouseRightJustReleased() && prevHover && hover)
                {
                    Events?.Invoke(EventType.OnRightClick, eventData, null);
                    Active = Hash.Empty;
                }
                else if (eventData.DetectMiddleMouse && MouseMiddleJustReleased() && prevHover && hover)
                {
                    Events?.Invoke(EventType.OnMiddleClick, eventData, null);
                    Active = Hash.Empty;
                }
            }

            // Border
            CreateBorderDrawCommand(id, pos, innerSize, border, style.BorderColor, style.BorderHoverColor, style.BorderFocusColor, style.BorderActiveColor, style.Opacity);

            // Button Color
            Commands.Add(new DrawCommand
            {
                Id = id,
                Hover = Hover == id,
                Focus = Focus == id,
                Active = Active == id,
                Type = DrawCommandType.Texture,
                Texture = DotTexture,
                Position = innerPos,
                Size = innerSize,
                Color = style.Color != DefaultValues.Style.Color ? style.Color : Theme.PrimaryColor,
                HoverColor = style.HoverColor != DefaultValues.Style.HoverColor ?style.HoverColor : Theme.PrimaryHoverColor,
                FocusColor = style.FocusColor != DefaultValues.Style.FocusColor ? style.FocusColor : Theme.PrimaryHoverColor,
                ActiveColor = style.ActiveColor != DefaultValues.Style.ActiveColor ?style.ActiveColor : Theme.PrimaryActiveColor,
                Opacity = style.Opacity != DefaultValues.Style.Opacity ? style.Opacity : Theme.Opacity,
            });

            // Button Label
            Commands.Add(new DrawCommand
            {
                Id = id,
                Hover = Hover == id,
                Focus = Focus == id,
                Active = Active == id,
                Type = DrawCommandType.String,
                Position = textPos,
                Size = textSize,
                Color = style.TextColor != DefaultValues.Style.TextColor ? style.TextColor : Theme.TextColor,
                HoverColor = style.TextHoverColor != DefaultValues.Style.TextHoverColor ? style.TextHoverColor : Theme.TextColor,
                FocusColor = style.TextFocusColor != DefaultValues.Style.TextFocusColor ? style.TextFocusColor : Theme.TextColor,
                ActiveColor = style.TextActiveColor != DefaultValues.Style.TextActiveColor ? style.TextActiveColor : Theme.TextColor,
                Opacity = style.Opacity != DefaultValues.Style.Opacity ? style.Opacity : Theme.Opacity,
                Text = label,
                Font = font
            });

            ExpandLayout(outerSize);
        }
    }
}
