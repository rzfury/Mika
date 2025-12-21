namespace Mika
{
    public enum DrawCommandType
    {
        None,
        Texture,
        String,
        SetClipping,
        ResetClipping,
        /// <summary>
        /// Will use <c>FontStashSharp.RichTextLayout</c>
        /// </summary>
        RTL
    }

}