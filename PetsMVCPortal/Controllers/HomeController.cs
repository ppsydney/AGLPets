using System.Collections.Generic;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;

using AGL.ALGPets.Portals.PetsMVCPortal.Models;
using AGL.ALGPets.Portals.PetsMVCPortal.Helpers;
using AGL.ALGPets.DataTransferObjects;
using AGL.AGLPets.Utilities;

namespace AGL.ALGPets.Portals.PetsMVCPortal.Controllers
{
    public class HomeController : Controller
    {
        private static HttpClient _client;

        public HomeController(IWebApiHttpClient client)
        {
            _client = client.Client;
        }

        public ActionResult Index()
        {
            InitialiseUserInterface();
            return View();
        }

        public async Task<ActionResult> GetPetList(AGLPetsEnums.PetTypeEnum PetTypeList)
        {
            InitialiseUserInterface();
            List<OwnersPetsViewModel> ownersPetsViewModel = await GetList(PetTypeList);
            
            return View("Index", ownersPetsViewModel);
        }

        private void InitialiseUserInterface()
        {
            ViewBag.PetTypeList = UserInterfaceHelpers.GetListOfPetTypes();
        }

        public Task<List<OwnersPetsViewModel>> GetList(AGLPetsEnums.PetTypeEnum petType)
        {
            var ownersPetsFlattenDTOs = SortHelper.GetListSorted(GetAGLPetsByType(petType).Result);
            var ownersPetsViewModel = AutoMapper.Mapper.Map<List<OwnersPetsFlattenedDTO>, List<OwnersPetsViewModel>>(ownersPetsFlattenDTOs);

            return Task.FromResult(ownersPetsViewModel);
        }

        public async Task<List<OwnersPetsFlattenedDTO>> GetAGLPetsByType(AGLPetsEnums.PetTypeEnum petType)
        {
            var ownersPetsFlatten = new List<OwnersPetsFlattenedDTO>();
            string path = GenerateURI(petType);
            HttpResponseMessage response = _client.GetAsync(path).Result;
            if (response.IsSuccessStatusCode)
            {
                ownersPetsFlatten = await response.Content.ReadAsAsync<List<OwnersPetsFlattenedDTO>>();
            }
            return ownersPetsFlatten;
        }

        public string GenerateURI(AGLPetsEnums.PetTypeEnum petType)
        {
            return 
                _client.BaseAddress +
                WebConfigurationManager.AppSettings["WebApiHttpClientPetController"] +
                petType.ToString();
        }

    }
}