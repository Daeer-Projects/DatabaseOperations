namespace DatabaseOperations.Tests.TestTypes
{
    using System;

    public sealed class Type
    {
        public int Identity { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.MinValue;
    }
}