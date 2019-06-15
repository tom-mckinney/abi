﻿using Abi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abi.Data
{
    public interface IVariantRepository : IRepository<Variant, int>
    {
        Task<Variant> GetByPublicIdAsync(string variantId);
        Task<Variant> CreateAsync(string contentItemId);
        Task UpdateAsync(Variant variant);
    }
}