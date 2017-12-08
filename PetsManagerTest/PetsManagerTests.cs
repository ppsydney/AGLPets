using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AGL.ALGPets.DataTransferObjects;
using AGL.ALGPets.Managers.PetsManager.Controllers;
using AGL.ALGPets.Managers.PetsManager.Helpers;
using AGL.AGLPets.Utilities;

namespace AGL.AGLPets.UnitTests.PetsManagerTest
{
    [TestClass]
    public class PetsManagerTests
    {
        #region Properties
        private TestContext testContextInstance;

        private readonly string _AGLPetsURI = "http://agl-developer-test.azurewebsites.net/people.json";
        private string _ownersJSON;
        private List<OwnerDTO> _owners;
        private List<OwnersPetsFlattenedDTO> _ownersPetFlattened;
        private List<OwnersPetsFlattenedDTO> _ownersPetFlattenedAndFiltered;
        #endregion 

        #region Initialisers
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            #region Data
            // OwnerDTOs
            _owners = new List<OwnerDTO>()
            {
                new OwnerDTO("Alpha", "Male", 1, new List<PetDTO>() {
                    new PetDTO("b", AGLPetsEnums.PetTypeEnum.Cat.ToString()),
                    new PetDTO("a", AGLPetsEnums.PetTypeEnum.Dog.ToString()) }),
                new OwnerDTO("Beta", "Female", 2, new List<PetDTO>() {
                    new PetDTO("c", AGLPetsEnums.PetTypeEnum.Elephant.ToString()),
                    new PetDTO("d", AGLPetsEnums.PetTypeEnum.Cat.ToString()) }),
            };

            // JSON representation of _owners
            _ownersJSON = new JavaScriptSerializer().Serialize(_owners);

            // Flattened _owners
            _ownersPetFlattened = new List<OwnersPetsFlattenedDTO>()
            {
                new OwnersPetsFlattenedDTO("Male", new List<string>()
                {
                    "b","a"
                }),
                new OwnersPetsFlattenedDTO("Female", new List<string>()
                {
                    "c","d"
                })
            };

            // Flattened and filtered _owners by pet Type = 'Cat',
            _ownersPetFlattenedAndFiltered = new List<OwnersPetsFlattenedDTO>()
            {
                new OwnersPetsFlattenedDTO("Male", new List<string>()
                {
                    "b"
                }),
                new OwnersPetsFlattenedDTO("Female", new List<string>()
                {
                    "d"
                })
            };
            #endregion
        }
        #endregion

        #region Tests
        /// <summary>
        /// This test asserts that the AGL web service is up and running
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetAGLPets_Should_Return_NotNull()
        {
            // Arrange
            IAGLPetsApiHttpClient iPm = new AGLPetsApiHttpClient(_AGLPetsURI);
            PetsManagerController pm = new PetsManagerController(iPm);

            // Act
            List<OwnerDTO> result = await pm.GetAllAGLPets();

            // Assert
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// This test asserts that the JSON is correctly transformed into a list of OwnerDTO
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetAGLPets_Should_Return_ListOwnerDTO()
        {
            // Arrange
            // Mock the HttpClient
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = new StringContent(_ownersJSON);
            FooHandler handler = new FooHandler(httpResponseMessage);
            IAGLPetsApiHttpClient iPm = new AGLPetsApiHttpClient(handler);
            iPm.Client.BaseAddress = new Uri(_AGLPetsURI);
            PetsManagerController pm = new PetsManagerController(iPm);

            List<OwnerDTO> expected = _owners;

            // Act
            List<OwnerDTO> actual = await pm.GetAllAGLPets();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new OwnerDTOComparer());
        }

        /// <summary>
        /// This test asserts that the webAPI gets a list of OwnersDTOs from AGLPets webservice, 
        /// filters the results by pet type (Cat) and flattens the result properly
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetAGLPets_Should_Return_ListOwnersPetFlattenedAndFilteredDTO()
        {
            // Arrange
            // Mock the HttpClient
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = new StringContent(_ownersJSON);
            FooHandler handler = new FooHandler(httpResponseMessage);
            IAGLPetsApiHttpClient iPm = new AGLPetsApiHttpClient(handler);
            iPm.Client.BaseAddress = new Uri(_AGLPetsURI);
            PetsManagerController pm = new PetsManagerController(iPm);

            List<OwnersPetsFlattenedDTO> expected = _ownersPetFlattenedAndFiltered;

            AGLPetsEnums.PetTypeEnum petType = AGLPetsEnums.PetTypeEnum.Cat;

            // Act
            List<OwnersPetsFlattenedDTO> actual = await pm.GetAGLPetsFlatten(petType.ToString());

            // Assert
            CollectionAssert.AreEqual(expected, actual, new OwnersPetsFlattenedDTOComparer());
        }

        /// <summary>
        /// This test asserts that the Filter of list of OwnersDTO contains only pet type of CATs
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Helper_Should_Return_FilteredOwnerDTOByPetType()
        {
            // Arrange
            string expected = AGLPetsEnums.PetTypeEnum.Cat.ToString();

            // Act
            List<OwnerDTO> actual = DTOHelpers.FilterResults(_owners, expected);

            // Assert
            // get distinct pet types from the response for easier asserts
            List<string> actualFiltered = actual
                .Where(o => o.Pets != null)
                .SelectMany(p => p.Pets)
                .Select(pt => pt.Type)
                .Distinct()
                .ToList();

            Assert.IsTrue(actualFiltered.Count() == 1);     // Only one pet type
            Assert.IsTrue(actualFiltered[0] == expected);   // Of the expected type
        }

        /// <summary>
        /// This test asserts that the list of OwnersDTO is properly flattened
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Helper_Should_Return_FlattenedOwnerDTOs()
        {
            // Arrange
            List<OwnersPetsFlattenedDTO> expected = _ownersPetFlattened;

            // Act
            List<OwnersPetsFlattenedDTO> actual = DTOHelpers.FlattenResult(_owners);

            // Assert
            CollectionAssert.AreEqual(expected, actual, new OwnersPetsFlattenedDTOComparer());
        }
        #endregion
    }
}
