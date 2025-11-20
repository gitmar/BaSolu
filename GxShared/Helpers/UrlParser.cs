using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.Helpers
{
    public static class UrlParser
    {
        public static string ReplaceWithSubdomain(string userInputUrl, string newSubdomain)
        {
            var uri = new Uri(userInputUrl);
            var hostParts = uri.Host.Split('.');

            // Handle www.a24soft.com or a24soft.com
            string rootDomain = hostParts.Length >= 2
                ? string.Join('.', hostParts[^2], hostParts[^1])
                : uri.Host;

            return $"{uri.Scheme}://{newSubdomain}.{rootDomain}";
        }
    }
}
