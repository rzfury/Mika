using System;

namespace Mika
{
    public partial class Context
    {
        public void SetFont(object font)
        {
            if (TextController == null)
                throw new Exception("Cannot call SetFont, TextController has not been initialized.");

            TextController.SetFont(font);
        }
    }
}
