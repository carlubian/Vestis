using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace Vestis.Core.Model
{
    /// <summary>
    /// Makes Rest requests, or, Restquests.
    /// </summary>
    internal static class Restquest
    {
        internal static dynamic Get(string uri, string query)
        {
            using var web = new HttpClient();
            var request = web.GetAsync($"http://{uri}{query}");
            request.Wait();
            var response = request.Result;
            var content = response.Content.ReadAsStringAsync();
            content.Wait();
            var type = JObject.Parse(content.Result);

            return type;
        }
    }
}
