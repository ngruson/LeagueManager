using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Threading.Tasks;

namespace LeagueManager.Infrastructure.HttpHelpers
{
    public class HttpRequestFactory : IHttpRequestFactory
    {
        private readonly IHttpRequestBuilder requestBuilder;

        public HttpRequestFactory(IHttpRequestBuilder requestBuilder)
        {
            this.requestBuilder = requestBuilder;
        }

        public async Task<HttpResponseMessage> Get(string requestUri)
        {
            var builder = requestBuilder
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri);

            if (HttpContextHelper.HttpContext != null)
            {
                var accessToken = await AuthenticationHttpContextExtensions.GetTokenAsync(
                    HttpContextHelper.HttpContext, "access_token");
                if (!string.IsNullOrEmpty(accessToken))
                    builder.AddBearerToken(accessToken);
            }

            return await builder.SendAsync();
        }

        public async Task<HttpResponseMessage> Post(
           string requestUri, object value)
        {
            var builder = requestBuilder
                .AddMethod(HttpMethod.Post)
                .AddRequestUri(requestUri)
                .AddContent(new JsonContent(value));

            if (HttpContextHelper.HttpContext != null)
            {
                var accessToken = await AuthenticationHttpContextExtensions.GetTokenAsync(
                    HttpContextHelper.HttpContext, "access_token");
                if (!string.IsNullOrEmpty(accessToken))
                    builder.AddBearerToken(accessToken);
            }

            return await builder.SendAsync();
        }

        public async Task<HttpResponseMessage> Put(
           string requestUri, object value)
        {
            var builder = requestBuilder
                .AddMethod(HttpMethod.Put)
                .AddRequestUri(requestUri)
                .AddContent(new JsonContent(value));

            if (HttpContextHelper.HttpContext != null)
            {
                var accessToken = await AuthenticationHttpContextExtensions.GetTokenAsync(
                    HttpContextHelper.HttpContext, "access_token");
                if (!string.IsNullOrEmpty(accessToken))
                    builder.AddBearerToken(accessToken);
            }

            return await builder.SendAsync();
        }

        public async Task<HttpResponseMessage> Patch(
           string requestUri, object value)
        {
            var builder = requestBuilder
                .AddMethod(new HttpMethod("PATCH"))
                .AddRequestUri(requestUri)
                .AddContent(new PatchContent(value));

            if (HttpContextHelper.HttpContext != null)
            {
                var accessToken = await AuthenticationHttpContextExtensions.GetTokenAsync(
                    HttpContextHelper.HttpContext, "access_token");
                if (!string.IsNullOrEmpty(accessToken))
                    builder.AddBearerToken(accessToken);
            }

            return await builder.SendAsync();
        }

        public async Task<HttpResponseMessage> Delete(string requestUri)
        {
            var builder = requestBuilder
                .AddMethod(HttpMethod.Delete)
                .AddRequestUri(requestUri);

            if (HttpContextHelper.HttpContext != null)
            {
                var accessToken = await AuthenticationHttpContextExtensions.GetTokenAsync(
                    HttpContextHelper.HttpContext, "access_token");
                if (!string.IsNullOrEmpty(accessToken))
                    builder.AddBearerToken(accessToken);
            }

            return await builder.SendAsync();
        }
    }
}