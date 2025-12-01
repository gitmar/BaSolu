using System.Text;

using Newtonsoft.Json;

namespace GxStk.Helpers
{
    public static class JwtHelper
    {
        public static IDictionary<string, object> DecodePayload(string jwt)
        {
            var parts = jwt.Split('.');
            if (parts.Length != 3)
                throw new ArgumentException("Invalid JWT format");

            var payload = parts[1];
            var json = Encoding.UTF8.GetString(PadBase64(payload));
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json)!;
        }

        private static byte[] PadBase64(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64.Replace('-', '+').Replace('_', '/'));
        }
    }
}
