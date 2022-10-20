namespace Parkomatik.Models
{
    public class ParkingSpot
    {
        public int ID { get; set; }
        public bool Occupied { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
