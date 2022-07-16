namespace DatabaseOperations.Extensions
{
    using Useful.Extensions;

    internal static class StringValueExtensions
    {
        private const string EqualSymbol = "=";

        internal static string ToValue(
            this string key,
            string value)
        {
            string searchString = key + EqualSymbol;
            string? newValue = value.SubstringAfterValue(searchString);
            return newValue;
        }
    }
}