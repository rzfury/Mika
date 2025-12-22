using Microsoft.Xna.Framework;

namespace Mika
{
    public static class DefaultValues
    {
        public static readonly Color Color = new Color(255, 255, 255, 0);

        public static readonly Edges Edges = Edges.All(-1);

        public static readonly LayoutState LayoutState = new LayoutState
        {
            Type = LayoutType.Vertical,
            Size = Point.Zero,
            Spacing = -1,
            SizingMode = LayoutSizingMode.Auto,
            Cursor = Point.Zero,
        };

        public static readonly EventData EventData = new EventData
        {
            Name = "",
            NavigateLeftTarget = "",
            NavigateRightTarget = "",
            NavigateUpTarget = "",
            NavigateDownTarget = "",
            DetectLeftMouse = true,
            DetectRightMouse = false,
            DetectMiddleMouse = false,
        };

        public static readonly Style Style = new Style
        {
            Origin = new Point(int.MinValue, int.MinValue),
            Rotation = float.MinValue,
            Size = new Point(int.MinValue, int.MinValue),
            Padding = Edges.All(int.MinValue),
            Border = Edges.All(int.MinValue),
            Hidden = false,
            ZIndex = 0,

            Color = Color,
            HoverColor = Color,
            FocusColor = Color,
            ActiveColor = Color,
            Opacity = float.MinValue,

            BorderColor = Color,
            BorderHoverColor = Color,
            BorderFocusColor = Color,
            BorderActiveColor = Color,
            BorderOpacity = float.MinValue,

            TextColor = Color,
            TextHoverColor = Color,
            TextFocusColor = Color,
            TextActiveColor = Color,
            TextOpacity = float.MinValue,
            TextAlign = TextAlignment.Left,

            SliderWidth = int.MinValue,
            SliderHeight = int.MinValue,
            SliderThumbSize = int.MinValue,
            SliderThumbColor = Color,
            SliderThumbHoverColor = Color,
            SliderThumbFocusColor = Color,
            SliderThumbActiveColor = Color,
            SliderStep = 0,

            Spacing = int.MinValue,
            GridSpacing = new Point(int.MinValue, int.MinValue),
        };
    }
}
