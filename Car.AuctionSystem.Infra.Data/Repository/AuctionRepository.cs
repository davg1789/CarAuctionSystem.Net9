using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Interfaces.Repository;
using Car.AuctionSystem.Infra.Data.Context;
using Car.AuctionSystem.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Car.AuctionSystem.Infra.Data.Repository
{
    public class AuctionRepository : Repository<Auction>, IAuctionRepository
    {
        public AuctionRepository(CarAuctionSystemContext context)
            : base(context)
        {

        }

        public async Task<Auction?> GetWithBidsByIdAsync(Guid id)
        {
            return await DbSet
                .Include(a => a.Bids)
                .Include(a => a.Vehicle)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Auction?> GetActiveByVehicleIdAsync(Guid vehicleId)
        {
            return await Db.Auctions
                .Where(a => a.VehicleId == vehicleId && a.IsActive && a.EndTime == null)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasPastAuctionWithBidsAsync(Guid vehicleId)
        {
            return await Db.Auctions
                .Include(a => a.Bids)
                .Where(a => a.VehicleId == vehicleId && !a.IsActive && a.EndTime != null)
                .AnyAsync(a => a.Bids != null && a.Bids.Any());
        }
    }
}
