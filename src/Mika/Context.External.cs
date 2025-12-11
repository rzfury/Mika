namespace Mika
{
    public partial class Context
    {
        /// <summary>
        /// Allow pushing custom draw command to render custom widget
        /// </summary>
        /// <param name="drawCommand"></param>
        public void PushDrawCommand(DrawCommand drawCommand)
        {
            Commands.Add(drawCommand);
        }
    }
}
