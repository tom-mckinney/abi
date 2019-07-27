using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.OrchardCore
{
    public interface IContentBalancer
    {
        int GetRandomIndex<T>(ICollection<T> collection);
    }

    public class ContentBalancer : IContentBalancer
    {
        private readonly Random _random;

        public ContentBalancer()
        {
            _random = new Random();
        }

        public virtual int GetRandomIndex<T>(ICollection<T> collection)
        {
            return _random.Next(0, collection.Count);
        }
    }
}
