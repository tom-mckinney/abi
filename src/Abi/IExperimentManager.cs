using System;
using System.Collections.Generic;
using System.Text;

namespace Abi
{
    public partial interface IExperimentManager<TModel> where TModel : class
    {
        int GetVariantIndex(TModel content);
    }
}
