using System;

namespace ABPUtils
{
    public static class StringUtils
    {
        public static string FirstUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return Char.ToLowerInvariant(input[0]) + input.Substring(1);
        }
    }
}
