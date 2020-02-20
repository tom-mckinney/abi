using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Models
{
    public class Experiment<TKey> : AbiEntity<TKey>
    {
        public string Name { get; set; } = null!;
    }

    public class Experiment : Experiment<int>
    {
    }
}
