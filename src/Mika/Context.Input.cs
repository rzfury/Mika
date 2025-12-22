using Microsoft.Xna.Framework.Input;

namespace Mika
{
    public partial class Context
    {
        internal bool MouseLeftJustPressed()
        {
            return PrevMouseState.LeftButton == ButtonState.Released && MouseState.LeftButton == ButtonState.Pressed;
        }

        internal bool MouseRightJustPressed()
        {
            return PrevMouseState.RightButton == ButtonState.Released && MouseState.RightButton == ButtonState.Pressed;
        }

        internal bool MouseMiddleJustPressed()
        {
            return PrevMouseState.MiddleButton == ButtonState.Released && MouseState.MiddleButton == ButtonState.Pressed;
        }

        internal bool MouseLeftJustReleased()
        {
            return PrevMouseState.LeftButton == ButtonState.Pressed && MouseState.LeftButton == ButtonState.Released;
        }

        internal bool MouseRightJustReleased()
        {
            return PrevMouseState.RightButton == ButtonState.Pressed && MouseState.RightButton == ButtonState.Released;
        }

        internal bool MouseMiddleJustReleased()
        {
            return PrevMouseState.MiddleButton == ButtonState.Pressed && MouseState.MiddleButton == ButtonState.Released;
        }
    }
}
