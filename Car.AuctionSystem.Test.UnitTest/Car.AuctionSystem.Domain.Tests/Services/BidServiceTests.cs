using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Interfaces.Repository;
using Car.AuctionSystem.Domain.Services;
using Moq;

namespace Car.AuctionSystem.Test.UnitTest.Domain
{
    public class BidServiceTests
    {
        private readonly Mock<IAuctionRepository> _auctionRepoMock;
        private readonly Mock<IBidRepository> _bidRepoMock;
        private readonly BidService _service;

        public BidServiceTests()
        {
            _auctionRepoMock = new Mock<IAuctionRepository>();
            _bidRepoMock = new Mock<IBidRepository>();
            _service = new BidService(_auctionRepoMock.Object, _bidRepoMock.Object);
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldThrow_WhenAuctionNotFound()
        {
            _auctionRepoMock.Setup(r => r.GetWithBidsByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Auction)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.PlaceBidAsync(Guid.NewGuid(), 1000, "Test Bidder"));
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldThrow_WhenAuctionIsNotActive()
        {
            // Arrange
            var vehicle = new Sedan("Toyota", "Corolla", 2023, 10000, 4);
            var auction = new Auction(vehicle, DateTime.UtcNow) { Id = Guid.NewGuid() };
            auction.IsActive = false; 

            _auctionRepoMock.Setup(r => r.GetWithBidsByIdAsync(auction.Id))
                            .ReturnsAsync(auction);
            _auctionRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Auction>()))
                            .ReturnsAsync(auction);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.PlaceBidAsync(auction.Id, 1000, "Test"));

            Assert.Equal("Unable to bid on an inactive auction.", exception.Message);
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldThrow_WhenBidderNameIsEmpty()
        {
            // Arrange
            var vehicle = new Sedan("Toyota", "Corolla", 2023, 10000, 4);
            var auction = new Auction(vehicle, DateTime.UtcNow) { Id = Guid.NewGuid() };
            auction.IsActive = true;

            _auctionRepoMock.Setup(r => r.GetWithBidsByIdAsync(auction.Id))
                            .ReturnsAsync(auction);
            _auctionRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Auction>()))
                            .ReturnsAsync(auction);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.PlaceBidAsync(auction.Id, 1000, ""));

            Assert.Contains("Bidder name is required.", exception.Message);
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldPlaceBid()
        {
            var bidId = Guid.NewGuid();
            var vehicle = new Sedan("Toyota", "Corolla", 2023, 10000, 4);
            var auction = new Auction(vehicle, DateTime.UtcNow) { Id = Guid.NewGuid(), IsActive = true };
            var bidMock = new Bid { Id = bidId, Amount = 15000, Bidder = "Bidder", PlacedAt = DateTime.UtcNow };

            _auctionRepoMock.Setup(r => r.GetWithBidsByIdAsync(auction.Id)).ReturnsAsync(auction);
            _bidRepoMock.Setup(r => r.AddAsync(It.IsAny<Bid>())).ReturnsAsync(bidMock);

            var bid = await _service.PlaceBidAsync(auction.Id, 20000, "Bidder");

            Assert.NotNull(bid);
            Assert.Equal(15000, bid.Amount);
            Assert.Equal("Bidder", bid.Bidder);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnBid()
        {
            var bidId = Guid.NewGuid();
            var bid = new Bid { Id = bidId, Amount = 1500, Bidder = "Test", PlacedAt = DateTime.UtcNow };

            _bidRepoMock.Setup(r => r.GetByIdAsync(bidId)).ReturnsAsync(bid);

            var result = await _service.GetByIdAsync(bidId);

            Assert.Equal(bidId, result?.Id);
        }

        [Fact]
        public async Task GetByAuctionIdAsync_ShouldThrow_WhenAuctionNotFound()
        {
            _auctionRepoMock.Setup(r => r.GetWithBidsByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Auction)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.GetByAuctionIdAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetByAuctionIdAsync_ShouldReturnBids()
        {
            var auctionId = Guid.NewGuid();
            var auction = new Auction { Id = auctionId };
            var bids = new List<Bid>
            {
                new Bid { Id = Guid.NewGuid(), Amount = 1000, Bidder = "Alice", PlacedAt = DateTime.UtcNow },
                new Bid { Id = Guid.NewGuid(), Amount = 1200, Bidder = "Bob", PlacedAt = DateTime.UtcNow }
            };
            auction.GetType().GetProperty("Bids")?.SetValue(auction, bids);

            _auctionRepoMock.Setup(r => r.GetWithBidsByIdAsync(auctionId)).ReturnsAsync(auction);

            var result = await _service.GetByAuctionIdAsync(auctionId);

            Assert.Equal(2, result.Count());
        }
    }
}
