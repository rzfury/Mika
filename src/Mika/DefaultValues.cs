using FontStashSharp;
using Microsoft.Xna.Framework;

namespace Mika
{
    public static class DefaultValues
    {
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
            Rotation = int.MinValue,
            Size = new Point(int.MinValue, int.MinValue),
            Padding = Edges.All(int.MinValue),
            Border = Edges.All(0),
            Hidden = false,
            ZIndex = 0,

            Color = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            HoverColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            FocusColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            ActiveColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),

            BorderColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            BorderHoverColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            BorderFocusColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            BorderActiveColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),

            Font = null,
            TextColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            TextHoverColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            TextFocusColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            TextActiveColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            TextAlign = TextAlignment.Left,
            TextStyle = TextStyle.None,

            SliderWidth = int.MinValue,
            SliderHeight = int.MinValue,
            SliderThumbSize = int.MinValue,
            SliderThumbColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            SliderThumbHoverColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            SliderThumbFocusColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            SliderThumbActiveColor = new Color(int.MinValue, int.MinValue, int.MinValue, int.MinValue),
            SliderStep = 0,

            Spacing = int.MinValue,
            GridSpacing = new Point(int.MinValue, int.MinValue),
        };
    }
}
