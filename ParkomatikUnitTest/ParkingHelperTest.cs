using Microsoft.EntityFrameworkCore;
using Moq;
using Parkomatik.Models;

namespace ParkomatikUnitTest
{
    [TestClass]
    public class ParkingHelperTest
    {
        private ParkingContext context;

        [TestInitialize]
        public void TestInitialize()
        {

        }

        [TestMethod]
        public void ShouldCreatePass()
        {
            var passes = new List<Pass>();

            var queryablePasses = passes.AsQueryable();

            var mockPassDbSet = new Mock<DbSet<Pass>>();
            var mockParkingSpotDbSet = new Mock<DbSet<ParkingSpot>>();

            mockPassDbSet.As<IQueryable<Pass>>().Setup(x => x.Provider).Returns(queryablePasses.Provider);
            mockPassDbSet.As<IQueryable<Pass>>().Setup(x => x.Expression).Returns(queryablePasses.Expression);
            mockPassDbSet.As<IQueryable<Pass>>().Setup(x => x.ElementType).Returns(queryablePasses.ElementType);
            mockPassDbSet.As<IQueryable<Pass>>().Setup(x => x.GetEnumerator()).Returns(queryablePasses.GetEnumerator());

            var mockDbContext = new Mock<ParkingContext>();

            mockDbContext.Setup(x => x.Passes).Returns(mockPassDbSet.Object);

            var parkingHelper = new ParkingHelper(mockDbContext.Object);

            parkingHelper.CreatePass("Customer1", true, 10);

            Assert.AreEqual(1, mockPassDbSet.Object.Count());
        }
    }
}