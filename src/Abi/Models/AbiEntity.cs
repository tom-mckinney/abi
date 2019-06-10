using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Models
{
    public abstract class AbiEntity
    {
        public int Id { get; set; }

        public DateTime? CreatedUtc { get; set; }

        public DateTime? ModifiedUtc { get; set; }
    }
}
