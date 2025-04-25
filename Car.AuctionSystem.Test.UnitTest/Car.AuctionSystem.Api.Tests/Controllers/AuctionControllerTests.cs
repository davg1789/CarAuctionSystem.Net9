using Car.AuctionSystem.Api.Controllers;
using Car.AuctionSystem.Application.Interface;
using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Car.AuctionSystem.Test.UnitTest.Controllers
{
    public class AuctionControllerTests
    {
        private readonly Mock<IAuctionAppService> _serviceMock;
        private readonly AuctionController _controller;

        public AuctionControllerTests()
        {
            _serviceMock = new Mock<IAuctionAppService>();
            _controller = new AuctionController(_serviceMock.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction()
        {
            var viewModel = new AuctionCreateViewModel { VehicleId = Guid.NewGuid(), StartTime = DateTime.UtcNow };
            var auction = new AuctionResponse { Id = Guid.NewGuid(), VehicleId = viewModel.VehicleId, StartTime = viewModel.StartTime };

            _serviceMock.Setup(s => s.CreateAuctionAsync(viewModel)).ReturnsAsync(auction);

            var result = await _controller.Create(viewModel);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(auction, createdResult.Value);
        }

        [Fact]
        public async Task Start_ShouldReturnOkWithAuction()
        {
            var auctionId = Guid.NewGuid();
            var auction = new AuctionResponse { Id = auctionId, IsActive = true };

            _serviceMock.Setup(s => s.StartAuctionAsync(auctionId)).ReturnsAsync(auction);

            var result = await _controller.Start(auctionId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(auction, okResult.Value);
        }

        [Fact]
        public async Task Close_ShouldReturnOkResult()
        {
            var auctionId = Guid.NewGuid();
            var auction = new AuctionResponse { Id = auctionId, IsActive = false };
            _serviceMock.Setup(s => s.CloseAuctionAsync(auctionId)).ReturnsAsync(auction);

            var result = await _controller.Close(auctionId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenExists()
        {
            var auction = new AuctionResponse { Id = Guid.NewGuid() };

            _serviceMock.Setup(s => s.GetByIdAsync(auction.Id)).ReturnsAsync(auction);

            var result = await _controller.GetById(auction.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(auction, okResult.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenNull()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((AuctionResponse)null);

            var result = await _controller.GetById(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllAuctions()
        {
            var auctions = new List<AuctionResponse>
            {
                new AuctionResponse { Id = Guid.NewGuid() },
                new AuctionResponse { Id = Guid.NewGuid() }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(auctions);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(auctions, okResult.Value);
        }
    }
}
