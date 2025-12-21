namespace Mika
{
    public partial class Context
    {
        /// <summary>
        /// Allow pushing custom draw command to render custom widget
        /// </summary>
        /// <param name="drawCommand"></param>
        public int PushDrawCommand(DrawCommand drawCommand)
        {
            var index = Commands.Count;
            Commands.Add(drawCommand);
            return index;
        }

        public void UpdateDrawCommand(int index, DrawCommand newDrawCommand)
        {
            if (index >= Commands.Count || index < 0)
                throw new System.IndexOutOfRangeException(string.Format("Draw command of index {0} is out of range.", index));

            Commands[index] = newDrawCommand;
        }

        public void PushLayout(LayoutState layout)
        {
            LayoutStack.Push(layout);
        }

        public LayoutState PopLayout()
        {
            if (ContainerStack.Count == 0)
                throw new System.Exception("Cannot pop empty layout stack.");

            return LayoutStack.Pop();
        }

        public void PushContainer(ContainerState container)
        {
            ContainerStack.Push(container);
        }

        public ContainerState PopContainer()
        {
            if (ContainerStack.Count == 0)
                throw new System.Exception("Cannot pop empty container stack.");
            return ContainerStack.Pop();
        }
    }
}
