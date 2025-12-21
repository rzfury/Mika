using FontStashSharp;
using Microsoft.Xna.Framework.Graphics;

namespace Mika
{
    public partial class Context
    {
        private static Texture2D _dotTexture = null;

        /// <summary>
        /// A texture used for solid color and default texture.
        /// </summary>
        public static Texture2D DotTexture
        {
            get { return _dotTexture; }
            internal set { _dotTexture = value; }
        }

        private static SpriteFontBase _defaultFont = null;

        public static SpriteFontBase DefaultFont
        {
            get { return _defaultFont; }
            internal set { _defaultFont = value; }
        }
    }
}
