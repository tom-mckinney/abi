using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.Data
{
    public interface IExperimentRepository<TModel> : IRepository<TModel> where TModel : class
    {
    }
}
