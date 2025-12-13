using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mika
{
    public partial class Context
    {
        /// <summary>
        /// Prepare default assets used by Mika.
        /// </summary>
        public void Prepare(
            GraphicsDevice graphicsDevice,
            SpriteFontBase defaultFont)
        {
            var dot = new Texture2D(graphicsDevice, 1, 1);
            dot.SetData(new Color[] { Color.White });
            DotTexture = dot;
            DefaultFont = defaultFont;
        }

        /// <summary>
        /// Basic default renderer if you choose to not create it yourself.
        /// </summary>
        public void Render(SpriteBatch spriteBatch)
        {
            while (TryDequeueCommand(out var command))
            {
                if (command.Hidden) continue;

                var pos = command.Position;
                var size = command.Size;
                var color = command.Color;

                var rect = Utils.RectFromPosAndSize(pos, size);
                var posVec = Utils.PointToVec2(pos);

                if (command.Hover && command.HoverColor != default) color = command.HoverColor;
                if (command.Focus && command.FocusColor != default) color = command.FocusColor;
                if (command.Active && command.ActiveColor != default) color = command.ActiveColor;

                switch (command.Type)
                {
                    case DrawType.Texture:
                        if (command.SourceRect != default)
                            spriteBatch.Draw(command.Texture, rect, command.SourceRect, color);
                        else
                            spriteBatch.Draw(command.Texture, rect, color);
                        break;
                    case DrawType.String:
                        spriteBatch.DrawString(command.Font, command.Text, posVec, color, rotation: command.Rotation, origin: Utils.PointToVec2(command.Origin));
                        break;
                    case DrawType.RTL:
                        command.RTL.Draw(spriteBatch, posVec, color);
                        break;
                    default: break;
                }
            }
        }
    }
}
