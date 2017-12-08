using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections;

namespace AGL.ALGPets.DataTransferObjects
{
    [DataContract]
    public class PetDTO
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Type { get; set; }

        public PetDTO() { }

        public PetDTO(string Name, string Type)
        {
            this.Name = Name;
            this.Type = Type;
        }
    }

    /// <summary>
    /// This is needed for unit testing, to compare two lists properly. 
    /// It should be refactored using Reflection to be resilient to property changes in the PetDTO class.
    /// </summary>
    public class PetDTOComparer : IComparer<PetDTO>, IComparer
    {
        // TODO: Refactor to use reflection

        public int Compare(PetDTO x, PetDTO y)
        {
            return (
                x.Name.CompareTo(y.Name) +
                x.Type.CompareTo(y.Type)
                );
        }

        public int Compare(object x, object y)
        {
            PetDTO castX = x as PetDTO;
            PetDTO castY = y as PetDTO;

            if (castX == null || castY == null)
            {
                return 0;
            }

            return Compare(castX, castY);
        }
    }
}
