using Moq;
using Parkomatik.Models;

namespace ParkomatikUnitTest
{
    [TestClass]
    public class ParkingHelperTest
    {
        [TestMethod]
        public void ShouldCreatePass()
        {
            // Arrange
            var mockDbContext = new Mock<ParkingContext>();

            var addedPasses = new List<Pass>();
            var savedPasses = new List<Pass>();

            mockDbContext.Setup(x => x.Passes.Add(It.IsAny<Pass>())).Callback((Pass pass) =>
            {
                addedPasses.Add(pass);
            });
            mockDbContext.Setup(x => x.SaveChanges()).Callback(() =>
            {
                savedPasses.AddRange(addedPasses);
                addedPasses.Clear();
            });

            var parkingHelper = new ParkingHelper(mockDbContext.Object);

            // Act
            parkingHelper.CreatePass("Customer1", true, 10);

            // Assert
            Assert.AreEqual(1, savedPasses.Count());

            var savedPass = savedPasses.First();
            Assert.AreEqual("Customer1", savedPass.Purchaser);
            Assert.AreEqual(true, savedPass.Premium);
            Assert.AreEqual(10, savedPass.Capacity);
        }       
    }
}