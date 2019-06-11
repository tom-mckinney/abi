﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Models
{
    public class Visitor : AbiEntity
    {
        public int? UserId { get; set; }

        public virtual IEnumerable<Session> Sessions { get; set; }
    }
}