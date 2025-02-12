namespace FireballEngine.Blazor
{
    internal static class ShaderCounter
    {
        private static int _count = 0;
        public static int Count
        {
            get
            {
                return ++_count;
            }
        }
    }
}