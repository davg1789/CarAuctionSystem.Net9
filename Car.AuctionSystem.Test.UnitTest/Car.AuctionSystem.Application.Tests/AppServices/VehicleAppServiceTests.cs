using Car.AuctionSystem.Application.ViewModel;
using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Entities.Enum;
using Car.AuctionSystem.Domain.Interfaces.Service;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Linq.Expressions;

namespace Car.AuctionSystem.Test.UnitTest.AppServices
{
    public class VehicleAppServiceTests
    {
        private readonly Mock<IVehicleService> _vehicleServiceMock;
        private readonly Mock<IValidator<VehicleViewModel>> _validatorMock;
        private readonly VehicleAppService _appService;

        public VehicleAppServiceTests()
        {
            _vehicleServiceMock = new Mock<IVehicleService>();
            _validatorMock = new Mock<IValidator<VehicleViewModel>>();
            _appService = new VehicleAppService(_vehicleServiceMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task AddVehicleAsync_ShouldThrowValidationException_WhenInvalid()
        {
            var viewModel = new VehicleViewModel();
            _validatorMock.Setup(v => v.ValidateAsync(viewModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Field", "Error") }));

            var exception = await Assert.ThrowsAsync<ValidationException>(() => _appService.AddVehicleAsync(viewModel));

            Assert.Contains("Validation failed", exception.Message);
            Assert.Contains("Field", exception.Message);
            Assert.Contains("Error", exception.Message);
        }

        [Fact]
        public async Task AddVehicleAsync_ShouldReturnVehicle_WhenValid()
        {
            var viewModel = new VehicleViewModel
            {
                Manufacturer = "Toyota",
                Model = "Corolla",
                Year = 2022,
                StartingBid = 10000,
                Type = VehicleType.Sedan,
                NumberOfDoors = 4
            };

            _validatorMock.Setup(v => v.ValidateAsync(viewModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _vehicleServiceMock.Setup(s => s.AddAsync(It.IsAny<Vehicle>()))
                .ReturnsAsync((Vehicle v) => v);

            var result = await _appService.AddVehicleAsync(viewModel);

            Assert.NotNull(result);
            Assert.Equal(viewModel.Manufacturer, result.Manufacturer);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnVehicle()
        {
            var vehicle = new Sedan("Toyota", "Corolla", 2022, 10000, 4);
            _vehicleServiceMock.Setup(s => s.GetByIdAsync(vehicle.Id)).ReturnsAsync(vehicle);

            var result = await _appService.GetByIdAsync(vehicle.Id);

            Assert.Equal(vehicle.Manufacturer, result.Manufacturer);
            Assert.Equal(vehicle.Model, result.Model);
            Assert.Equal(vehicle.Year, result.Year);
            Assert.Equal(vehicle.StartingBid, result.StartingBid);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllVehicles()
        {
            var vehicles = new List<Vehicle> { new Sedan("Toyota", "Corolla", 2022, 10000, 4) };
            _vehicleServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(vehicles);

            var result = await _appService.GetAllAsync();

            Assert.Equal(vehicles.Count, result.Count());
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnFilteredVehicles()
        {
            var filter = new VehicleSearchViewModel { Manufacturer = "Toyota" };
            var vehicles = new List<Vehicle> { new Sedan("Toyota", "Corolla", 2022, 10000, 4) };

            _vehicleServiceMock
                .Setup(s => s.SearchAsync(It.IsAny<Expression<Func<Vehicle, bool>>>()))
                .ReturnsAsync(vehicles);

            var result = await _appService.SearchAsync(filter);

            Assert.Equal(vehicles.Count, result.Count());
        }

        [Fact]
        public async Task UpdateVehicleAsync_ShouldThrowValidationException_WhenInvalid()
        {
            var viewModel = new VehicleViewModel();
            _validatorMock.Setup(v => v.ValidateAsync(viewModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Field", "Error") }));

            await Assert.ThrowsAsync<ValidationException>(() => _appService.UpdateVehicleAsync(Guid.NewGuid(), viewModel));
        }

        [Fact]
        public async Task UpdateVehicleAsync_ShouldReturnUpdatedVehicle()
        {
            var viewModel = new VehicleViewModel
            {
                Manufacturer = "Honda",
                Model = "Civic",
                Year = 2023,
                StartingBid = 12000,
                Type = VehicleType.Sedan,
                NumberOfDoors = 4
            };

            _validatorMock.Setup(v => v.ValidateAsync(viewModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _vehicleServiceMock.Setup(s => s.UpdateAsync(It.IsAny<Vehicle>()))
                .ReturnsAsync((Vehicle v) => v);

            var result = await _appService.UpdateVehicleAsync(Guid.NewGuid(), viewModel);

            Assert.NotNull(result);
            Assert.Equal(viewModel.Manufacturer, result.Manufacturer);
        }
    }
}
