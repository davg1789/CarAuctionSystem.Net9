using Car.AuctionSystem.Application.Implementation;
using Car.AuctionSystem.Application.ViewModel;
using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Interfaces.Service;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Car.AuctionSystem.Test.UnitTest.AppServices
{
    public class AuctionAppServiceTests
    {
        private readonly Mock<IAuctionService> _auctionServiceMock;
        private readonly Mock<IVehicleService> _vehicleServiceMock;
        private readonly Mock<IValidator<AuctionCreateViewModel>> _validatorMock;
        private readonly AuctionAppService _appService;

        public AuctionAppServiceTests()
        {
            _auctionServiceMock = new Mock<IAuctionService>();
            _vehicleServiceMock = new Mock<IVehicleService>();
            _validatorMock = new Mock<IValidator<AuctionCreateViewModel>>();
            _appService = new AuctionAppService(_auctionServiceMock.Object, _vehicleServiceMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldThrowValidationException_WhenInvalid()
        {
            var model = new AuctionCreateViewModel();
            var failures = new List<ValidationFailure> { new ValidationFailure("Field", "Error") };
            _validatorMock.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Field", "Error") }));

            var exception = await Assert.ThrowsAsync<ValidationException>(() => _appService.CreateAuctionAsync(model));

            Assert.Contains("Field", exception.Message);
            Assert.Contains("Error", exception.Message);                        
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldThrow_WhenVehicleNotFound()
        {
            var model = new AuctionCreateViewModel { VehicleId = Guid.NewGuid(), StartTime = DateTime.UtcNow };
            _validatorMock.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _vehicleServiceMock.Setup(s => s.GetByIdAsync(model.VehicleId)).ReturnsAsync((Vehicle)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _appService.CreateAuctionAsync(model));

            Assert.Equal("Vehicle not found.", exception.Message);
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldReturnAuction_WhenValid()
        {
            var model = new AuctionCreateViewModel { VehicleId = Guid.NewGuid(), StartTime = DateTime.UtcNow };
            var vehicle = new Sedan("Toyota", "Corolla", 2023, 10000, 4);
            var auction = new Auction(vehicle, model.StartTime.Value);

            _validatorMock.Setup(v => v.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _vehicleServiceMock.Setup(s => s.GetByIdAsync(model.VehicleId)).ReturnsAsync(vehicle);
            _auctionServiceMock.Setup(s => s.AddAsync(It.IsAny<Auction>())).ReturnsAsync(auction);

            var result = await _appService.CreateAuctionAsync(model);

            Assert.Equal(auction.StartTime, result.StartTime);
            Assert.Equal(auction.Id, result.Id);
        }

        [Fact]
        public async Task StartAuctionAsync_ShouldThrow_WhenNotFound()
        {
            _auctionServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Auction)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _appService.StartAuctionAsync(Guid.NewGuid()));

            Assert.Equal("Auction not found.", exception.Message);
        }

        [Fact]
        public async Task StartAuctionAsync_ShouldThrow_WhenAlreadyActive()
        {
            var auction = new Auction { Id = Guid.NewGuid(), IsActive = true };

            _auctionServiceMock.Setup(s => s.GetByIdAsync(auction.Id)).ReturnsAsync(auction);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _appService.StartAuctionAsync(auction.Id));

            Assert.Equal("Auction is already active.", exception.Message);
        }

        [Fact]
        public async Task StartAuctionAsync_ShouldActivateAuction()
        {
            var auction = new Auction { Id = Guid.NewGuid(), IsActive = false, VehicleId = Guid.NewGuid() };

            _auctionServiceMock.Setup(s => s.GetByIdAsync(auction.Id)).ReturnsAsync(auction);
            _auctionServiceMock.Setup(s => s.HasPastAuctionWithBidsAsync(auction.VehicleId)).ReturnsAsync(false);
            _auctionServiceMock.Setup(s => s.GetActiveByVehicleIdAsync(auction.VehicleId)).ReturnsAsync((Auction)null);
            _auctionServiceMock.Setup(s => s.UpdateAsync(It.IsAny<Auction>())).ReturnsAsync(auction);

            var result = await _appService.StartAuctionAsync(auction.Id);

            Assert.True(result.IsActive);
        }

        [Fact]
        public async Task CloseAuctionAsync_ShouldThrow_WhenAlreadyClosed()
        {
            var auction = new Auction { Id = Guid.NewGuid(), EndTime = DateTime.UtcNow };

            _auctionServiceMock.Setup(s => s.GetByIdAsync(auction.Id)).ReturnsAsync(auction);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _appService.CloseAuctionAsync(auction.Id));

            Assert.Equal("Auction is already closed.", exception.Message);
        }

        [Fact]
        public async Task CloseAuctionAsync_ShouldThrow_WhenNotActive()
        {
            var auction = new Auction { Id = Guid.NewGuid(), IsActive = false };

            _auctionServiceMock.Setup(s => s.GetByIdAsync(auction.Id)).ReturnsAsync(auction);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _appService.CloseAuctionAsync(auction.Id));

            Assert.Equal("Auction is not active.", exception.Message);
        }

        [Fact]
        public async Task CloseAuctionAsync_ShouldSetInactiveAndEndTime()
        {
            var auction = new Auction { Id = Guid.NewGuid(), IsActive = true };

            _auctionServiceMock.Setup(s => s.GetByIdAsync(auction.Id)).ReturnsAsync(auction);
            _auctionServiceMock.Setup(s => s.UpdateAsync(auction)).ReturnsAsync(auction);

            await _appService.CloseAuctionAsync(auction.Id);

            Assert.False(auction.IsActive);
            Assert.NotNull(auction.EndTime);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WithInvalidAuction()
        {
            var auction = new Auction { Id = Guid.NewGuid() };            

            var result = await _appService.GetByIdAsync(auction.Id);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAuction()
        {
            var auction = new Auction { Id = Guid.NewGuid(), EndTime = DateTime.Now, StartTime = DateTime.Now, IsActive = true,
                Bids = new List<Bid> { new Bid {  Amount = 500, Bidder = "James"} } };

            _auctionServiceMock.Setup(s => s.GetWithBidsByIdAsync(auction.Id)).ReturnsAsync(auction);

            var result = await _appService.GetByIdAsync(auction.Id);

            Assert.Equal(auction.Id, result.Id);
            Assert.Equal(auction.IsActive, result.IsActive);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAuctions()
        {
            var auctions = new List<Auction> { new Auction { Id = Guid.NewGuid() } };

            _auctionServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(auctions);

            var result = await _appService.GetAllAsync();

            Assert.Equal(auctions.Count, result.Count());
        }

        [Fact]
        public async Task StartAuctionAsync_ShouldThrow_WhenVehicleHadPastAuctionWithBids()
        {
            var auction = new Auction { Id = Guid.NewGuid(), IsActive = false, VehicleId = Guid.NewGuid() };

            _auctionServiceMock.Setup(s => s.GetByIdAsync(auction.Id)).ReturnsAsync(auction);
            _auctionServiceMock.Setup(s => s.HasPastAuctionWithBidsAsync(auction.VehicleId)).ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _appService.StartAuctionAsync(auction.Id));
        }

        [Fact]
        public async Task StartAuctionAsync_ShouldThrow_WhenAnotherAuctionIsActive()
        {
            var auction = new Auction { Id = Guid.NewGuid(), IsActive = false, VehicleId = Guid.NewGuid() };

            _auctionServiceMock.Setup(s => s.GetByIdAsync(auction.Id)).ReturnsAsync(auction);
            _auctionServiceMock.Setup(s => s.HasPastAuctionWithBidsAsync(auction.VehicleId)).ReturnsAsync(false);
            _auctionServiceMock.Setup(s => s.GetActiveByVehicleIdAsync(auction.VehicleId)).ReturnsAsync(new Auction());

            await Assert.ThrowsAsync<InvalidOperationException>(() => _appService.StartAuctionAsync(auction.Id));
        }
    }
}
