using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Entities.Enum;

namespace Car.AuctionSystem.Application.Factory
{
    public static class VehicleListResponseFactory
    {
        public static VehicleListResponse Create(Vehicle vehicle)
        {
            return vehicle.Type switch
            {
                VehicleType.Sedan => new SedanListResponse
                {
                    Id = vehicle.Id,
                    Manufacturer = vehicle.Manufacturer,
                    Model = vehicle.Model,
                    Year = vehicle.Year,
                    StartingBid = vehicle.StartingBid,
                    NumberOfDoors = ((Sedan)vehicle).NumberOfDoors
                },

                VehicleType.Hatchback => new HatchbackListResponse
                {
                    Id = vehicle.Id,
                    Manufacturer = vehicle.Manufacturer,
                    Model = vehicle.Model,
                    Year = vehicle.Year,
                    StartingBid = vehicle.StartingBid,
                    NumberOfDoors = ((Hatchback)vehicle).NumberOfDoors
                },

                VehicleType.SUV => new SuvListResponse
                {
                    Id = vehicle.Id,
                    Manufacturer = vehicle.Manufacturer,
                    Model = vehicle.Model,
                    Year = vehicle.Year,
                    StartingBid = vehicle.StartingBid,
                    NumberOfSeats = ((Suv)vehicle).NumberOfSeats
                },

                VehicleType.Truck => new TruckListResponse
                {
                    Id = vehicle.Id,
                    Manufacturer = vehicle.Manufacturer,
                    Model = vehicle.Model,
                    Year = vehicle.Year,
                    StartingBid = vehicle.StartingBid,
                    LoadCapacity = ((Truck)vehicle).LoadCapacity
                },

                _ => new VehicleListResponse
                {
                    Id = vehicle.Id,
                    Manufacturer = vehicle.Manufacturer,
                    Model = vehicle.Model,
                    Year = vehicle.Year,
                    StartingBid = vehicle.StartingBid
                }
            };
        }
    }
}
