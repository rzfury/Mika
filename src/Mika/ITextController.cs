using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mika
{
    public interface ITextController
    {
        object GetFont();
        Point MeasureString(string text);
        void Draw(SpriteBatch spriteBatch);
    }
}
