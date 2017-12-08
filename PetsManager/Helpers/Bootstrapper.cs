using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Unity.WebApi;

namespace AGL.ALGPets.Managers.PetsManager.Helpers
{
    public class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IHttpClientHandler, WebApiHttpClient>();
            MvcUnityContainer.Container = container;
            return container;
        }
    }
}