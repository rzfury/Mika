using Microsoft.Xna.Framework;

namespace Mika
{
    public partial class Context
    {
        public void BeginClip(Rectangle rect)
        {
            Rectangle final = rect;

            if (ClippingStack.Count > 0)
                final = Rectangle.Intersect(ClippingStack.Peek().Rect, rect);

            ClippingStack.Push(new ClippingState
            {
                DrawCommandIndex = Commands.Count,
                Rect = final
            });
            Commands.Add(new DrawCommand
            {
                Type = DrawCommandType.SetClipping,
                ClippingRect = final,
            });
        }

        public void EndClip()
        {
            _ = ClippingStack.Pop();
            Commands.Add(new DrawCommand
            {
                Type = DrawCommandType.ResetClipping,
            });
        }

        public void EndClip(Rectangle finalRect)
        {
            var clipping = ClippingStack.Pop();

            var command = Commands[clipping.DrawCommandIndex];
            command.ClippingRect = finalRect;

            Commands[clipping.DrawCommandIndex] = command;

            Commands.Add(new DrawCommand
            {
                Type = DrawCommandType.ResetClipping,
            });
        }
    }
}
