using Car.AuctionSystem.Domain.Entities.Enum;

namespace Car.AuctionSystem.Domain.Entities
{
    public abstract class Vehicle
    {
        public Guid Id { get; set; }
        public VehicleType Type { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public decimal StartingBid { get; set; }
        public virtual ICollection<Auction> Auctions { get; set; } = new List<Auction>();

        protected Vehicle() { }

        protected Vehicle(VehicleType type, string manufacturer, string model, int year, decimal startingBid)
        {
            Id = Guid.NewGuid();
            Type = type;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
            StartingBid = startingBid;            
        }
    }
}
