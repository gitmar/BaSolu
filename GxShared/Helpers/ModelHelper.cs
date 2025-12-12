using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.Helpers
{
    public static class ModelHelper
    {
        public static TDestination MapTo<TSource, TDestination>(
            TSource source,
            bool ignoreNulls = false,
            bool caseInsensitive = false)
            where TDestination : new()
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var comparison = caseInsensitive
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;
            var sourceProps = typeof(TSource)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead);
            var destProps = typeof(TDestination)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToList();
            var destination = new TDestination();
            foreach (var sProp in sourceProps)
            {
                var dProp = destProps.FirstOrDefault(
                    p => p.Name.Equals(sProp.Name, comparison) &&
                         p.PropertyType.IsAssignableFrom(sProp.PropertyType));

                if (dProp != null)
                {
                    var value = sProp.GetValue(source);
                    if (ignoreNulls && value == null)
                        continue;

                    dProp.SetValue(destination, value);
                }
            }
            return destination;
        }
        public static void CopyMatchingProperties<TSource, TDestination>(
                TSource source,
                TDestination destination,
                bool ignoreNulls = false,
                bool caseInsensitive = false)
        {
            if (source == null || destination == null)
                throw new ArgumentNullException("Source or destination cannot be null.");

            var comparison = caseInsensitive
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            var sourceProps = typeof(TSource)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead);

            var destProps = typeof(TDestination)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToList();

            foreach (var sProp in sourceProps)
            {
                var dProp = destProps.FirstOrDefault(
                    p => p.Name.Equals(sProp.Name, comparison) &&
                         p.PropertyType.IsAssignableFrom(sProp.PropertyType));

                if (dProp != null)
                {
                    var value = sProp.GetValue(source);
                    if (ignoreNulls && value == null)
                        continue;

                    dProp.SetValue(destination, value);
                }
            }
        }
    }
}
