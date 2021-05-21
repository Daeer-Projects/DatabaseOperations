using Useful.Extensions;

namespace DatabaseOperations.Extensions
{
    internal static class StringValueExtensions
    {
        private const string EqualSymbol = "=";

        internal static string ToValue(this string key, string value)
        {
            var searchString = key + EqualSymbol;
            var newValue = value.SubstringAfterValue(searchString);
            return newValue;
        }
    }
}