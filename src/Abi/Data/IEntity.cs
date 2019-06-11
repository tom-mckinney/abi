using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Data
{
    public interface IEntity<TKey> where TKey : struct
    {
        TKey Id { get; set; }
    }
}
