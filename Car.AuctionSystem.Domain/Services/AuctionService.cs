using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Interfaces.Repository;
using Car.AuctionSystem.Domain.Interfaces.Service;

namespace Car.AuctionSystem.Domain.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _repository;

        public AuctionService(IAuctionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Auction> AddAsync(Auction auction)
        {
            return await _repository.AddAsync(auction);
        }

        public async Task<Auction?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Auction>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Auction> UpdateAsync(Auction auction)
        {
            return await _repository.UpdateAsync(auction);
        }

        public Task<Auction?> GetWithBidsByIdAsync(Guid id)
        {
            return _repository.GetWithBidsByIdAsync(id);
        }

        public async Task<Auction?> GetActiveByVehicleIdAsync(Guid vehicleId)
        {
            return await _repository.GetActiveByVehicleIdAsync(vehicleId);            
        }

        public async Task<bool> HasPastAuctionWithBidsAsync(Guid vehicleId)
        {
            return await _repository.HasPastAuctionWithBidsAsync(vehicleId);
        }
    }
}
