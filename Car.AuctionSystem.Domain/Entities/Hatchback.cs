using Car.AuctionSystem.Domain.Entities.Enum;

namespace Car.AuctionSystem.Domain.Entities
{
    public class Hatchback : Vehicle
    {
        public Hatchback() { }

        public Hatchback(string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
          : base(VehicleType.Hatchback, manufacturer, model, year, startingBid)
        {
            NumberOfDoors = numberOfDoors;
        }

        public int NumberOfDoors { get; set; }
    }
}
