using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

using AGL.ALGPets.DataTransferObjects;
using AGL.ALGPets.Managers.PetsManager.Helpers;
using Newtonsoft.Json;

namespace AGL.ALGPets.Managers.PetsManager.Controllers
{
    public class PetsManagerController : ApiController, IPetsManagerInterface
    {
        #region Properties
        private static HttpClient _client;
        #endregion

        #region ctor
        /// <summary>
        /// Dependency Injected ctor
        /// </summary>
        /// <param name="client"></param>
        public PetsManagerController(IAGLPetsApiHttpClient client)
        {
            _client = client.Client;
        }
        #endregion

        #region GETs
        /// <summary>
        /// Gets a list of flattened OwnersPetsFlattenedDTOs from a list of OwnerDTOs by pet type
        /// </summary>
        /// <param name="petType"></param>
        /// <returns>IHttpActionResult</returns>
        // GET: api/PetsManager/{petType}
        [HttpGet]
        [Route("api/PetsManager/{petType}")]
        public async Task<IHttpActionResult> GetOwnersPetsFlattenedDTOsByPetType(string petType)
        {
            List<OwnersPetsFlattenedDTO> ownersFlatten = await GetAGLPetsFlatten(petType);
            return Ok<List<OwnersPetsFlattenedDTO>>(ownersFlatten);
        }

        #endregion

        #region Accessors
        /// <summary>
        /// Gets a list of OwnersDTOs from AGLPets webservice, 
        /// filters the results by pet type and flattens the result
        /// </summary>
        /// <param name="petType"></param>
        /// <returns>List<OwnersPetsFlattenedDTO></returns>
        public async Task<List<OwnersPetsFlattenedDTO>> GetAGLPetsFlatten(string petType)
        {
            var ownersList = await GetAllAGLPets();

            ownersList = DTOHelpers.FilterResults(ownersList, petType);
            var ownersFlattenList = DTOHelpers.FlattenResult(ownersList);

            return ownersFlattenList;
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
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var ownersList = JsonConvert.DeserializeObject<List<OwnerDTO>>(responseBody);

            return ownersList;
        }
        #endregion
    }
}
