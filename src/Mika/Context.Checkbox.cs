using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mika
{
    public partial class Context
    {
        public void Checkbox(string label, bool value)
        {
            Checkbox(label, value, DefaultValues.Style, DefaultValues.EventData);
        }

        public void Checkbox(string label, bool value, EventData eventData)
        {
            Checkbox(label, value, DefaultValues.Style, eventData);
        }

        public void Checkbox(string label, bool value, Style style, EventData eventData)
        {
            Checkbox(value, style, eventData);
            SameLine();
            Text(label, style);
        }

        public void Checkbox(bool value, Style style, EventData eventData)
        {
            var id = !string.IsNullOrEmpty(eventData.Name)
                ? GetId(eventData.Name)
                : GetId();

            var layout = PeekLayout();
            var pos = layout.Cursor;
            var border = style.Border != DefaultValues.Style.Border ? style.Border : Theme.BorderSize;
            var size = style.Size != DefaultValues.Style.Size ? style.Size : Theme.CheckboxSize;
            var tickSize = style.Size != DefaultValues.Style.Size
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

                if (!prevHover && hover) Events?.Invoke(EventType.OnMouseEnter, eventData, value);
                if (prevHover && !hover) Events?.Invoke(EventType.OnMouseLeave, eventData, value);
                if (!prevFocus && focus) Events?.Invoke(EventType.OnFocus, eventData, value);
                if (prevFocus && !focus) Events?.Invoke(EventType.OnLostFocus, eventData, value);

                if (eventData.DetectLeftMouse && MouseLeftJustReleased() && prevHover && hover)
                {
                    Events?.Invoke(EventType.OnClick, eventData, value);
                    Events?.Invoke(EventType.OnChange, eventData, value);
                    Active = Hash.Empty;
                }
                else if (eventData.DetectRightMouse && MouseRightJustReleased() && prevHover && hover)
                {
                    Events?.Invoke(EventType.OnRightClick, eventData, value);
                    Active = Hash.Empty;
                }
                else if (eventData.DetectMiddleMouse && MouseMiddleJustReleased() && prevHover && hover)
                {
                    Events?.Invoke(EventType.OnMiddleClick, eventData, value);
                    Active = Hash.Empty;
                }
            }

            // Border
            CreateBorderDrawCommand(id, pos, size, border, style.BorderColor, style.BorderHoverColor, style.BorderFocusColor, style.BorderActiveColor, style.BorderOpacity);

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
                Color = style.Color != DefaultValues.Style.Color ? style.Color : Theme.BaseColor,
                HoverColor = style.HoverColor != DefaultValues.Style.HoverColor ? style.HoverColor : Theme.BaseHoverColor,
                FocusColor = style.FocusColor != DefaultValues.Style.FocusColor ? style.FocusColor : Theme.BaseHoverColor,
                ActiveColor = style.ActiveColor != DefaultValues.Style.ActiveColor ? style.ActiveColor : Theme.BaseActiveColor,
                Opacity = style.Opacity != DefaultValues.Style.Opacity ? style.Opacity : Theme.Opacity,
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
                Color = style.Color != DefaultValues.Style.Color ? style.Color : Theme.PrimaryColor,
                HoverColor = style.HoverColor != DefaultValues.Style.HoverColor ? style.HoverColor : Theme.PrimaryHoverColor,
                FocusColor = style.FocusColor != DefaultValues.Style.FocusColor ? style.FocusColor : Theme.PrimaryHoverColor,
                ActiveColor = style.ActiveColor != DefaultValues.Style.ActiveColor ? style.ActiveColor : Theme.PrimaryActiveColor,
                Opacity = style.Opacity != DefaultValues.Style.Opacity ? style.Opacity : Theme.Opacity,
            });

            ExpandLayout(finalSize);
        }
    }
}
