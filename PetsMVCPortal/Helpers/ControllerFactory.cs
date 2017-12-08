﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace AGL.ALGPets.Portals.PetsMVCPortal.Helpers
{
    public interface IWebApiHttpClient
    {
        HttpClient Client { get; set; }
    }

    public class WebApiHttpClient : IWebApiHttpClient
    {
        public HttpClient Client { get; set; }

        public WebApiHttpClient()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["WebApiHttpClientURI"]);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public WebApiHttpClient(string uri)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(uri);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Used for mocking the HttpMessageHandler in unit testing.
        /// </summary>
        /// <param name="fooHandler"></param>
        public WebApiHttpClient(FooHandler fooHandler)
        {
            Client = new HttpClient(fooHandler);
        }
    }

    /// <summary>
    /// This is used for mocking the HttpMessageHandler, 
    /// since the HttpClient has no interface and cannot be directly mocked.
    /// </summary>
    public class FooHandler : HttpMessageHandler
    {
        private HttpResponseMessage _hm;

        public FooHandler(HttpResponseMessage hm)
        {
            _hm = hm;
        }

        /// <summary>
        /// Mock the response message
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = _hm;
            return response;
        }
    }
}