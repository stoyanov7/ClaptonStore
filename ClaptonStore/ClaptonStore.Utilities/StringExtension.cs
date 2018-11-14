namespace ClaptonStore.Utilities
{
    public static class StringExtension
    {
        public static string GetShortDescription(this string desc)
        {
            return $"{desc.Substring(0, 100)} . . .";
        }
    }
}