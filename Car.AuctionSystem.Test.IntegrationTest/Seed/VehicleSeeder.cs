using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Entities.Enum;
using Car.AuctionSystem.Infra.Data.Context;

namespace Car.AuctionSystem.Test.IntegrationTest.Seed
{
    namespace IntegrationTests.VehicleTests.Seeders
    {
        public static class VehicleSeeder
        {
            public static readonly Guid VehicleId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            public static readonly Guid TruckId = Guid.Parse("55555555-5555-5555-5555-555555555544");
            public static readonly Guid SuvId = Guid.Parse("4c0379ed-133d-4be0-938f-bede48c67557");
            public static void Seed(CarAuctionSystemContext context)
            {
                if (context.Vehicles.Any()) return;

                var vehicles = new List<Vehicle>
            {
                new Sedan
                {
                    Id = Guid.Parse("096bdb34-1766-4e24-8e37-bf1a2bf06c93"),
                    Type = VehicleType.Sedan,
                    Manufacturer = "Toyota",
                    Model = "Corolla",
                    Year = 2022,
                    StartingBid = 15000,
                    NumberOfDoors = 4
                },
                new Hatchback
                {
                    Id = VehicleId,
                    Type = VehicleType.Hatchback,
                    Manufacturer = "Ford",
                    Model = "Fiesta",
                    Year = 2021,
                    StartingBid = 12000,
                    NumberOfDoors = 4
                },
                new Suv
                {
                    Id = SuvId,
                    Type = VehicleType.SUV,
                    Manufacturer = "Jeep",
                    Model = "Renegade",
                    Year = 2023,
                    StartingBid = 25000,
                    NumberOfSeats = 5
                },
                new Truck
                {
                    Id = TruckId,
                    Type = VehicleType.Truck,
                    Manufacturer = "Volvo",
                    Model = "FH",
                    Year = 2020,
                    StartingBid = 40000,
                    LoadCapacity = 18.5
                }
            };

                context.Vehicles.AddRange(vehicles);
                context.SaveChanges();
            }
        }
    }
}
