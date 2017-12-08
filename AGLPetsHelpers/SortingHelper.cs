using AGL.ALGPets.DataTransferObjects;
using System.Collections.Generic;
using System.Linq;

namespace AGL.AGLPets.Utilities
{
    public static class SortHelper
    {
        /// <summary>
        /// Sorts the list alphabetically
        /// </summary>
        /// <param name="ownersPetsFlattenDTOs"></param>
        /// <returns></returns>
        public static List<OwnersPetsFlattenedDTO> GetListSorted(List<OwnersPetsFlattenedDTO> ownersPetsFlattenDTOs)
        {
            List<OwnersPetsFlattenedDTO> ownersPetsViewModel = new List<OwnersPetsFlattenedDTO>();

            ownersPetsViewModel = ownersPetsFlattenDTOs?
                .Where(p => p.Pets != null)
                .GroupBy(g => g.Gender,
                    (key, group) => new OwnersPetsFlattenedDTO()
                    {
                        Gender = key,
                        Pets = group
                                .SelectMany(pt => pt.Pets)
                                .OrderBy(ord => ord)
                                .ToList()
                    })
                .ToList();

            return ownersPetsViewModel;
        }
    }
}
