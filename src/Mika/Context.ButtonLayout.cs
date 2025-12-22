using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Mika
{
    public partial class Context
    {
        public void ButtonLayout()
        {
            ButtonLayout(LayoutType.Horizontal, LayoutSizingMode.Auto, Point.Zero, DefaultValues.Style, DefaultValues.EventData);
        }

        public void ButtonLayout(EventData eventData)
        {
            ButtonLayout(LayoutType.Horizontal, LayoutSizingMode.Auto, Point.Zero, DefaultValues.Style, eventData);
        }

        public void ButtonLayout(Style style, EventData eventData)
        {
            ButtonLayout(LayoutType.Horizontal, LayoutSizingMode.Auto, Point.Zero, style, eventData);
        }

        public void ButtonLayout(LayoutType layoutType, EventData eventData)
        {
            ButtonLayout(layoutType, LayoutSizingMode.Auto, Point.Zero, DefaultValues.Style, eventData);
        }

        public void ButtonLayout(LayoutType layoutType, Style style, EventData eventData)
        {
            ButtonLayout(layoutType, LayoutSizingMode.Auto, Point.Zero, style, eventData);
        }

        public void ButtonLayout(LayoutSizingMode layoutSizingMode, Point size, EventData eventData)
        {
            ButtonLayout(LayoutType.Horizontal, layoutSizingMode, size, DefaultValues.Style, eventData);
        }

        public void ButtonLayout(
            LayoutType layoutType,
            LayoutSizingMode sizingMode,
            Point size,
            Style style,
            EventData eventData)
        {
            var id = GetId();
            var layout = PeekLayout();
            var startPos = layout.Cursor;
            var spacing = style.Spacing > 0 ? style.Spacing : Theme.LayoutSpacing;
            var posOffset = new Point(startPos.X, startPos.Y);

            ContainerStack.Push(new ContainerState
            {
                DrawCommandIndex = Commands.Count,
                Size = size,
                Padding = default,
                BorderSize = default,
                LayoutSpacing = spacing,
                Style = style,
                StartingCursor = startPos,
                Id = id,
                Interactable = true,
            });

            Layout(layoutType, sizingMode, size, spacing);
            var newLayout = LayoutStack.Pop();
            newLayout.Cursor = posOffset;
            newLayout.Anchor = posOffset;
            LayoutStack.Push(newLayout);

            if (sizingMode == LayoutSizingMode.Fixed)
            {
                ClippingStack.Pop();
                BeginClip(Utils.RectFromPosAndSize(posOffset, size));
            }
        }

        public void CloseButtonLayout()
        {
            var button = ContainerStack.Pop();
            var layout = LayoutStack.Pop(); // Layout is closed by this

            int sizeX = 0, sizeY = 0;
            if (layout.SizingMode == LayoutSizingMode.Fixed)
            {
                sizeX = layout.Size.X + button.Padding.TotalX + button.BorderSize.TotalX;
                sizeY = layout.Size.Y + button.Padding.TotalY + button.BorderSize.TotalY;
            }
            else
            {
                if (layout.Type == LayoutType.Vertical)
                {
                    sizeX = layout.Size.X + button.Padding.TotalX + button.BorderSize.TotalX;
                    sizeY = (layout.Cursor.Y - button.StartingCursor.Y) + button.Padding.Bottom + button.BorderSize.Bottom - button.LayoutSpacing;
                    if (sizeY < 0) sizeY = button.Padding.TotalY + button.BorderSize.TotalY;
                }
                else if (layout.Type == LayoutType.Horizontal)
                {
                    sizeX = (layout.Cursor.X - button.StartingCursor.X) + button.Padding.Right + button.BorderSize.Right - button.LayoutSpacing;
                    sizeY = layout.Size.Y + button.Padding.TotalY + button.BorderSize.TotalY;
                    if (sizeX < 0)
                        sizeX = button.Padding.TotalX + button.BorderSize.TotalX;
                }
            }

            ExpandLayout(new Point(sizeX, sizeY));

            #region Interaction

            if (button.Interactable)
            {
                var id = button.Id;
                var clickableArea = button.ClickableArea;
                var eventData = button.EventData;

                var rect = new Rectangle(
                    button.StartingCursor.X + button.ClickableArea.X,
                    button.StartingCursor.Y + button.ClickableArea.Y,
                    clickableArea.Width > 0 ? clickableArea.Width : sizeX,
                    clickableArea.Height > 0 ? clickableArea.Height : sizeY);
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
            }

            #endregion
        }
    }
}
