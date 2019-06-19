using System.Text.RegularExpressions;

namespace Pspl.Shared.Extensions
{
    public static class StringExt
    {
        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        public static string StripTagsRegexCompiled(this string source)
            => _htmlRegex.Replace(source, string.Empty);
    }
}