using Newtonsoft.Json;
using System.Net.Http;

namespace LeagueManager.Infrastructure.HttpHelpers
{
    public static class HttpResponseExtensions
    {
        public static T ContentAsType<T>(this HttpResponseMessage response)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            return string.IsNullOrEmpty(data) ?
                default :
                JsonConvert.DeserializeObject<T>(data);
        }

        public static T ContentAsType<T>(this HttpResponseMessage response, JsonSerializerSettings settings)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            return string.IsNullOrEmpty(data) ?
                default :
                JsonConvert.DeserializeObject<T>(data, settings);
        }

        public static string ContentAsJson(this HttpResponseMessage response)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.SerializeObject(data);
        }

        public static string ContentAsString(this HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}