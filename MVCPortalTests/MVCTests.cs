using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AGL.ALGPets.Portals.PetsMVCPortal.Controllers;
using AGL.ALGPets.DataTransferObjects;
using AGL.AGLPets.Utilities;
using AGL.ALGPets.Portals.PetsMVCPortal.Helpers;
using System.Configuration;

namespace AGL.AGLPets.UnitTests.MVCPortalTests
{
    [TestClass]
    public class MVCTests
    {
        #region Properties
        private TestContext _testContextInstance;

        private readonly string _WebAPIURI = ConfigurationManager.AppSettings["WebApiHttpClientURI"] + ConfigurationManager.AppSettings["WebApiHttpClientPetController"];
        private List<OwnerDTO> _owners;
        private List<OwnersPetsFlattenedDTO> _ownersPetFlattenedFiltered;
        private List<OwnersPetsFlattenedDTO> _ownersPetFlattenedAndSorted;
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
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
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
                    new PetDTO("c", AGLPetsEnums.PetTypeEnum.Cat.ToString()),
                    new PetDTO("b", AGLPetsEnums.PetTypeEnum.Cat.ToString()),
                    new PetDTO("a", AGLPetsEnums.PetTypeEnum.Dog.ToString()) }),
                new OwnerDTO("Beta", "Female", 2, new List<PetDTO>() {
                    new PetDTO("d", AGLPetsEnums.PetTypeEnum.Elephant.ToString()),
                    new PetDTO("e", AGLPetsEnums.PetTypeEnum.Cat.ToString()) }),
            };


            // Flattened _owners
            _ownersPetFlattenedFiltered = new List<OwnersPetsFlattenedDTO>()
            {
                new OwnersPetsFlattenedDTO("Male", new List<string>()
                {
                    "c","b"
                }),
                new OwnersPetsFlattenedDTO("Female", new List<string>()
                {
                    "e"
                })
            };

            // Flattened and filtered _owners by pet Type = 'Cat',
            _ownersPetFlattenedAndSorted = new List<OwnersPetsFlattenedDTO>()
            {
                new OwnersPetsFlattenedDTO("Male", new List<string>()
                {
                    "b", "c"
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
        /// Asserts that the WebAPI is up and running and accessible from MVCPortal
        /// </summary>
        [TestMethod]
        public async Task MVCPortal_Should_Return_WebApiAccessible()
        {
            // Arrange
            WebApiHttpClient webAPI = new WebApiHttpClient(_WebAPIURI);
            AGLPetsEnums.PetTypeEnum petType = AGLPetsEnums.PetTypeEnum.Cat;

            // Act
            List<OwnersPetsFlattenedDTO> actual = await webAPI.GetAGLOwnersPetsFlattenedByType(petType);

            // Assert
            Assert.IsNotNull(actual);
        }
        #endregion
    }
}
