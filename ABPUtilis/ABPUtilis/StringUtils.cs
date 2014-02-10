using System;
using System.IO;

namespace ABPUtils
{
    public static class StringUtils
    {
        /// <summary>
        /// Upper the first char of input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return Char.ToLowerInvariant(input[0]) + input.Substring(1);
        }

        /// <summary>
        /// Get full path of relative path
        /// </summary>
        /// <param name="path1">relative Path</param>
        /// <returns></returns>
        public static string ToFullPath(this string path1)
        {
            var t = Environment.CurrentDirectory;
            return Path.GetFullPath(Path.Combine(t, path1));
        }

        /// <summary>
        /// Get full path of relative path
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static string ToFullPath(this String path1, string path2)
        {
            var t = Environment.CurrentDirectory;
            return Path.GetFullPath(Path.Combine(t, path1, path2));
        }
    }
}
