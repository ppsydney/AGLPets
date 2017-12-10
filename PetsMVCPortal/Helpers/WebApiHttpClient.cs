using AGL.AGLPets.Utilities;
using AGL.ALGPets.DataTransferObjects;
using System;
using System.Collections.Generic;
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

        Task<List<OwnersPetsFlattenedDTO>> GetAGLOwnersPetsFlattenedByType(AGLPetsEnums.PetTypeEnum petType);
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

        public async Task<List<OwnersPetsFlattenedDTO>> GetAGLOwnersPetsFlattenedByType(AGLPetsEnums.PetTypeEnum petType)
        {
            var ownersPetsFlatten = new List<OwnersPetsFlattenedDTO>();
            string path = GenerateGetOwnersPetsFlattenedDTOsByPetTypeURI(petType);
            HttpResponseMessage response = Client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                ownersPetsFlatten = await response.Content.ReadAsAsync<List<OwnersPetsFlattenedDTO>>();
            }
            return ownersPetsFlatten;
        }

        private string GenerateGetOwnersPetsFlattenedDTOsByPetTypeURI(AGLPetsEnums.PetTypeEnum petType)
        {
            return
                Client.BaseAddress +
                WebConfigurationManager.AppSettings["WebApiHttpClientPetController"] +
                petType.ToString();
        }

        /// <summary>
        /// Used for mocking the HttpMessageHandler in unit testing.
        /// </summary>
        /// <param name="fooHandler"></param>
        public WebApiHttpClient(FooWebApiHttpMessageHandler fooHandler)
        {
            Client = new HttpClient(fooHandler);
        }
    }

    /// <summary>
    /// This is used for mocking the HttpMessageHandler, 
    /// since the HttpClient has no interface and cannot be directly mocked.
    /// </summary>
    public class FooWebApiHttpMessageHandler : HttpMessageHandler
    {
        private HttpResponseMessage _hm;

        public FooWebApiHttpMessageHandler(HttpResponseMessage hm)
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