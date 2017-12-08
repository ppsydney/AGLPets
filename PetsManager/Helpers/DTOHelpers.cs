using System.Collections.Generic;
using System.Linq;

using AGL.ALGPets.DataTransferObjects;

namespace AGL.ALGPets.Managers.PetsManager.Helpers
{
    public static class DTOHelpers
    {
        #region Helpers

        /// <summary>
        /// Filter list of OwnerDTOs by pet type
        /// </summary>
        /// <param name="owners"></param>
        /// <param name="petType"></param>
        /// <returns></returns>
        public static List<OwnerDTO> FilterResults(List<OwnerDTO> owners, string petType)
        {
            owners?
                .ForEach(o =>
                    o.Pets?.RemoveAll(pt => pt.Type.ToUpper() != petType.ToUpper())
                );

            return owners;
        }

        /// <summary>
        /// Transforms list of OwnerDTOs in a flattened list of OwnerPetsFlattenedDTOs, grouped by owner's gender.
        /// </summary>
        /// <param name="owners"></param>
        /// <returns></returns>
        public static List<OwnersPetsFlattenedDTO> FlattenResult(List<OwnerDTO> owners)
        {
            List<OwnersPetsFlattenedDTO> result = owners?
                    .Where(p => p.Pets != null)
                    .GroupBy(g => g.Gender,
                        (key, group) => new OwnersPetsFlattenedDTO()
                        {
                            Gender = key,
                            Pets = group
                                    .SelectMany(pt => pt.Pets)
                                    .Select(q => q.Name)
                                    .ToList()
                        })
                    .ToList();

            return result;
        }
        #endregion
    }
}