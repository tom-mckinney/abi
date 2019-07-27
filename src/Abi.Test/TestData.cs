using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Test
{
    public class TestData
    {
        private static Random _random;
        private static Random Random
        {
            get
            {
                if (_random == null)
                    _random = new Random();

                return _random;
            }
        }

        public static TModel Create<TModel>() where TModel : class
        {
            TModel model = Activator.CreateInstance<TModel>();

            foreach (var property in typeof(TModel).GetProperties())
            {
                if (property.Name?.ToLowerInvariant() == "id")
                    continue;

                var propertyType = property.PropertyType;

                if (propertyType == typeof(string))
                {
                    property.SetValue(model, "Very Berry Wumbo");
                }
                else if (propertyType == typeof(int))
                {
                    property.SetValue(model, Random.Next(100000));
                }
                else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTimeOffset))
                {
                    int year = Random.Next(2010, 2023);
                    int month = Random.Next(1, 12);
                    int day = Random.Next(1, 28);

                    property.SetValue(model, new DateTime(year, month, day));
                }
            }

            return model;
        }

        public static TModel Create<TModel>(Action<TModel> callback) where TModel : class
        {
            TModel model = Create<TModel>();

            callback(model);

            return model;
        }
    }
}
