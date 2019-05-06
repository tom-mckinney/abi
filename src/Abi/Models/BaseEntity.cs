using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset Updated { get; set; }
    }
}
