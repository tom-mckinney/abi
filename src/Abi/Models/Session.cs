using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Models
{
    public class Session : AbiEntity
    {
        public string SessionId { get; set; } = null!;

        public string VisitorId { get; set; } = null!;

        public Visitor Visitor { get; set; } = null!;

        public string? DeviceName { get; set; }
    }
}
