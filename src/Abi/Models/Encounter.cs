using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Models
{
    public class Encounter : AbiEntity
    {
        public int SessionId { get; set; }

        public virtual Session Session { get; set; }

        public string ContentVariantId { get; set; }
    }
}
