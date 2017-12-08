using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AGL.AGLPets.Utilities;
using AGL.ALGPets.DataTransferObjects;

namespace AGL.AGLPets.UnitTests.MVCPortalTests
{
    /// <summary>
    /// Tests MVC helpers
    /// </summary>
    [TestClass]
    public class HelpersTests
    {
        #region Properties
        private TestContext testContextInstance;
        private List<OwnersPetsFlattenedDTO> _ownersPetFlattened;
        private List<OwnersPetsFlattenedDTO> _ownersPetFlattenedSorted;
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
            // Flattened DTO
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

            // Flattened DTO sorted
            _ownersPetFlattenedSorted = new List<OwnersPetsFlattenedDTO>()
            {
                new OwnersPetsFlattenedDTO("Male", new List<string>()
                {
                    "a","b"
                }),
                new OwnersPetsFlattenedDTO("Female", new List<string>()
                {
                    "c","d"
                })
            };
            #endregion
        }
        #endregion

        #region Tests
        /// <summary>
        /// Asserts that a non null collection is sorted into a non null collection with same number of nodes
        /// </summary>
        [TestMethod]
        public void Sort_Should_Return_NotNull()
        {
            // Arrange

            // Act
            List<OwnersPetsFlattenedDTO> actual = SortHelper.GetListSorted(_ownersPetFlattened);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == _ownersPetFlattened.Count);
        }

        /// <summary>
        /// Asserts that the list is sorted in an alphabetic order
        /// </summary>
        [TestMethod]
        public void Sort_Should_Return_AlphabeticSorted()
        {
            // Arrange

            // Act
            List<OwnersPetsFlattenedDTO> actual = SortHelper.GetListSorted(_ownersPetFlattened);

            // Assert
            CollectionAssert.AreEqual(_ownersPetFlattenedSorted, actual, new OwnersPetsFlattenedDTOComparer());
        }
        #endregion
    }
}
