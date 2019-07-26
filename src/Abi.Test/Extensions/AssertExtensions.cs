using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace Xunit
{
    public partial class CustomAssert
    {
        /// <summary>
        /// Asserts that all properties of <paramref name="actual"/> have been mapped from <paramref name="expected"/>.
        /// </summary>
        /// <typeparam name="TOrigin">origin object type</typeparam>
        /// <typeparam name="TMap">mapped object type</typeparam>
        /// <param name="expected">origin object</param>
        /// <param name="actual">mapped object</param>
        /// <returns></returns>
        public static bool AllPropertiesMapped<TOrigin, TMap>(TOrigin expected, TMap actual)
        {
            return AllPropertiesMapped(expected, actual, new List<string>(), new Dictionary<string, string>());
        }

        /// <summary>
        /// Asserts that all properties of <paramref name="actual"/> have been mapped from <paramref name="expected"/>.
        /// All <paramref name="excludedProperties"/> will be excluded.
        /// </summary>
        /// <typeparam name="TOrigin">origin object type</typeparam>
        /// <typeparam name="TMap">mapped object type</typeparam>
        /// <param name="expected">origin object</param>
        /// <param name="actual">mapped object</param>
        /// <param name="excludedProperties">excluded properties</param>
        /// <returns></returns>
        public static bool AllPropertiesMapped<TOrigin, TMap>(TOrigin expected, TMap actual, IEnumerable<string> excludedProperties)
        {
            return AllPropertiesMapped(expected, actual, excludedProperties, new Dictionary<string, string>());

        }

        /// <summary>
        /// Asserts that all properties of <paramref name="actual"/> have been mapped from <paramref name="expected"/>.
        /// All <paramref name="excludedProperties"/> will be excluded and any properties with a matching key in <paramref name="translatedProperties"/> will be translated to respective property in class <typeparamref name="TOrigin"/>
        /// </summary>
        /// <typeparam name="TOrigin">origin object type</typeparam>
        /// <typeparam name="TMap">mapped object type</typeparam>
        /// <param name="expected">origin object</param>
        /// <param name="actual">mapped object</param>
        /// <param name="excludedProperties">excluded properties</param>
        /// <param name="translatedProperties">translated properties</param>
        /// <returns>True or throws Xunit exception</returns>
        public static bool AllPropertiesMapped<TOrigin, TMap>(TOrigin expected, TMap actual, IEnumerable<string> excludedProperties, IDictionary<string, string> translatedProperties)
        {
            if (excludedProperties == null)
                throw new ArgumentException("Argument cannot be null", nameof(excludedProperties));
            if (translatedProperties == null)
                throw new ArgumentException("Argument cannot be null", nameof(translatedProperties));

            PropertyInfo[] originProperties = typeof(TOrigin).GetProperties();
            PropertyInfo[] mappedProperties = typeof(TMap).GetProperties();

            foreach (var prop in mappedProperties)
            {
                if (excludedProperties.Contains(prop.Name))
                    continue;

                if (!translatedProperties.TryGetValue(prop.Name, out string propertyName))
                {
                    propertyName = prop.Name;
                }

                try
                {
                    var originProperty = Assert.Single(originProperties, p => p.Name == propertyName);
                    Assert.Equal(originProperty.GetValue(expected), prop.GetValue(actual));
                }
                catch (Exception e)
                {
                    throw new PropertyMapException($"Could not find matching property or value for {propertyName}.", typeof(TOrigin), typeof(TMap), e);
                }
            }

            return true;
        }
    }
}
