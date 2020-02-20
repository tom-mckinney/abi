using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Models
{
    public class Visitor : AbiEntity
    {
        public string VisitorId { get; set; } = null!;

        public int? UserId { get; set; }

        public IEnumerable<Session> Sessions { get; set; } = null!;
    }
}
