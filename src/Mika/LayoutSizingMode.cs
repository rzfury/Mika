namespace Mika
{
    public enum LayoutSizingMode
    {
        /// <summary>
        /// Default sizing mode. Automatically grew the layout size whenever a widget is drawn. Will <b>disable widget alignments</b>.
        /// </summary>
        Auto,
        /// <summary>
        /// Fix the layout size. Oversized widgets will be clipped. Allow widget alignments.
        /// </summary>
        Fixed
    }
}