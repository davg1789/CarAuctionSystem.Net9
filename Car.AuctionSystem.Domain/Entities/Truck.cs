using Car.AuctionSystem.Domain.Entities.Enum;

namespace Car.AuctionSystem.Domain.Entities
{
    public class Truck : Vehicle
    {
        public Truck() { }

        public Truck(string manufacturer, string model, int year, decimal startingBid, double loadCapacity)
           : base(VehicleType.Truck, manufacturer, model, year, startingBid)
        {
            LoadCapacity = loadCapacity;
        }

        public double LoadCapacity { get; set; } 
    }
}
