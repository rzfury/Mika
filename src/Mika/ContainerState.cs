using Microsoft.Xna.Framework;

namespace Mika
{
    public struct ContainerState
    {
        public int DrawCommandIndex;
        public Point Size;
        public Edges Padding;
        public Edges BorderSize;
        public int LayoutSpacing;
        public Style Style;
        public Point StartingCursor;

        // ButtonLayout
        public uint Id;
        public Rectangle ClickableArea;
        public EventData EventData;
    }
}
