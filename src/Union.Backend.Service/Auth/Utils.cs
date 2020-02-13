using Jose;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Union.Backend.Service.Exceptions;

namespace Union.Backend.Service.Auth
{
    public static class Utils
    {
        private static HttpClient SetHeaders(this HttpClient httpClient, params (HttpRequestHeader, string)[] headers)
        {
            foreach (var t in headers)
                httpClient.DefaultRequestHeaders.Add(t.Item1.ToString(), t.Item2);
            return httpClient;
        }

        public static async Task<HttpResponseMessage> HttpGetAsync(string host, params (HttpRequestHeader, string)[] headers)
        {
            var httpClient = new HttpClient().SetHeaders(headers);
            return await httpClient.GetAsync(host);
        }
        public static async Task<T> HttpGetAsync<T>(string host, params (HttpRequestHeader, string)[] headers)
        {
            var result = await HttpGetAsync(host, headers);
            if (result.StatusCode.Equals(HttpStatusCode.OK))
            {
                return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
            }
            else
                throw new BadRequestApiException();
        }

        public static async Task<HttpResponseMessage> HttpPostAsync(
            string host,
            HttpContent content,
            params (HttpRequestHeader, string)[] headers)
        {
            var httpClient = new HttpClient().SetHeaders(headers);
            return await httpClient.PostAsync(host, content);
        }
        public static async Task<T> HttpPostAsync<T>(
            string host, 
            HttpContent content,
            params (HttpRequestHeader, string)[] headers)
        {
            var result = await HttpPostAsync(host, content, headers);
            if (result.StatusCode.Equals(HttpStatusCode.OK))
            {
                return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
            }
            else
                throw new BadRequestApiException();
        }

        public static T ValidateAndGetToken<T>(string token, string secret)
        {
            var accessToken = JWT.Decode<T>(token, Convert.FromBase64String(secret));
            return accessToken;
        }
    }
}
