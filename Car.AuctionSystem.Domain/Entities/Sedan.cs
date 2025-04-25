using Car.AuctionSystem.Domain.Entities.Enum;

namespace Car.AuctionSystem.Domain.Entities
{
    public class Sedan : Vehicle
    {
        public Sedan() { }

        public Sedan(string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
         : base(VehicleType.Sedan, manufacturer, model, year, startingBid)
        {
            NumberOfDoors = numberOfDoors;
        }

        public int NumberOfDoors { get;  set; }     
    }
}
