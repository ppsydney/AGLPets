using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AGL.ALGPets.DataTransferObjects;

namespace AGL.ALGPets.Portals.PetsMVCPortal.Models
{
    public class OwnerModel : OwnerDTO
    {
        public OwnerModel() : base() { }

        public OwnerModel(string Name, string Gender, int Age, IEnumerable<PetDTO> Pets) 
            : base(Name, Gender, Age, Pets) { }
    }
}