using Abi.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Models
{
    public abstract class AbiEntity : IEntity<int>
    {
        public int Id { get; set; }

        public DateTime? CreatedUtc { get; set; }

        public DateTime? ModifiedUtc { get; set; }
    }
}
