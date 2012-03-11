namespace Shared.Model
{
    public class Trip
    {
        public int Id { get; set; }
        public Transport Transport { get; set; }
        public Accommodation Accommodation { get; set; }
        public decimal Price { get; set; }
    }
}