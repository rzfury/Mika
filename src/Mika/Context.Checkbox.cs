using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mika
{
    public partial class Context
    {
        public void Checkbox(
            string label,
            bool value,
            Style style = default,
            Style textStyle = default,
            EventData eventData = default)
        {
            Checkbox(value, eventData: eventData);
            SameLine();
            Text(label, style: textStyle);
        }

        public void Checkbox(
            bool value,
            Style style = default,
            EventData eventData = default)
        {
            var id = !string.IsNullOrEmpty(eventData.Name)
                ? GetId(eventData.Name)
                : GetId();

            var layout = PeekLayout();
            var pos = layout.Cursor;
            var font = style.Font ?? DefaultFont;
            var border = style.Border != default ? style.Border : Theme.BorderSize;
            var size = style.Size != default ? style.Size : Theme.CheckboxSize;
            var tickSize = style.Size != default
                ? new Point(style.Size.X * 85 / 100, style.Size.Y * 85 / 100)
                : Theme.CheckboxTickSize;

            var finalSize = size + border.TotalXY;

            var rect = new Rectangle(pos.X, pos.Y, finalSize.X, finalSize.Y);
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

            if (!eventData.Equals(DefaultValues.EventData))
            {
                if ((!prevHover && hover || !prevFocus && focus) || NextEventTargetName == eventData.Name)
                    CurrentEventTarget = eventData;

                if (!prevHover && hover) Events(EventType.OnMouseEnter, eventData, value);
                if (prevHover && !hover) Events(EventType.OnMouseLeave, eventData, value);
                if (!prevFocus && focus) Events(EventType.OnFocus, eventData, value);
                if (prevFocus && !focus) Events(EventType.OnLostFocus, eventData, value);

                if (eventData.DetectLeftMouse && MouseLeftJustReleased() && prevHover && hover)
                {
                    Events(EventType.OnClick, eventData, value);
                    Events(EventType.OnChange, eventData, value);
                    Active = Hash.Empty;
                }
                else if (eventData.DetectRightMouse && MouseRightJustReleased() && prevHover && hover)
                {
                    Events(EventType.OnRightClick, eventData, value);
                    Active = Hash.Empty;
                }
                else if (eventData.DetectMiddleMouse && MouseMiddleJustReleased() && prevHover && hover)
                {
                    Events(EventType.OnMiddleClick, eventData, value);
                    Active = Hash.Empty;
                }
            }

            // Border
            Commands.Add(new DrawCommand
            {
                Id = id,
                Hover = Hover == id,
                Focus = Focus == id,
                Active = Active == id,
                Type = DrawCommandType.Texture,
                Texture = DotTexture,
                Position = pos,
                Size = finalSize,
                Color = style.BorderColor != default ? style.BorderColor : Theme.BorderColor,
                HoverColor = style.BorderHoverColor != default ? style.BorderHoverColor : Theme.BorderHoverColor,
                FocusColor = style.BorderFocusColor != default ? style.BorderFocusColor : Theme.BorderHoverColor,
                ActiveColor = style.BorderActiveColor != default ? style.BorderActiveColor : Theme.BorderActiveColor,
            });

            // Base Color
            Commands.Add(new DrawCommand
            {
                Id = id,
                Hover = Hover == id,
                Focus = Focus == id,
                Active = Active == id,
                Type = DrawCommandType.Texture,
                Texture = DotTexture,
                Position = pos + new Point(border.Left, border.Top),
                Size = size,
                Color = style.Color != default ? style.Color : Theme.BaseColor,
                HoverColor = style.HoverColor != default ? style.HoverColor : Theme.BaseHoverColor,
                FocusColor = style.FocusColor != default ? style.FocusColor : Theme.BaseHoverColor,
                ActiveColor = style.ActiveColor != default ? style.ActiveColor : Theme.BaseActiveColor,
            });

            // Tick Color
            Commands.Add(new DrawCommand
            {
                Id = id,
                Hidden = !value,
                Hover = Hover == id,
                Focus = Focus == id,
                Active = Active == id,
                Type = DrawCommandType.Texture,
                Texture = DotTexture,
                Position = pos + new Point(border.Left + (size.X - tickSize.X) / 2, border.Top + (size.Y - tickSize.Y) / 2),
                Size = tickSize,
                Color = style.Color != default ? style.Color : Theme.PrimaryColor,
                HoverColor = style.HoverColor != default ? style.HoverColor : Theme.PrimaryHoverColor,
                FocusColor = style.FocusColor != default ? style.FocusColor : Theme.PrimaryHoverColor,
                ActiveColor = style.ActiveColor != default ? style.ActiveColor : Theme.PrimaryActiveColor,
            });

            ExpandLayout(finalSize);
        }
    }
}
