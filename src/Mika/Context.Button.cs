using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Mika
{
    public partial class Context
    {
        public void Button(
            string label,
            SpriteFontBase font,
            Point size = default,
            Style style = default,
            EventData eventData = default)
        {
            var specificId = !string.IsNullOrEmpty(eventData.Name) ? $"{label}##{eventData.Name}" : label;
            var id = GetId(specificId);

            var layout = PeekLayout();

            var pos = layout.Cursor;

            var textPos = new Point(
                pos.X + style.Border.Left + style.Padding.Left,
                pos.Y + style.Border.Top + style.Padding.Top);
            var textSize = Utils.Vec2ToPoint(font.MeasureString(label));

            var innerPos = new Point(
                pos.X + style.Border.Left,
                pos.Y + style.Border.Top);
            var innerSize = new Point(
                Math.Max(size.X, textSize.X) + style.Padding.TotalX,
                Math.Max(size.Y, textSize.Y) + style.Padding.TotalY);

            var outerSize = new Point(
                innerSize.X + style.Border.TotalX,
                innerSize.Y + style.Border.TotalY);

            var origin = style.Origin;
            if (style.TextAlign == TextAlignment.Left)
                textPos = new Point(textPos.X, textPos.Y);
            else if (style.TextAlign == TextAlignment.Center)
                textPos = new Point(textPos.X + (outerSize.X / 2) - (textSize.X / 2), textPos.Y);
            else if (style.TextAlign == TextAlignment.Right)
                textPos = new Point(textPos.X + outerSize.X - style.Padding.TotalX - style.Border.TotalX - textSize.X, textPos.Y);

            var rect = new Rectangle(pos.X, pos.Y, outerSize.X, outerSize.Y);
            var isMouseOver = rect.Contains(Utils.Vec2ToPoint(MousePosition));

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

            if (!eventData.Equals(EventData.Default))
            {
                if ((!prevHover && hover || !prevFocus && focus) || NextEventTargetName == eventData.Name)
                    CurrentEventTarget = eventData;

                if (!prevHover && hover) Events(EventType.OnMouseEnter, eventData);
                if (prevHover && !hover) Events(EventType.OnMouseLeave, eventData);
                if (!prevFocus && focus) Events(EventType.OnFocus, eventData);
                if (prevFocus && !focus) Events(EventType.OnLostFocus, eventData);

                if (eventData.DetectLeftMouse && MouseLeftJustPressed() && prevHover && hover)
                    Events(EventType.OnPress, eventData);
                else if (eventData.DetectRightMouse && MouseRightJustPressed() && prevHover && hover)
                    Events(EventType.OnRightPress, eventData);
                else if (eventData.DetectMiddleMouse && MouseMiddleJustPressed() && prevHover && hover)
                    Events(EventType.OnMiddlePress, eventData);

                if (eventData.DetectLeftMouse && MouseLeftJustReleased() && prevHover && hover)
                {
                    Events(EventType.OnClick, eventData);
                    Active = Hash.Empty;
                }
                else if (eventData.DetectRightMouse && MouseRightJustReleased() && prevHover && hover)
                {
                    Events(EventType.OnRightClick, eventData);
                    Active = Hash.Empty;
                }
                else if (eventData.DetectMiddleMouse && MouseMiddleJustReleased() && prevHover && hover)
                {
                    Events(EventType.OnMiddleClick, eventData);
                    Active = Hash.Empty;
                }
            }

            Commands.Add(new DrawCommand
            {
                Id = id,
                Hover = Hover == id,
                Focus = Focus == id,
                Active = Active == id,
                Type = DrawType.Texture,
                Texture = DotTexture,
                Position = pos,
                Size = outerSize,
                Color = style.BorderColor,
                Rotation = style.Rotation,
                HoverColor = style.HoverBorderColor,
                FocusColor = style.FocusBorderColor,
                ActiveColor = style.ActiveBorderColor
            });

            Commands.Add(new DrawCommand
            {
                Id = id,
                Hover = Hover == id,
                Focus = Focus == id,
                Active = Active == id,
                Type = DrawType.Texture,
                Texture = DotTexture,
                Position = innerPos,
                Size = innerSize,
                Color = style.Color,
                Rotation = style.Rotation,
                HoverColor = style.HoverColor,
                FocusColor = style.FocusColor,
                ActiveColor = style.ActiveColor
            });

            Commands.Add(new DrawCommand
            {
                Id = id,
                Hover = Hover == id,
                Focus = Focus == id,
                Active = Active == id,
                Type = DrawType.String,
                Position = textPos,
                Size = textSize,
                Color = style.TextColor,
                Rotation = style.Rotation,
                HoverColor = style.HoverTextColor,
                FocusColor = style.FocusTextColor,
                ActiveColor = style.ActiveTextColor,
                Text = label,
                Font = font
            });

            ExpandLayout(outerSize);
        }
    }
}
