using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Models
{
    public class Session : AbiEntity
    {
        public string PublicId { get; set; }

        public int VisitorId { get; set; }

        public virtual Visitor Visitor { get; set; }

        public string DeviceName { get; set; }
    }
}
