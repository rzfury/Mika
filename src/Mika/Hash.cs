using System.Text;

namespace Mika
{
    public static class Hash
    {
        public const uint Prime = 16777619u;
        public const uint Offset = 2166136261u;

        public static uint Empty => Offset;

        public static uint Of<T>(T input, uint offset = Offset)
        {
            uint hash = offset;

            if (input is string str)
            {
                if (string.IsNullOrEmpty(str)) return hash;

                byte[] bytes = Encoding.UTF8.GetBytes(str);
                for (int i = 0; i < bytes.Length; i++)
                    hash = (hash ^ bytes[i]) * Prime;

                return hash;
            }

            if (input is int numInt)
            {
                byte[] bytes = System.BitConverter.GetBytes(numInt);
                for (int i = 0; i < bytes.Length; i++)
                    hash = (hash ^ bytes[i]) * Prime;
                return hash;
            }

            throw new System.NotImplementedException(string.Format("There's no implementation for '{0}' type", input != null ? input.GetType().ToString() : "Unknown"));
        }

        public static uint Combine(params uint[] hashes)
        {
            if (hashes == null)
                return Offset;

            uint hash = Offset;

            for (int i = 0; i < hashes.Length; i++)
                hash = (hash ^ hashes[i]) * Prime;

            return hash;
        }

        public static uint Join(params string[] strs)
        {
            if (strs == null)
                return Offset;

            uint hash = Of(strs[0]);

            for (int i = 1; i < strs.Length; i++)
                hash = Of(strs[i], hash);

            return hash;
        }
    }

}