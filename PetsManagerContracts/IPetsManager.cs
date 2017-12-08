using System.Web.Http;
using System.Threading.Tasks;

namespace AGL.ALGPets.Managers.PetsManager
{
    public interface IPetsManagerInterface
    {
        // GET: api/PetsManager/{petType}
        Task<IHttpActionResult> GetOwnersPetsFlattenedDTOsByPetType(string petType);
    }
}
