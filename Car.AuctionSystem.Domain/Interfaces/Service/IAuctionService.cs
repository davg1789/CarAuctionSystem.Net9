using Car.AuctionSystem.Domain.Entities;

namespace Car.AuctionSystem.Domain.Interfaces.Service
{
    public interface IAuctionService
    {
        Task<Auction> AddAsync(Auction auction);
        Task<Auction?> GetByIdAsync(Guid id);
        Task<IEnumerable<Auction>> GetAllAsync();
        Task<Auction> UpdateAsync(Auction auction);
        Task<Auction?> GetWithBidsByIdAsync(Guid id);
        Task<Auction?> GetActiveByVehicleIdAsync(Guid vehicleId);
        Task<bool> HasPastAuctionWithBidsAsync(Guid vehicleId);
    }
}
