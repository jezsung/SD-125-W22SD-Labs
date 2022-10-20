namespace Parkomatik.Models
{
    public class Pass
    {
        public int ID { get; set; }
        public string Purchaser { get; set; }
        public bool Premium { get; set; }
        public int Capacity { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
