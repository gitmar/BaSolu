using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GxShared.Others;

namespace GxShared.Helpers
{
    public static class ApiResponseHelper
    {
        /// <summary>
        /// Ensures the ApiResponse has a non-null Payload and that Payload.Succeeded == true.
        /// Returns true if valid, false otherwise.
        /// </summary>
        public static bool EnsureSucceeded<T>(ApiResponse<T> response, out T payload)
            where T : class
        {
            payload = null;

            if (response == null)
                return false;

            payload = response.Payload;

            // If the payload has a Succeeded property, check it
            var succeededProp = typeof(T).GetProperty("Succeeded");
            if (succeededProp != null)
            {
                var succeededValue = succeededProp.GetValue(payload) as bool?;
                if (succeededValue != true)
                    return false;
            }

            return payload != null;
        }
    }
}
