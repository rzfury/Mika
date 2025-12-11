using FontStashSharp.RichText;
using System.Collections.Generic;

namespace Mika
{
    public struct Caches
    {
        public const int RTLCapacity = 32;

        public Dictionary<uint, RichTextLayout> RTL;

        public Caches(Dictionary<uint, RichTextLayout> rtl = null)
        {
            if (rtl == null)
            {
                RTL = new Dictionary<uint, RichTextLayout>(RTLCapacity);
            }
            else
            {
                RTL = rtl;
            }
        }
    }
}
