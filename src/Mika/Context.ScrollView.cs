using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Mika
{
    public partial class Context
    {
        public void ScrollView()
        {
            ScrollView(false, DefaultValues.Style, DefaultValues.EventData);
        }

        public void ScrollView(Style style)
        {
            ScrollView(false, style, DefaultValues.EventData);
        }

        public void ScrollView(EventData eventData)
        {
            ScrollView(false, DefaultValues.Style, eventData);
        }

        public void ScrollView(EventData eventData, Style style)
        {
            ScrollView(false, style, eventData);
        }

        public void ScrollView(bool autoScroll)
        {
            ScrollView(autoScroll, DefaultValues.Style, DefaultValues.EventData);
        }

        public void ScrollView(bool autoScroll, Style style)
        {
            ScrollView(autoScroll, style, DefaultValues.EventData);
        }

        public void ScrollView(bool autoScroll, Style scrollBarStyle, EventData eventData)
        {
            var id = GetId((_autoIdScrollableIndex--).ToString());
            var layout = PeekLayout();

            if (!ScrollStates.ContainsKey(id))
                ScrollStates.Add(id, default);

            var state = ScrollStates[id];

            OffsetLayout(new Point(0, -state.ScrollY));

            state.ContentHeight = 0;
            state.ViewHeight = layout.Size.Y;
            state.AutoScroll = autoScroll;
            state.EventData = eventData;
            state.PrevScrollY = state.ScrollY;
            state.ScrollbarStyle = scrollBarStyle;

            ScrollStates[id] = state;

            ScrollStack.Push(id);
        }

        public void CloseScrollView()
        {
            var id = ScrollStack.Pop();
            var layout = PeekLayout();

            var state = ScrollStates[id];
            var style = state.ScrollbarStyle;

            state.ContentHeight = layout.RealSize.Y;
            state.MaxScrollY = Math.Max(0, layout.RealSize.Y - layout.Size.Y);

            var layoutRect = Utils.RectFromPosAndSize(layout.Position, layout.Size);
            var mousePos = Utils.Vec2ToPoint(MousePosition);
            var isMouseInside = layoutRect.Contains(mousePos);

            if (isMouseInside)
            {
                var wheelDiff = PrevMouseState.ScrollWheelValue - MouseState.ScrollWheelValue;
                var wheelDirection = Utils.Clamp(wheelDiff, -1, 1);
                state.ScrollY += ScrollSpeed * wheelDirection;
            }

            state.ScrollY = Utils.Clamp(state.ScrollY, 0, state.MaxScrollY);

            #region Vertical Scrollbar

            if (state.ContentHeight > layout.Size.Y)
            {
                var pos = layout.Position;

                var trackRect = new Rectangle(
                    pos.X + layout.Size.X - Theme.ScrollTrackSize,
                    pos.Y,
                    Theme.ScrollTrackSize,
                    layout.Size.Y);
                var thumbHeight = Utils.Clamp(
                    (int)(trackRect.Height * ((float)layout.Size.Y / layout.RealSize.Y)),
                    Theme.ScrollThumbMinSize,
                    trackRect.Height);
                var thumbY = (int)(trackRect.Y + (trackRect.Height - thumbHeight) * ((float)state.ScrollY / state.MaxScrollY));
                var thumbRect = new Rectangle(
                    trackRect.X,
                    thumbY,
                    Theme.ScrollThumbSize,
                    thumbHeight);

                var isMouseOverTrack = trackRect.Contains(mousePos);
                var isMouseOverThumb = thumbRect.Contains(mousePos);

                if (MouseState.LeftButton == ButtonState.Pressed && isMouseOverTrack)
                {
                    thumbRect.Y = mousePos.Y - (thumbRect.Height / 2);
                    state.ScrollY = (int)((mousePos.Y - trackRect.Y) / (float)trackRect.Height * state.MaxScrollY);
                }

                if (!state.IsDraggingVScrollbar && MouseState.LeftButton == ButtonState.Pressed && isMouseOverThumb)
                {
                    state.IsDraggingVScrollbar = true;
                    state.ScrollbarDragOffsetY = mousePos.Y - thumbRect.Y;
                }

                if (MouseState.LeftButton == ButtonState.Released)
                    state.IsDraggingVScrollbar = false;

                if (state.IsDraggingVScrollbar)
                {
                    var localY = mousePos.Y - trackRect.Y - state.ScrollbarDragOffsetY;
                    var t = localY / (float)(trackRect.Height - thumbRect.Height);
                    state.ScrollY = Utils.LerpInt(0, state.MaxScrollY, Utils.ClampF(t, 0.0f, 1.0f));
                }

                thumbRect.Y = Utils.Clamp(thumbRect.Y, trackRect.Top, trackRect.Bottom - thumbRect.Height);

                // Scroll Track
                var trackColor = style.Color != DefaultValues.Style.Color ? style.Color : Theme.BaseColor;
                var trackHoverColor = style.HoverColor != DefaultValues.Style.HoverColor ? style.HoverColor : Theme.BaseHoverColor;
                var trackActiveColor = style.ActiveColor != DefaultValues.Style.ActiveColor ? style.ActiveColor : Theme.BaseActiveColor;
                Commands.Add(new DrawCommand
                {
                    Type = DrawCommandType.Texture,
                    Id = id,
                    Hover = isMouseOverTrack,
                    Active = state.IsDraggingVScrollbar,
                    Hidden = style.Hidden,
                    Position = new Point(trackRect.X, trackRect.Y),
                    Size = new Point(trackRect.Width, trackRect.Height),
                    Texture = DotTexture,
                    Color = trackColor,
                    HoverColor = trackHoverColor,
                    ActiveColor = trackActiveColor,
                    Opacity = style.Opacity != DefaultValues.Style.Opacity ? style.Opacity : Theme.Opacity,
                });

                // Scroll Thumb
                var thumbColor = style.SliderThumbColor != DefaultValues.Style.Color ? style.Color : Theme.ScrollThumbColor;
                var thumbHoverColor = style.SliderThumbHoverColor != DefaultValues.Style.HoverColor ? style.HoverColor : Theme.ScrollThumbHoverColor;
                var thumbActiveColor = style.SliderThumbActiveColor != DefaultValues.Style.ActiveColor ? style.ActiveColor : Theme.ScrollThumbActiveColor;
                Commands.Add(new DrawCommand
                {
                    Type = DrawCommandType.Texture,
                    Id = id,
                    Hover = isMouseOverThumb,
                    Active = state.IsDraggingVScrollbar,
                    Hidden = style.Hidden,
                    Position = new Point(thumbRect.X, thumbRect.Y),
                    Size = new Point(thumbRect.Width, thumbRect.Height),
                    Texture = DotTexture,
                    Color = thumbColor,
                    HoverColor = thumbHoverColor,
                    ActiveColor = thumbActiveColor,
                    Opacity = style.Opacity != DefaultValues.Style.Opacity ? style.Opacity : Theme.Opacity,
                });
            }

            #endregion

            #region Horizontal Scrollbar

            // TODO

            #endregion

            if (state.PrevScrollY != state.ScrollY)
                Events?.Invoke(EventType.OnChange, state.EventData, new Point(state.ScrollX, state.ScrollY));

            ScrollStates[id] = state;
        }

        public ScrollState GetScrollState()
        {
            return ScrollStates[ScrollStack.Peek()];
        }
    }
}
