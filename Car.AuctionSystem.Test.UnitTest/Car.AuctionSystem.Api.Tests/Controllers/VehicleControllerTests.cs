using Car.AuctionSystem.Api.Controllers;
using Car.AuctionSystem.Application.Interface;
using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Application.ViewModel;
using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Entities.Enum;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Car.AuctionSystem.Test.UnitTest.Controllers
{
    public class VehicleControllerTests
    {
        private readonly Mock<IVehicleAppService> _serviceMock;
        private readonly VehicleController _controller;

        public VehicleControllerTests()
        {
            _serviceMock = new Mock<IVehicleAppService>();
            _controller = new VehicleController(_serviceMock.Object);
        }

        [Fact]
        public async Task AddVehicle_ShouldReturnCreatedResult()
        {
            var viewModel = new VehicleViewModel { Manufacturer = "Test", Model = "X1", Year = 2024, StartingBid = 10000, Type = VehicleType.Sedan };
            var createdVehicle = new SedanResponse
            {
                Id = Guid.NewGuid(),
                Manufacturer = "Test",
                Model = "X1",
                Year = 2024,
                StartingBid = 10000,
                NumberOfDoors = 4
            };

            _serviceMock.Setup(s => s.AddVehicleAsync(viewModel)).ReturnsAsync(createdVehicle);

            var result = await _controller.AddVehicle(viewModel);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(createdVehicle, createdResult.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenVehicleExists()
        {
            var vehicle = new SedanResponse
            {
                Id = Guid.NewGuid(),
                Manufacturer = "Test",
                Model = "X1",
                Year = 2024,
                StartingBid = 10000,
                NumberOfDoors = 4
            };
            _serviceMock.Setup(s => s.GetByIdAsync(vehicle.Id)).ReturnsAsync(vehicle);

            var result = await _controller.GetById(vehicle.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(vehicle, okResult.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenVehicleDoesNotExist()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((VehicleResponse)null);

            var result = await _controller.GetById(Guid.NewGuid());

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var json = JsonConvert.SerializeObject(notFoundResult.Value);
            var jObject = JObject.Parse(json);

            Assert.Equal("Vehicle not found.", jObject["message"]?.ToString());
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfVehicles()
        {
            var vehicles = new List<VehicleListResponse> { new SedanListResponse
                {
                    Id = Guid.NewGuid(),
                    Manufacturer = "Test",
                    Model = "X1",
                    Year = 2024,
                    StartingBid = 10000,
                    NumberOfDoors = 4
                } }; 
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(vehicles);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(vehicles, okResult.Value);
        }

        [Fact]
        public async Task Search_ShouldReturnFilteredVehicles()
        {
            var filter = new VehicleSearchViewModel { Manufacturer = "Test" };
            var vehicles = new List<VehicleListResponse> { new SedanListResponse
                {
                    Id = Guid.NewGuid(),
                    Manufacturer = "Test",
                    Model = "X1",
                    Year = 2024,
                    StartingBid = 10000,
                    NumberOfDoors = 4
                } };

            _serviceMock.Setup(s => s.SearchAsync(filter)).ReturnsAsync(vehicles);

            var result = await _controller.Search(filter);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(vehicles, okResult.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnUpdatedVehicle()
        {
            var viewModel = new VehicleViewModel { Manufacturer = "Updated", Model = "Y2", Year = 2025, StartingBid = 12000, Type = VehicleType.Sedan };
            var updatedVehicle = new SedanResponse
            {
                Id = Guid.NewGuid(),
                Manufacturer = "Test",
                Model = "X1",
                Year = 2024,
                StartingBid = 10000,
                NumberOfDoors = 4
            };
            var vehicleId = Guid.NewGuid();

            _serviceMock.Setup(s => s.UpdateVehicleAsync(vehicleId, viewModel)).ReturnsAsync(updatedVehicle);

            var result = await _controller.Update(vehicleId, viewModel);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedVehicle, okResult.Value);
        }
    }
}
