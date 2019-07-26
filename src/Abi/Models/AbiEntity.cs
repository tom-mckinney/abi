using Abi.Data;
using Abi.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Models
{
    public abstract class AbiEntity : IEntity<int>
    {
        /// <summary>
        /// The primary key in the database. For each entity use an additional string-based logical identifier with a MapIndex
        /// </summary>
        public int Id { get; set; }

        public DateTime? CreatedUtc { get; set; }

        public DateTime? ModifiedUtc { get; set; }
    }
}
