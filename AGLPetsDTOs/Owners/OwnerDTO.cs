using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections;

namespace AGL.ALGPets.DataTransferObjects
{
    [DataContract]
    public class OwnerDTO
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public List<PetDTO> Pets { get; set; }

        public OwnerDTO() { }

        public OwnerDTO(string Name, string Gender, int Age, IEnumerable<PetDTO> Pets)
        {
            this.Name = Name;
            this.Gender = Gender;
            this.Age = Age;
            this.Pets = new List<PetDTO>(Pets);
        }
    }

    /// <summary>
    /// This is needed for unit testing, to compare two lists properly. 
    /// It should be refactored using Reflection to be resilient to property changes in the OwnerDTO class.
    /// </summary>
    public class OwnerDTOComparer : IComparer<OwnerDTO>, IComparer
    {
        // TODO: Refactor to use reflection

        public int Compare(OwnerDTO x, OwnerDTO y)
        {
            int compareResult =
                x.Gender.CompareTo(y.Gender) +
                x.Name.CompareTo(y.Name) +
                x.Age.CompareTo(y.Age)
                ;

            if(x.Pets?.Count != y.Pets?.Count)
            {
                return -1;
            }

            for (int i= 0; i < x.Pets?.Count(); i++)
            {
                compareResult += x.Pets[i].Name.CompareTo(y.Pets[i].Name);
                compareResult += x.Pets[i].Type.CompareTo(y.Pets[i].Type);
            }

            return compareResult;
        }

        public int Compare(object x, object y)
        {
            OwnerDTO castX = x as OwnerDTO;
            OwnerDTO castY = y as OwnerDTO;

            if (castX == null || castY == null)
            {
                return 0;
            }

            return Compare(castX, castY);
        }
    }
}
