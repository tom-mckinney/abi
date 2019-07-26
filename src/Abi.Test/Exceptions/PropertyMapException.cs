using System;
using System.Collections.Generic;
using System.Text;

namespace Xunit.Sdk
{
    public class PropertyMapException : XunitException
    {
        public PropertyMapException(string message, Type originType, Type mapType) : this(message, originType, mapType, null)
        {
        }

        public PropertyMapException(string message, Type originType, Type mapType, Exception innerException)
            : base(FormatMessage(message, originType, mapType), innerException)
        {
        }

        private static string FormatMessage(string message, Type originType, Type mapType)
        {
            return $"{message}\nAttempted to map from {originType?.Name} to {mapType?.Name}.";
        }
    }
}