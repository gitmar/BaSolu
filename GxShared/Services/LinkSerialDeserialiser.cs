using System.Net;
using System.Reflection;
using System.Text;

using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;

namespace GxShared.Services
{
    public class LinkSerialiser
    {
#if NET9_0_OR_GREATER
    private readonly IJSRuntime _jsRun;
#endif

#if NET9_0_OR_GREATER
    public LinkSerialiser(IJSRuntime jsRun)
    {
        _jsRun = jsRun;
    }
#else
        public LinkSerialiser()
        {
            // No JSRuntime for net8.0 or backend
        }
#endif
        public string GenerateLink<T>(T dto, string baseUrl, string strctrl)
        {
            Console.WriteLine($"Link du modele {dto.ToString}");
            var query = BuildQuery(dto);
            var bytes = System.Text.Encoding.UTF8.GetBytes(query);
            var encoded = WebEncoders.Base64UrlEncode(bytes);
            string nwbaseUrl = baseUrl.TrimEnd('/');
            string generatedLink = $"{nwbaseUrl}/{strctrl}?cdlink={encoded}";
            //_ = _jsRun.InvokeVoidAsync("generateQrCode", generatedLink);
            Console.WriteLine($"Link = {generatedLink}");
            return generatedLink;
        }
        private string BuildQuery<T>(T dto)
        {
            var props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            var parts = props
                .Select(p => new {
                    Name = p.Name.ToLower(),
                    Value = p.GetValue(dto)?.ToString()
                })
                .Where(x => !string.IsNullOrEmpty(x.Value))  // filter out null or empty values
                .Select(x => $"{x.Name}={Uri.EscapeDataString(x.Value)}");

            return string.Join("&", parts);
        }
        //private string BuildQuery<T>(T dto)
        //{
        //    var props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        //    var parts = props.Select(p => $"{p.Name.ToLower()}={Uri.EscapeDataString(p.GetValue(dto)?.ToString() ?? string.Empty)}");
        //    return string.Join("&", parts);
        //}
        public T DecodeAndDeserialize<T>(string encoded)
    where T : new()
        {
            var bytes = WebEncoders.Base64UrlDecode(encoded);
            var query = System.Text.Encoding.UTF8.GetString(bytes);

            // Parse query into dictionary
            var parsed = QueryHelpers.ParseQuery(query);

            var obj = new T();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                var key = prop.Name.ToLower();

                if (parsed.TryGetValue(key, out var value) && value.Count > 0)
                {
                    var strValue = value[0];

                    if (string.IsNullOrWhiteSpace(strValue))
                        continue;

                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    object? converted =
                        targetType.IsEnum ? Enum.Parse(targetType, strValue, true)
                        : targetType == typeof(string) ? strValue
                        : Convert.ChangeType(strValue, targetType);

                    prop.SetValue(obj, converted);
                }
            }

            return obj;
        }
        public string EncodeAndSerialize<T>(T extraDto)
        {
            var extraQuery = BuildQuery(extraDto);
            var extraBytes = Encoding.UTF8.GetBytes(extraQuery);
            var extraEncoded = WebEncoders.Base64UrlEncode(extraBytes);
            return extraEncoded;
        }
        //public class LinkDeserializer()
        //{
        //    public static T? DeserializeQueryToModel<T>(string encodedQuery) where T : new()
        //    {
        //        // Decode base64 URL string to query string
        //        var bytes = WebEncoders.Base64UrlDecode(encodedQuery);
        //        var queryString = System.Text.Encoding.UTF8.GetString(bytes);

        //        // Parse query string into dictionary with case-insensitive keys
        //        var keyValues = queryString.Split('&', StringSplitOptions.RemoveEmptyEntries)
        //            .Select(part => part.Split('=', 2))
        //            .Where(parts => parts.Length == 2)
        //            .ToDictionary(parts => parts[0], parts => WebUtility.UrlDecode(parts[1]), StringComparer.OrdinalIgnoreCase);

        //        var obj = new T();
        //        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //        foreach (var prop in props)
        //        {
        //            if (keyValues.TryGetValue(prop.Name.ToLower(), out var strValue))
        //            {
        //                try
        //                {
        //                    var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
        //                    object? value = null;

        //                    if (string.IsNullOrEmpty(strValue))
        //                    {
        //                        // For nullable types, set to null; for non-nullable, leave default or skip setting
        //                        if (Nullable.GetUnderlyingType(prop.PropertyType) != null)
        //                        {
        //                            value = null;
        //                        }
        //                        else
        //                        {
        //                            // Skip or default value for non-nullable types, do not set property or set default
        //                            continue;
        //                        }
        //                    }
        //                    else if (targetType == typeof(string))
        //                    {
        //                        value = strValue;
        //                    }
        //                    else if (targetType.IsEnum)
        //                    {
        //                        value = Enum.Parse(targetType, strValue, ignoreCase: true);
        //                    }
        //                    else
        //                    {
        //                        // Convert.ChangeType on underlying type
        //                        value = Convert.ChangeType(strValue, targetType);
        //                    }

        //                    prop.SetValue(obj, value);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine($"Failed to convert '{strValue}' to {prop.PropertyType}: {ex.Message}");
        //                }
        //            }
        //        }

        //        return obj;
        //    }
        //    // Existing methods that do not depend on IJSRuntime stay unchanged
        //}
        //public static class ModelHelper
        //{
        //    public static void CopyMatchingProperties<TSource, TDestination>(
        //        TSource source,
        //        TDestination destination,
        //        bool ignoreNulls = false,
        //        bool caseInsensitive = false)
        //    {
        //        if (source == null || destination == null)
        //            throw new ArgumentNullException("Source or destination cannot be null.");

        //        var comparison = caseInsensitive
        //            ? StringComparison.OrdinalIgnoreCase
        //            : StringComparison.Ordinal;

        //        var sourceProps = typeof(TSource)
        //            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        //            .Where(p => p.CanRead);

        //        var destProps = typeof(TDestination)
        //            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        //            .Where(p => p.CanWrite)
        //            .ToList();

        //        foreach (var sProp in sourceProps)
        //        {
        //            var dProp = destProps.FirstOrDefault(
        //                p => p.Name.Equals(sProp.Name, comparison) &&
        //                     p.PropertyType.IsAssignableFrom(sProp.PropertyType));

        //            if (dProp != null)
        //            {
        //                var value = sProp.GetValue(source);
        //                if (ignoreNulls && value == null)
        //                    continue;

        //                dProp.SetValue(destination, value);
        //            }
        //        }
        //    }
        //}
    }
}

        
        
    //    public T DecodeAndDeserialize<T>(string encoded)
    //    {
    //        var bytes = WebEncoders.Base64UrlDecode(encoded);
    //        var query = System.Text.Encoding.UTF8.GetString(bytes);

    //        // Parse query string into a dictionary
    //        var dict = System.Web.HttpUtility.ParseQueryString(query);

    //        // Map dictionary to model
    //        var obj = Activator.CreateInstance<T>();
    //        var props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
    //        foreach (var prop in props)
    //        {
    //            var val = dict[prop.Name.ToLower()];
    //            if (!string.IsNullOrEmpty(val))
    //            {
    //                var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
    //                var convertedValue = Convert.ChangeType(val, targetType);
    //                prop.SetValue(obj, convertedValue);
    //            }

    //            //if (val != null)
    //            //{
    //            //    prop.SetValue(obj, Convert.ChangeType(val, prop.PropertyType));
    //            //}
    //        }
    //        return obj;
    //    }
    //}
    

            //foreach (var prop in props)
            //{
            //    if (keyValues.TryGetValue(prop.Name.ToLower(), out var strValue))
            //    {
            //        try
            //        {
            //            object? value = null;
            //            if (prop.PropertyType == typeof(string))
            //            {
            //                value = strValue;
            //            }
            //            else if (prop.PropertyType.IsEnum)
            //            {
            //                value = Enum.Parse(prop.PropertyType, strValue, ignoreCase: true);
            //            }
            //            else
            //            {
            //                // Convert.ChangeType handles int, bool, double, DateTime, etc.
            //                value = Convert.ChangeType(strValue, prop.PropertyType);
            //            }
            //            prop.SetValue(obj, value);
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine($"Failed to convert '{strValue}' to {prop.PropertyType}: {ex.Message}");
            //            // Handle conversion errors (optional: log or ignore)
            //        }
            //    }
            //}
     //       return obj;
     //   }
    //}
    //}
