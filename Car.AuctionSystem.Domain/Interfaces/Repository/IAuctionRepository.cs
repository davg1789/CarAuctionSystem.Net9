using Car.AuctionSystem.Domain.Entities;

namespace Car.AuctionSystem.Domain.Interfaces.Repository
{
    public interface IAuctionRepository : IRepository<Auction>
    {
        Task<Auction?> GetWithBidsByIdAsync(Guid id);
        Task<Auction?> GetActiveByVehicleIdAsync(Guid vehicleId);
        Task<bool> HasPastAuctionWithBidsAsync(Guid vehicleId);
    }
}
