using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Models
{
    public class Encounter : AbiEntity
    {
        public string EncounterId { get; set; }

        public string SessionId { get; set; }

        public string VariantId { get; set; }
    }
}
