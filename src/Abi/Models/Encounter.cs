using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Models
{
    public class Encounter : AbiEntity
    {
        public string EncounterId { get; set; } = null!;

        public string SessionId { get; set; } = null!;

        public string VariantId { get; set; } = null!;
    }
}
