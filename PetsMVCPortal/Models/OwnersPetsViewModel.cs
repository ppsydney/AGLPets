using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AGL.ALGPets.DataTransferObjects;

namespace AGL.ALGPets.Portals.PetsMVCPortal.Models
{
    public class OwnersPetsViewModel : OwnersPetsFlattenedDTO
    {
        public OwnersPetsViewModel() : base() { }

        public OwnersPetsViewModel(string Gender, IEnumerable<string> Pets)
            : base(Gender, Pets) { }
    }
}