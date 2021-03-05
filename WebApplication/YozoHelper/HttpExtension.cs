using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication
{
    public static class HttpExtension
    {
        /// <summary>
        /// 发起GET同步请求
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string HttpGet(this HttpClient client, string url, string contentType = "application/json",
                                     Dictionary<string, string> headers = null)
        {
            if (contentType != null)
                client.DefaultRequestHeaders.Add("ContentType", contentType);
            if (headers != null)
            {
                foreach (var header in headers)
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            HttpResponseMessage response = client.GetAsync(url).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// 发起GET异步请求
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(this HttpClient client, string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            if (contentType != null)
                client.DefaultRequestHeaders.Add("ContentType", contentType);
            if (headers != null)
            {
                foreach (var header in headers)
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 发起POST同步请求
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="timeOut"></param>
        /// <param name="headers">填充消息头</param>
        /// <returns></returns>
        public static string HttpPost(this HttpClient client, string url, string postData = null,
                                      string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
        {
            postData = postData ?? "";
            if (headers != null)
            {
                foreach (var header in headers)
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
            {
                if (contentType != null)
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                HttpResponseMessage response = client.PostAsync(url, httpContent).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        /// <summary>
        /// 发起POST异步请求
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>
        /// <returns></returns>
        public static async Task<string> HttpPostAsync(this HttpClient client, string url, string postData = null, string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
        {
            postData = postData ?? "";
            client.Timeout = new TimeSpan(0, 0, timeOut);
            if (headers != null)
            {
                foreach (var header in headers)
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
            {
                if (contentType != null)
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                HttpResponseMessage response = await client.PostAsync(url, httpContent);
                return await response.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// 发起POST同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>
        /// <returns></returns>
        public static T HttpPost<T>(this HttpClient client, string url, string postData = null, string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
        {
            return client.HttpPost(url, postData, contentType, timeOut, headers).ToEntity<T>();
        }

        /// <summary>
        /// 发起POST异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>
        /// <returns></returns>
        public static async Task<T> HttpPostAsync<T>(this HttpClient client, string url, string postData = null, string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
        {
            var res = await client.HttpPostAsync(url, postData, contentType, timeOut, headers);
            return res.ToEntity<T>();
        }

        /// <summary>
        /// 发起GET同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static T HttpGet<T>(this HttpClient client, string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            return client.HttpGet(url, contentType, headers).ToEntity<T>();
        }

        /// <summary>
        /// 发起GET异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static async Task<T> HttpGetAsync<T>(this HttpClient client, string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            var res = await client.HttpGetAsync(url, contentType, headers);
            return res.ToEntity<T>();
        }
    }
}