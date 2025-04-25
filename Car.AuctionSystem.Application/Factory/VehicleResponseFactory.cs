using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Domain.Entities;

namespace Car.AuctionSystem.Application.Factory
{
    public static class VehicleResponseFactory
    {
        public static VehicleResponse Create(Vehicle vehicle)
        {
            return vehicle switch
            {
                Sedan sedan => new SedanResponse
                {
                    Id = sedan.Id,
                    Manufacturer = sedan.Manufacturer,
                    Model = sedan.Model,
                    Year = sedan.Year,
                    StartingBid = sedan.StartingBid,
                    NumberOfDoors = sedan.NumberOfDoors,
                    Type = sedan.Type.ToString()
                },

                Hatchback hatch => new HatchbackResponse
                {
                    Id = hatch.Id,
                    Manufacturer = hatch.Manufacturer,
                    Model = hatch.Model,
                    Year = hatch.Year,
                    StartingBid = hatch.StartingBid,
                    NumberOfDoors = hatch.NumberOfDoors,
                    Type = hatch.Type.ToString()
                },

                Suv suv => new SuvResponse
                {
                    Id = suv.Id,
                    Manufacturer = suv.Manufacturer,
                    Model = suv.Model,
                    Year = suv.Year,
                    StartingBid = suv.StartingBid,
                    NumberOfSeats = suv.NumberOfSeats,
                    Type = suv.Type.ToString()
                },

                Truck truck => new TruckResponse
                {
                    Id = truck.Id,
                    Manufacturer = truck.Manufacturer,
                    Model = truck.Model,
                    Year = truck.Year,
                    StartingBid = truck.StartingBid,
                    LoadCapacity = truck.LoadCapacity,
                    Type = truck.Type.ToString()
                },

                _ => new VehicleResponse
                {
                    Id = vehicle.Id,
                    Manufacturer = vehicle.Manufacturer,
                    Model = vehicle.Model,
                    Year = vehicle.Year,
                    StartingBid = vehicle.StartingBid,
                    Type = vehicle.Type.ToString()
                }
            };
        }
    }
}
