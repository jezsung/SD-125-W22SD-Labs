namespace SD_W22SD_125_UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ShouldHaveEmptySlotsFrom1WhenInitialized()
        {
            VehicleTracker vehicleTracker = new VehicleTracker(10, "123 Fake St");


            Assert.AreEqual(vehicleTracker.Capacity, vehicleTracker.VehicleList.Count);
            for (int i = 1; i <= vehicleTracker.Capacity; i++)
            {
                Assert.IsNull(vehicleTracker.VehicleList[i]);
            }
        }

        [TestMethod]
        public void ShouldAddVehicleToFirstEmptySlot()
        {
            VehicleTracker vehicleTracker = new VehicleTracker(10, "123 Fake St");
            Vehicle vehicle1 = new Vehicle("A01 T22", true);
            vehicleTracker.AddVehicle(vehicle1);

            Assert.AreEqual(vehicle1, vehicleTracker.VehicleList[1]);
        }

        [TestMethod]
        public void ShouldThrowExceptionWhenAddingVehicleAndThereIsNoOpenSlots()
        {
            VehicleTracker vehicleTracker = new VehicleTracker(3, "123 Fake St");
            Vehicle vehicle1 = new Vehicle("A01 T22", true);
            Vehicle vehicle2 = new Vehicle("A01 T23", true);
            Vehicle vehicle3 = new Vehicle("A01 T24", true);
            vehicleTracker.AddVehicle(vehicle1);
            vehicleTracker.AddVehicle(vehicle2);
            vehicleTracker.AddVehicle(vehicle3);


            Assert.ThrowsException<IndexOutOfRangeException>(() =>
            {
                Vehicle vehicle4 = new Vehicle("A01 T24", true);
                vehicleTracker.AddVehicle(vehicle4);
            },
            VehicleTracker.SlotsFullMessage);
        }

        [TestMethod]
        public void ShouldRemoveVehicleBySlotNumber()
        {
            VehicleTracker vehicleTracker = new VehicleTracker(3, "123 Fake St");
            Vehicle vehicle1 = new Vehicle("A01 T22", true);
            vehicleTracker.AddVehicle(vehicle1);

            vehicleTracker.RemoveVehicle(1);

            Assert.IsNull(vehicleTracker.VehicleList[1]);
        }

        [TestMethod]
        public void ShouldRemoveVehicleByLicense()
        {
            VehicleTracker vehicleTracker = new VehicleTracker(3, "123 Fake St");
            string license = "A01 T22";
            Vehicle vehicle1 = new Vehicle(license, true);
            vehicleTracker.AddVehicle(vehicle1);
            int slotNumber = vehicleTracker.VehicleList.First(v => v.Value.Licence == license).Key;

            vehicleTracker.RemoveVehicle(license);

            Assert.IsNull(vehicleTracker.VehicleList[slotNumber]);
        }

        [TestMethod]
        public void ShouldThrowExceptionWhenSlotNumberIsInvalid()
        {
            int capacity = 3;
            VehicleTracker vehicleTracker = new VehicleTracker(capacity, "123 Fake St");
            Vehicle vehicle1 = new Vehicle("A01 T22", true);
            vehicleTracker.AddVehicle(vehicle1);

            Assert.ThrowsException<IndexOutOfRangeException>(() =>
            {
                vehicleTracker.RemoveVehicle(-1);
            });

            Assert.ThrowsException<IndexOutOfRangeException>(() =>
            {
                vehicleTracker.RemoveVehicle(0);
            });

            Assert.ThrowsException<IndexOutOfRangeException>(() =>
            {
                vehicleTracker.RemoveVehicle(capacity + 1);
            });
        }

        [TestMethod]
        public void ShouldThrowExceptionWhenLicenseIsNotFound()
        {
            VehicleTracker vehicleTracker = new VehicleTracker(3, "123 Fake St");
            Vehicle vehicle1 = new Vehicle("A01 T22", true);
            vehicleTracker.AddVehicle(vehicle1);

            Assert.ThrowsException<NullReferenceException>(() =>
            {
                vehicleTracker.RemoveVehicle("Invalid License");
            });
        }

        [TestMethod]
        public void ShouldTrackNumberOfAvailableSlots()
        {
            int capacity = 3;
            VehicleTracker vehicleTracker = new VehicleTracker(capacity, "123 Fake St");
            Vehicle vehicle1 = new Vehicle("A01 T22", true);

            Assert.AreEqual(3, vehicleTracker.SlotsAvailable);

            vehicleTracker.AddVehicle(vehicle1);

            Assert.AreEqual(2, vehicleTracker.SlotsAvailable);

            vehicleTracker.RemoveVehicle("A01 T22");

            Assert.AreEqual(3, vehicleTracker.SlotsAvailable);
        }

        [TestMethod]
        public void ShouldReturnListOfVehiclesThatHavePass()
        {
            VehicleTracker vehicleTracker = new VehicleTracker(3, "123 Fake St");
            Vehicle vehicle1 = new Vehicle("A01 T22", true);
            Vehicle vehicle2 = new Vehicle("A01 T23", false);
            Vehicle vehicle3 = new Vehicle("A01 T24", true);
            vehicleTracker.AddVehicle(vehicle1);
            vehicleTracker.AddVehicle(vehicle2);
            vehicleTracker.AddVehicle(vehicle3);

            List<Vehicle> parkedPasswordholders = vehicleTracker.ParkedPassholders();

            Assert.IsTrue(parkedPasswordholders.Contains(vehicle1));
            Assert.IsFalse(parkedPasswordholders.Contains(vehicle2));
            Assert.IsTrue(parkedPasswordholders.Contains(vehicle3));
        }

        [TestMethod]
        public void ShouldReturnPercentageOfVehiclesThatHavePass()
        {
            int capacity = 3;
            VehicleTracker vehicleTracker = new VehicleTracker(capacity, "123 Fake St");
            Vehicle vehicle1 = new Vehicle("A01 T22", true);
            Vehicle vehicle2 = new Vehicle("A01 T23", false);
            Vehicle vehicle3 = new Vehicle("A01 T24", true);
            vehicleTracker.AddVehicle(vehicle1);
            vehicleTracker.AddVehicle(vehicle2);
            vehicleTracker.AddVehicle(vehicle3);

            int percentage = vehicleTracker.ParkedPassholders().Count / capacity * 100;

            Assert.AreEqual(percentage, vehicleTracker.PassholderPercentage());
        }
    }
}