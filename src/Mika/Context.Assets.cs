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
    }
}
