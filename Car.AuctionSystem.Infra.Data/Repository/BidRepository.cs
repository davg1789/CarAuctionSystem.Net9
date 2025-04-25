using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Interfaces.Repository;
using Car.AuctionSystem.Infra.Data.Context;
using Car.AuctionSystem.Infra.Data.Repositories;

namespace Car.AuctionSystem.Infra.Data.Repository
{
    public class BidRepository : Repository<Bid>, IBidRepository
    {
        public BidRepository(CarAuctionSystemContext context)
            : base(context)
        {

        }
    }
}
