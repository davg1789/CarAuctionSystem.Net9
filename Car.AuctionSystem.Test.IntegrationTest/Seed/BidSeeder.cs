
using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Infra.Data.Context;

namespace Car.AuctionSystem.Test.IntegrationTest.Seed
{
    public static class BidSeeder
    {
        public static void Seed(CarAuctionSystemContext context)
        {
            if (!context.Bids.Any())
            {
                var bid = new Bid { AuctionId = AuctionSeeder.AuctionId, Amount = 10500, Bidder = "Daniel" };
                var secondBid = new Bid { AuctionId = AuctionSeeder.AuctionId, Amount = 11500, Bidder = "Augusto" };
                bid.Id = Guid.Parse("189c078e-a08f-45cf-ae14-308569216836");
                secondBid.Id = Guid.Parse("d38dcd45-314d-48a8-acca-3381763cf31b");
                context.Bids.AddRange(new List<Bid>
                {
                    bid,
                    secondBid
                });
                context.SaveChanges();
            }
        }
    }
}
