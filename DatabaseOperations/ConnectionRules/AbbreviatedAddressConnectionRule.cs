﻿namespace DatabaseOperations.ConnectionRules
{
    using DataTransferObjects;
    using Extensions;
    using Interfaces;

    internal class AbbreviatedAddressConnectionRule : IConnectionRule
    {
        private const string AbbreviatedAddressLookUp = "addr";

        public bool Check(string item)
        {
            return item.ToLower()
                .Contains(AbbreviatedAddressLookUp);
        }

        public ConnectionOptions ApplyChange(
            ConnectionOptions options,
            string item)
        {
            if (string.IsNullOrWhiteSpace(options.Server)) options.Server = AbbreviatedAddressLookUp.ToValue(item);
            return options;
        }

        public ConnectionProperties ApplyChange(
            ConnectionProperties properties,
            string item)
        {
            if (string.IsNullOrWhiteSpace(properties.Server)) properties.Server = AbbreviatedAddressLookUp.ToValue(item);
            return properties;
        }
    }
}