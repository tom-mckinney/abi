using Abi.Data.Abstractions;
using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Data
{
    public interface IExperimentRepository<TKey> : IRepository<Experiment<TKey>, TKey>
        where TKey : class
    {
    }
}
