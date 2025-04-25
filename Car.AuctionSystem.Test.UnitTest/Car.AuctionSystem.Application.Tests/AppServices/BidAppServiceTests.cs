using Car.AuctionSystem.Application.ViewModel;
using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Interfaces.Service;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Car.AuctionSystem.Test.UnitTest.AppServices
{
    public class BidAppServiceTests
    {
        private readonly Mock<IBidService> _bidServiceMock;
        private readonly Mock<IValidator<BidCreateViewModel>> _validatorMock;
        private readonly BidAppService _appService;

        public BidAppServiceTests()
        {
            _bidServiceMock = new Mock<IBidService>();
            _validatorMock = new Mock<IValidator<BidCreateViewModel>>();
            _appService = new BidAppService(_bidServiceMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldReturnBid_WhenValid()
        {
            var model = new BidCreateViewModel
            {
                AuctionId = Guid.NewGuid(),
                Amount = 500,
                Bidder = "John Doe"
            };

            var expectedBid = new Bid { Id = Guid.NewGuid(), Amount = model.Amount, Bidder = model.Bidder };

            _validatorMock.Setup(v => v.ValidateAsync(model, default))
                .ReturnsAsync(new ValidationResult());

            _bidServiceMock.Setup(s => s.PlaceBidAsync(model.AuctionId, model.Amount, model.Bidder))
                .ReturnsAsync(expectedBid);

            var result = await _appService.PlaceBidAsync(model);

            Assert.Equal(expectedBid.AuctionId, result.AuctionId);
            Assert.Equal(expectedBid.Amount, result.Amount);
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldThrowValidationException_WhenInvalid()
        {
            var model = new BidCreateViewModel
            {
                AuctionId = Guid.NewGuid(),
                Amount = -10,
                Bidder = "Simon"
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Amount", "Amount must be greater than 0")
            });

            _validatorMock.Setup(v => v.ValidateAsync(model, default))
                .ReturnsAsync(validationResult);

            var exception = await Assert.ThrowsAsync<ValidationException>(() => _appService.PlaceBidAsync(model));

            Assert.Contains("Validation failed", exception.Message);
            Assert.Contains("Amount", exception.Message);
            Assert.Contains("Amount must be greater than 0", exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnBid()
        {
            var bidId = Guid.NewGuid();
            var expectedBid = new Bid { Id = bidId };

            _bidServiceMock.Setup(s => s.GetByIdAsync(bidId))
                .ReturnsAsync(expectedBid);

            var result = await _appService.GetByIdAsync(bidId);

            Assert.Equal(expectedBid.Id, result.Id);
        }

        [Fact]
        public async Task GetByAuctionIdAsync_ShouldReturnBids()
        {
            var auctionId = Guid.NewGuid();
            var expectedBids = new List<Bid> { new Bid { Id = Guid.NewGuid(), Amount = 100 } };

            _bidServiceMock.Setup(s => s.GetByAuctionIdAsync(auctionId))
                .ReturnsAsync(expectedBids);

            var result = await _appService.GetByAuctionIdAsync(auctionId);

            Assert.Equal(expectedBids.Count, result.Count());
        }
    }
}
