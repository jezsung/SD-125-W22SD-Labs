using Microsoft.EntityFrameworkCore;
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

        [TestMethod]
        [DataRow("A")]
        [DataRow("AB")]
        [DataRow("ABCDEFGHIJKLMNOPQRSTU")]
        public void ShouldThrowExceptionForCreatePassWhenPurchaserLengthNotBetween3To20(string purchaser)
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

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                parkingHelper.CreatePass(purchaser, true, 10);
            });
        }

        [TestMethod]
        [DataRow(-3)]
        [DataRow(0)]
        public void ShouldThrowExceptionForCreatePassWhenCapacityIsNegative(int capacity)
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

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                parkingHelper.CreatePass("Customer1", true, capacity);
            });
        }

        [TestMethod]
        public void ShouldCreateParkingSpot()
        {
            // Arrange
            var mockDbContext = new Mock<ParkingContext>();

            var addedParkingSpots = new List<ParkingSpot>();
            var savedParkingSpots = new List<ParkingSpot>();

            mockDbContext.Setup(x => x.ParkingSpots.Add(It.IsAny<ParkingSpot>())).Callback((ParkingSpot parkingSpot) =>
            {
                addedParkingSpots.Add(parkingSpot);
            });
            mockDbContext.Setup(x => x.SaveChanges()).Callback(() =>
            {
                savedParkingSpots.AddRange(addedParkingSpots);
                addedParkingSpots.Clear();
            });

            var parkingHelper = new ParkingHelper(mockDbContext.Object);

            // Act
            parkingHelper.CreateParkingSpot();

            // Assert
            Assert.AreEqual(1, savedParkingSpots.Count());
        }

        [TestMethod]
        public void ShouldAddVehicleToPass()
        {
            var mockDbContext = new Mock<ParkingContext>();

            var testDirtyPasses = new List<Pass>();
            var testPasses = new List<Pass>();
            var testPass = new Pass()
            {
                ID = 1,
                Purchaser = "Customer1",
                Premium = false,
                Capacity = 3
            };
            var testVehicle = new Vehicle()
            {
                ID = 1,
                License = "ABC101",
                Parked = false
            };

            mockDbContext
                .Setup(x => x.Passes.First(p => It.IsAny<bool>()))
                .Returns(testPass);
            mockDbContext
                .Setup(x => x.Vehicles.First(It.IsAny<Func<Vehicle, bool>>()))
                .Returns(testVehicle);
            mockDbContext
                .Setup(x => x.Passes.Update(It.IsAny<Pass>()))
                .Callback((Pass pass) =>
                {
                    testDirtyPasses.Add(pass);
                });
            mockDbContext
                .Setup(x => x.SaveChanges())
                .Callback((Pass pass) =>
                {
                    testPasses.RemoveAll(p => testDirtyPasses.FirstOrDefault(dp => dp.ID == p.ID) != null);
                    testPasses.AddRange(testDirtyPasses);
                });

            var parkingHelper = new ParkingHelper(mockDbContext.Object);

            // Act
            parkingHelper.AddVehicleToPass(testPass.Purchaser, testVehicle.License);

            // Assert
            Assert.AreEqual(1, testPasses.Count());
            Assert.AreEqual(1, testPasses.First().Vehicles.Count());
        }
    }
}