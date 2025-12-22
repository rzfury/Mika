using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mika
{
    public interface ITextController
    {
        object GetFont();
        void SetFont(object font);
        Point MeasureString(string text);
        void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, DrawCommand drawCommand);
    }
}
