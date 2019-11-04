using System.Net.Http;
using System.Threading.Tasks;

namespace LeagueManager.Infrastructure.HttpHelpers
{
    public interface IHttpRequestFactory
    {
        Task<HttpResponseMessage> Delete(string requestUri);
        Task<HttpResponseMessage> Get(string requestUri);
        Task<HttpResponseMessage> Patch(string requestUri, object value);
        Task<HttpResponseMessage> Post(string requestUri, object value);
        Task<HttpResponseMessage> Put(string requestUri, object value);
    }
}