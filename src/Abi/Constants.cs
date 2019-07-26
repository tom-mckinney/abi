using System;
using System.Collections.Generic;
using System.Text;

namespace Abi
{
    public static class Constants
    {
        public static class Cookies
        {
            public const string Prefix = "abi";
            public const string Experiment = "experiment";
            public const string Session = "session";
            public const string Variant = "variant";
            public const string Visitor = "visitor";
        }

        public static class Types
        {
            public const string Experiment = "Experiment";
            public const string ContentVariant = "ContentVariant";
        }

        public static class CustomTables
        {
            public const string Encounters = "Encounters";
        }
    }
}
