using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Interfaces.Repository;
using Car.AuctionSystem.Domain.Services;
using Moq;
using System.Linq.Expressions;

namespace Car.AuctionSystem.Test.UnitTest.Domain
{
    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _repositoryMock;
        private readonly VehicleService _service;

        public VehicleServiceTests()
        {
            _repositoryMock = new Mock<IVehicleRepository>();
            _service = new VehicleService(_repositoryMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenVehicleExists()
        {
            var vehicle = new Sedan("Toyota", "Corolla", 2023, 10000, 4);
            _repositoryMock.Setup(r => r.GetByIdAsync(vehicle.Id)).ReturnsAsync(vehicle);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddAsync(vehicle));
        }

        [Fact]
        public async Task AddAsync_ShouldReturnVehicle_WhenNotExists()
        {
            var vehicle = new Sedan("Honda", "Civic", 2022, 12000, 4);
            _repositoryMock.Setup(r => r.GetByIdAsync(vehicle.Id)).ReturnsAsync((Vehicle)null);
            _repositoryMock.Setup(r => r.AddAsync(vehicle)).ReturnsAsync(vehicle);

            var result = await _service.AddAsync(vehicle);

            Assert.Equal(vehicle, result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnVehicle()
        {
            var vehicle = new Sedan("Nissan", "Sentra", 2021, 9500, 4);
            _repositoryMock.Setup(r => r.GetByIdAsync(vehicle.Id)).ReturnsAsync(vehicle);

            var result = await _service.GetByIdAsync(vehicle.Id);

            Assert.Equal(vehicle, result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnVehicles()
        {
            var vehicles = new List<Vehicle> { new Sedan("Ford", "Focus", 2020, 8000, 4) };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(vehicles);

            var result = await _service.GetAllAsync();

            Assert.Equal(vehicles, result);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnFilteredVehicles()
        {
            var predicate = It.IsAny<Expression<Func<Vehicle, bool>>>();
            var vehicles = new List<Vehicle> { new Sedan("Chevrolet", "Onix", 2019, 7500, 4) };

            _repositoryMock.Setup(r => r.SearchAsync(predicate)).ReturnsAsync(vehicles);

            var result = await _service.SearchAsync(predicate);

            Assert.Equal(vehicles, result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedVehicle()
        {
            var vehicle = new Sedan("Renault", "Logan", 2023, 8500, 4);
            _repositoryMock.Setup(r => r.UpdateAsync(vehicle)).ReturnsAsync(vehicle);

            var result = await _service.UpdateAsync(vehicle);

            Assert.Equal(vehicle, result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepository()
        {
            var id = Guid.NewGuid();
            _repositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask).Verifiable();

            await _service.DeleteAsync(id);

            _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }
    }
}
