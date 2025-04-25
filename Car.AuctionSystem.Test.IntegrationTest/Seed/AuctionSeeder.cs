
using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Infra.Data.Context;
using Car.AuctionSystem.Test.IntegrationTest.Seed.IntegrationTests.VehicleTests.Seeders;

namespace Car.AuctionSystem.Test.IntegrationTest.Seed
{
    public static class AuctionSeeder
    {
        public static readonly Guid AuctionId = Guid.Parse("55555555-5555-5555-5555-555555555524");
        public static readonly Guid AuctionIdActive = Guid.Parse("55555555-5555-5555-5555-555555555523");
        public static readonly Guid AuctionIdActiveToClose = Guid.Parse("a701f846-0f06-4df1-bdec-b4149030e9db");
        public static void Seed(CarAuctionSystemContext context)
        {
            var vehicle = context.Vehicles.FirstOrDefault();
            if (vehicle == null) return;

            if (!context.Auctions.Any())
            {
                context.Auctions.Add(new Auction { VehicleId = vehicle.Id, StartTime = DateTime.UtcNow.AddHours(-1), Id = AuctionId } );
                context.Auctions.Add(new Auction { VehicleId = VehicleSeeder.TruckId, StartTime = DateTime.UtcNow.AddHours(-1), Id = AuctionIdActive, IsActive = true });
                context.Auctions.Add(new Auction { VehicleId = VehicleSeeder.SuvId, StartTime = DateTime.UtcNow.AddHours(-1), Id = AuctionIdActiveToClose, IsActive = true });
                context.SaveChanges();
            }
        }
    }
}
