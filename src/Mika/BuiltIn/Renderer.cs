using FontStashSharp;
using Microsoft.Xna.Framework.Graphics;

namespace Mika.BuiltIn
{
    public static class Renderer
    {
        /// <summary>
        /// Built in basic renderer if you don't need to create it yourself.
        /// </summary>
        public static void Render(SpriteBatch spriteBatch, Context mika)
        {
            while (mika.TryDequeueCommand(out var command))
            {
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
