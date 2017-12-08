using System.Web.Mvc;
using System.Web.Routing;

using AGL.ALGPets.Portals.PetsMVCPortal.Helpers;
using PetsMVCPortal.App_Start;
using AutoMapper;
using AGL.ALGPets.DataTransferObjects;
using AGL.ALGPets.Portals.PetsMVCPortal.Models;

namespace PetsMVCPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Map to OwnerModel
            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<OwnersPetsFlattenedDTO, OwnersPetsViewModel>();
            });
        }
    }
}
