using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Interfaces.Repository;
using Car.AuctionSystem.Domain.Services;
using Moq;

namespace Car.AuctionSystem.Test.UnitTest.Domain
{
    public class AuctionServiceTests
    {
        private readonly Mock<IAuctionRepository> _repositoryMock;
        private readonly AuctionService _service;

        public AuctionServiceTests()
        {
            _repositoryMock = new Mock<IAuctionRepository>();
            _service = new AuctionService(_repositoryMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnAddedAuction()
        {
            var auction = new Auction { Id = Guid.NewGuid() };
            _repositoryMock.Setup(r => r.AddAsync(auction)).ReturnsAsync(auction);

            var result = await _service.AddAsync(auction);

            Assert.Equal(auction, result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAuction()
        {
            var id = Guid.NewGuid();
            var auction = new Auction { Id = id };
            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(auction);

            var result = await _service.GetByIdAsync(id);

            Assert.Equal(auction, result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAuctions()
        {
            var auctions = new List<Auction> { new Auction { Id = Guid.NewGuid() } };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(auctions);

            var result = await _service.GetAllAsync();

            Assert.Equal(auctions, result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedAuction()
        {
            var auction = new Auction { Id = Guid.NewGuid() };
            _repositoryMock.Setup(r => r.UpdateAsync(auction)).ReturnsAsync(auction);

            var result = await _service.UpdateAsync(auction);

            Assert.Equal(auction, result);
        }

        [Fact]
        public async Task GetWithBidsByIdAsync_ShouldReturnAuctionWithBids()
        {
            var id = Guid.NewGuid();
            var auction = new Auction { Id = id };
            _repositoryMock.Setup(r => r.GetWithBidsByIdAsync(id)).ReturnsAsync(auction);

            var result = await _service.GetWithBidsByIdAsync(id);

            Assert.Equal(auction, result);
        }

        [Fact]
        public async Task GetActiveByVehicleIdAsync_ShouldReturnAuction()
        {
            var vehicleId = Guid.NewGuid();
            var auction = new Auction { Id = Guid.NewGuid(), VehicleId = vehicleId, IsActive = true };
            _repositoryMock.Setup(r => r.GetActiveByVehicleIdAsync(vehicleId)).ReturnsAsync(auction);

            var result = await _service.GetActiveByVehicleIdAsync(vehicleId);

            Assert.Equal(auction, result);
        }

        [Fact]
        public async Task HasPastAuctionWithBidsAsync_ShouldReturnTrueIfExists()
        {
            var vehicleId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.HasPastAuctionWithBidsAsync(vehicleId)).ReturnsAsync(true);

            var result = await _service.HasPastAuctionWithBidsAsync(vehicleId);

            Assert.True(result);
        }

        [Fact]
        public async Task HasPastAuctionWithBidsAsync_ShouldReturnFalseIfNotExists()
        {
            var vehicleId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.HasPastAuctionWithBidsAsync(vehicleId)).ReturnsAsync(false);

            var result = await _service.HasPastAuctionWithBidsAsync(vehicleId);

            Assert.False(result);
        }
    }
}
