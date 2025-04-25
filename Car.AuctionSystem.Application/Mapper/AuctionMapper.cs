using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Domain.Entities;

namespace Car.AuctionSystem.Application.Mapper
{
    public static class AuctionMapper
    {
        public static AuctionResponse ToResponse(Auction auction)
        {
            return new AuctionResponse
            {
                Id = auction.Id,
                VehicleId = auction.VehicleId,
                StartTime = auction.StartTime,
                EndTime = auction.EndTime,
                IsActive = auction.IsActive
            };
        }

        public static AuctionWithBidsResponse ToResponseWithBids(Auction auction)
        {
            return new AuctionWithBidsResponse
            {
                Id = auction.Id,
                VehicleId = auction.VehicleId,
                IsActive = auction.IsActive,
                StartTime = auction.StartTime,
                EndTime = auction.EndTime,
                Bids = auction.Bids.Select(b => new BidResponse
                {
                    Id = b.Id,
                    AuctionId = b.AuctionId,
                    Amount = b.Amount,
                    Bidder = b.Bidder,
                    PlacedAt = b.PlacedAt
                }).ToList()
            };
        }
    }
}
