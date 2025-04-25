using Car.AuctionSystem.Api.Controllers;
using Car.AuctionSystem.Application.Interface;
using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Car.AuctionSystem.Test.UnitTest.Controllers
{
    public class BidControllerTests
    {
        private readonly Mock<IBidAppService> _serviceMock;
        private readonly BidController _controller;

        public BidControllerTests()
        {
            _serviceMock = new Mock<IBidAppService>();
            _controller = new BidController(_serviceMock.Object);
        }

        [Fact]
        public async Task PlaceBid_ShouldReturnCreatedAtAction()
        {
            var viewModel = new BidCreateViewModel { AuctionId = Guid.NewGuid(), Amount = 500, Bidder = "John Doe" };
            var bid = new BidResponse { Id = Guid.NewGuid(), Amount = 500, Bidder = "John Doe", PlacedAt = DateTime.UtcNow };

            _serviceMock.Setup(s => s.PlaceBidAsync(viewModel)).ReturnsAsync(bid);

            var result = await _controller.PlaceBid(viewModel);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(bid, createdResult.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenBidExists()
        {
            var bidId = Guid.NewGuid();
            var bid = new BidResponse { Id = bidId, Amount = 700, Bidder = "Jane Smith", PlacedAt = DateTime.UtcNow };

            _serviceMock.Setup(s => s.GetByIdAsync(bidId)).ReturnsAsync(bid);

            var result = await _controller.GetById(bidId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(bid, okResult.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenBidNotExists()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((BidResponse)null);

            var result = await _controller.GetById(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetByAuction_ShouldReturnListOfBids()
        {
            var auctionId = Guid.NewGuid();
            var bids = new List<BidResponse>
            {
                new BidResponse { Id = Guid.NewGuid(), Amount = 1000, Bidder = "Bidder A", PlacedAt = DateTime.UtcNow },
                new BidResponse { Id = Guid.NewGuid(), Amount = 1200, Bidder = "Bidder B", PlacedAt = DateTime.UtcNow }
            };

            _serviceMock.Setup(s => s.GetByAuctionIdAsync(auctionId)).ReturnsAsync(bids);

            var result = await _controller.GetByAuction(auctionId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(bids, okResult.Value);
        }
    }
}
