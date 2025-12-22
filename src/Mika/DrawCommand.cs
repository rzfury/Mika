using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mika
{
    public struct DrawCommand
    {
        public uint Id;
        public DrawCommandType Type;
        public bool Hidden;
        public bool Active;
        public bool Hover;
        public bool Focus;
        public Point Position;
        public Point Size;
        public Color Color;
        public float Opacity;
        public Point Origin;
        public float Rotation;
        public int ZIndex;

        public Color HoverColor;
        public Color FocusColor;
        public Color ActiveColor;

        public Rectangle ClippingRect;

        public Texture2D Texture;
        public Rectangle SourceRect;

        public string Text;
        public object Font;

        public object Metadata;
    }
}