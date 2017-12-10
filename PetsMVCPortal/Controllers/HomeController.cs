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
        #region Properties
        dynamic _webAPIClient;
        #endregion

        #region ctor
        /// <summary>
        /// Dependency Injection ctor
        /// </summary>
        /// <param name="client"></param>
        public HomeController(IWebApiHttpClient client)
        {
            _webAPIClient = client;
        }
        #endregion

        public ActionResult Index()
        {
            InitialiseUserInterface();
            return View();
        }

        /// <summary>
        /// Gets the owners/pets list and displays it
        /// </summary>
        /// <param name="PetTypeList"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetPetList(AGLPetsEnums.PetTypeEnum PetTypeList)
        {
            InitialiseUserInterface();
            List<OwnersPetsViewModel> ownersPetsViewModel = await GetList(PetTypeList);
            
            return View("Index", ownersPetsViewModel);
        }

        /// <summary>
        /// Initialises the dropdownlist with list of pet types
        /// </summary>
        private void InitialiseUserInterface()
        {
            ViewBag.PetTypeList = UserInterfaceHelpers.GetListOfPetTypes();
        }

        /// <summary>
        /// Gets the owners/pets list sorted and mapped to the view model
        /// </summary>
        /// <param name="petType"></param>
        /// <returns></returns>
        private Task<List<OwnersPetsViewModel>> GetList(AGLPetsEnums.PetTypeEnum petType)
        {
            var ownersPetsFlattenDTOs = SortingHelper.GetListSorted(_webAPIClient.GetAGLOwnersPetsFlattenedByType(petType).Result);
            var ownersPetsViewModel = AutoMapper.Mapper.Map<List<OwnersPetsFlattenedDTO>, List<OwnersPetsViewModel>>(ownersPetsFlattenDTOs);

            return Task.FromResult(ownersPetsViewModel);
        }

    }
}