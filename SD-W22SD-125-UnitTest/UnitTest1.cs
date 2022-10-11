namespace SD_W22SD_125_UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public VehicleTracker VehicleTracker { get; set; }

        public UnitTest1()
        {
            VehicleTracker = new VehicleTracker(10, "123 Fake St");
        }

        [TestMethod]
        public void ShouldHaveEmptySlotsFrom1WhenInitialized()
        {
            Assert.AreEqual(VehicleTracker.Capacity, VehicleTracker.VehicleList.Count);
            for (int i = 1; i <= VehicleTracker.Capacity; i++)
            {
                Assert.IsNull(VehicleTracker.VehicleList[i]);
            }
        }
    }
}