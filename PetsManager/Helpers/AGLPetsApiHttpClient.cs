using AGL.ALGPets.DataTransferObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace AGL.ALGPets.Managers.PetsManager.Helpers
{
    public interface IAGLPetsApiHttpClient
    {
        HttpClient Client { get; set; }

        Task<List<OwnerDTO>> GetAllAGLPets();
    }

    public class AGLPetsApiHttpClient : IAGLPetsApiHttpClient
    {
        public HttpClient Client { get; set; }

        public AGLPetsApiHttpClient()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(WebConfigurationManager.AppSettings["AGLPetsApiHttpClientURI"]);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public AGLPetsApiHttpClient(string uri)
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(uri);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Accesses the AGLPets webservice to get a list of OwnerDTOs
        /// Note: This could be stored in a Session variable to save network traffic, but the specs are not clear
        /// </summary>
        /// <param name=""></param>
        /// <returns>List<OwnerDTO></returns>
        public async Task<List<OwnerDTO>> GetAllAGLPets()
        {
            // TODO: Consider caching the response to save network traffic; clarify with BAs about the FS
            HttpResponseMessage response = await Client.GetAsync(Client.BaseAddress);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var ownersList = JsonConvert.DeserializeObject<List<OwnerDTO>>(responseBody);

            return ownersList;
        }

        /// <summary>
        /// Used for mocking the HttpMessageHandler in unit testing.
        /// </summary>
        /// <param name="fooHandler"></param>
        public AGLPetsApiHttpClient(FooAGLPetsApiHttpMessageHandler fooHandler)
        {
            Client = new HttpClient(fooHandler);
        }
    }

    /// <summary>
    /// This is used for mocking the HttpMessageHandler, 
    /// since the HttpClient has no interface and cannot be directly mocked.
    /// </summary>
    public class FooAGLPetsApiHttpMessageHandler : HttpMessageHandler
    {
        private HttpResponseMessage _hm;

        public FooAGLPetsApiHttpMessageHandler(HttpResponseMessage hm)
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