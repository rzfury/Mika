using Microsoft.Xna.Framework;

namespace Mika
{
    public struct ScrollState
    {
        public uint Id;

        public int ScrollX;
        public int ScrollY;

        public int PrevScrollX;
        public int PrevScrollY;

        public int MaxScrollX;
        public int MaxScrollY;


        public int ContentHeight;
        public int ViewHeight;
        public bool IsDraggingVScrollbar;
        public int ScrollbarDragOffsetY;

        public bool IsDraggingHScrollbar;

        public bool AutoScroll;

        public Style ScrollbarStyle;
        public EventData EventData;
    }
}
