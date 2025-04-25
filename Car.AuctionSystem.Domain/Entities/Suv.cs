using Car.AuctionSystem.Domain.Entities.Enum;

namespace Car.AuctionSystem.Domain.Entities
{
    public class Suv : Vehicle
    {
        public Suv() { }

        public Suv(string manufacturer, string model, int year, decimal startingBid, int numberOfSeats)
         : base(VehicleType.SUV, manufacturer, model, year, startingBid)
        {
            NumberOfSeats = numberOfSeats;
        }

        public int NumberOfSeats { get; set; }     
    }
}
