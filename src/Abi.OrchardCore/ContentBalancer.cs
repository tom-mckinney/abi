using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore
{
    public class ContentBalancer
    {
        private readonly Random _random;

        public ContentBalancer()
        {
            _random = new Random();
        }

        public int GetRandomIndex<T>(ICollection<T> collection)
        {
            return _random.Next(0, collection.Count);
        }
    }
}
