using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AGL.ALGPets.Portals.PetsMVCPortal.Helpers
{
    public static class UserInterfaceHelpers
    {
        public static List<SelectListItem> GetListOfPetTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var enumList = EnumHelper.GetSelectList(typeof(AGL.AGLPets.Utilities.AGLPetsEnums.PetTypeEnum));
            items.AddRange(enumList);

            return items;
        }
    }
}