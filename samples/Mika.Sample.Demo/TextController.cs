using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mika.Sample.Demo.FNA
{
    public class TextController : ITextController
    {
        private SpriteFontBase _currentFont = null;

        object ITextController.GetFont()
        {
            return _currentFont;
        }

        void ITextController.SetFont(object font)
        {
            _currentFont = (SpriteFontBase)font;
        }

        Point ITextController.MeasureString(string text)
        {
            var size = _currentFont.MeasureString(text);
            return new Point((int)size.X, (int)size.Y);
        }

        void ITextController.Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, DrawCommand drawCommand)
        {
            spriteBatch.DrawString((SpriteFontBase)drawCommand.Font, drawCommand.Text, Utils.PointToVec2(drawCommand.Position), drawCommand.Color);
        }
    }
}
