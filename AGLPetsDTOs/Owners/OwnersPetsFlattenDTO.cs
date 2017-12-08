using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AGL.ALGPets.DataTransferObjects
{
    [DataContract]
    public class OwnersPetsFlattenedDTO
    {
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public List<string> Pets { get; set; }

        public OwnersPetsFlattenedDTO() { }

        public OwnersPetsFlattenedDTO(string Gender, IEnumerable<string> Pets)
        {
            this.Gender = Gender;
            this.Pets = new List<string>(Pets);
        }
    }

    /// <summary>
    /// This is needed for unit testing, to compare two lists properly. 
    /// It should be refactored using Reflection to be resilient to property changes in the OwnersPetsFlattenedDTO class.
    /// </summary>
    public class OwnersPetsFlattenedDTOComparer : IComparer<OwnersPetsFlattenedDTO>, IComparer
    {
        // TODO: Refactor to use reflection

        public int Compare(OwnersPetsFlattenedDTO x, OwnersPetsFlattenedDTO y)
        {
            int compareResult = x.Gender.CompareTo(y.Gender);

            if (x.Pets?.Count != y.Pets?.Count)
            {
                return -1;
            }

            for (int i = 0; i < x.Pets?.Count; i++)
            {
                compareResult += x.Pets[i].CompareTo(y.Pets[i]);
            }

            return compareResult;
        }

        public int Compare(object x, object y)
        {
            OwnersPetsFlattenedDTO castX = x as OwnersPetsFlattenedDTO;
            OwnersPetsFlattenedDTO castY = y as OwnersPetsFlattenedDTO;

            if (castX == null || castY == null)
            {
                return 0;
            }

            return Compare(castX, castY);
        }
    }
}
