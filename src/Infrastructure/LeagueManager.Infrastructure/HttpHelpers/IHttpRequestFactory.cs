﻿using System.Net.Http;
using System.Threading.Tasks;

namespace LeagueManager.Infrastructure.HttpHelpers
{
    public interface IHttpRequestFactory
    {
        Task<HttpResponseMessage> Delete(string requestUri, string accessToken = "");
        Task<HttpResponseMessage> Get(string requestUri, string accessToken = "");
        Task<HttpResponseMessage> Patch(string requestUri, object value, string accessToken = "");
        Task<HttpResponseMessage> Post(string requestUri, object value, string accessToken = "");
        Task<HttpResponseMessage> Put(string requestUri, object value, string accessToken = "");
    }
}