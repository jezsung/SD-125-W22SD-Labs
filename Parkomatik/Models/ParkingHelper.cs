using Microsoft.EntityFrameworkCore;

namespace Parkomatik.Models
{
    public class ParkingHelper
    {
        private ParkingContext parkingContext;

        public ParkingHelper(ParkingContext context)
        {
            this.parkingContext = context;
        }

        public Pass CreatePass(string purchaser, bool premium, int capacity)
        {
            if (purchaser.Length < 3 || purchaser.Length > 20)
            {
                throw new ArgumentException("purchaser length should be between 3 to 20");
            }
            if (capacity <= 0)
            {
                throw new ArgumentException("capacity should be greater than zero");
            }

            Pass newPass = new Pass();
            newPass.Purchaser = purchaser;
            newPass.Premium = premium;
            newPass.Capacity = capacity;

            parkingContext.Passes.Add(newPass);
            parkingContext.SaveChanges();

            return newPass;
        }

        public ParkingSpot CreateParkingSpot()
        {
            ParkingSpot newSpot = new ParkingSpot();
            newSpot.Occupied = false;

            parkingContext.ParkingSpots.Add(newSpot);
            parkingContext.SaveChanges();

            return newSpot;
        }

        public void AddVehicleToPass(string passholderName, string vehicleLicense)
        {
            Pass pass = parkingContext.Passes.Include(p => p.Vehicles).First(p => p.Purchaser == passholderName);
            Vehicle vehicle = parkingContext.Vehicles.First(v => v.License == vehicleLicense);

            if (pass.Vehicles.Count() == pass.Capacity)
            {
                throw new IndexOutOfRangeException();
            }

            pass.Vehicles.Add(vehicle);

            parkingContext.Passes.Update(pass);
            parkingContext.SaveChanges();
        }
    }
}
