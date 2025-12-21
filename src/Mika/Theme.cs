using Microsoft.Xna.Framework;

namespace Mika
{
    public class Theme
    {
        public int LayoutSpacing = 4;

        public Edges ContainerPadding = Edges.All(0);
        public Edges TextPadding = Edges.LTRB(4, 4, 0, 4);

        public Color TextColor = new Color(230, 230, 230);
        public Color TextHoverColor = new Color(230, 230, 230);
        public Color TextActiveColor = new Color(230, 230, 230);

        public Color PanelColor = new Color(50, 50, 50);

        public Color BaseColor = new Color(30, 30, 30);
        public Color BaseHoverColor = new Color(35, 35, 35);
        public Color BaseActiveColor = new Color(40, 40, 40);

        public Color PrimaryColor = new Color(75, 75, 75);
        public Color PrimaryHoverColor = new Color(95, 95, 95);
        public Color PrimaryActiveColor = new Color(115, 115, 115);

        public Edges BorderSize = Edges.All(2);
        public Color BorderColor = new Color(25, 25, 25);
        public Color BorderHoverColor = new Color(25, 25, 25);
        public Color BorderActiveColor = new Color(25, 25, 25);

        public int ScrollTrackSize = 16;
        public int ScrollThumbSize = 16;
        public int ScrollThumbMinSize = 10;
        public Color ScrollThumbColor = new Color(75, 75, 75);
        public Color ScrollThumbHoverColor = new Color(95, 95, 95);
        public Color ScrollThumbActiveColor = new Color(115, 115, 115);

        public Point SliderSize = new Point(128, 24);
        public int SliderThumbWidth = 16;

        public Point CheckboxSize = new Point(24, 24);
        public Point CheckboxTickSize = new Point(20, 20);

        public int DividerSize = 2;

        internal float Opacity = 1.0f;
    }
}
