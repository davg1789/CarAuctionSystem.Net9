using Car.AuctionSystem.Infra.Data.Context;
using Car.AuctionSystem.Test.IntegrationTest.Seed.IntegrationTests.VehicleTests.Seeders;

namespace Car.AuctionSystem.Test.IntegrationTest.Seed
{
    public static class DatabaseSeeder
    {
        public static CarAuctionSystemContext Seed(CarAuctionSystemContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            VehicleSeeder.Seed(context);
            AuctionSeeder.Seed(context);
            BidSeeder.Seed(context);           

            return context;
        }
    }
}
