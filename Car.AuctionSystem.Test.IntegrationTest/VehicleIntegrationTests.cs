using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Application.ViewModel;
using Car.AuctionSystem.Domain.Entities.Enum;
using Car.AuctionSystem.Test.IntegrationTest.Configuration;
using Car.AuctionSystem.Test.IntegrationTest.Response;
using Car.AuctionSystem.Test.IntegrationTest.Seed.IntegrationTests.VehicleTests.Seeders;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Car.AuctionSystem.Test.IntegrationTest
{
    public class VehicleIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public VehicleIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllVehicles_ShouldReturnSeededVehicles()
        {
            // Act
            var response = await _client.GetFromJsonAsync<List<VehicleListResponse>>("/api/Vehicle");

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetById_ShouldReturnVehicle_WhenExists()
        {
            var vehicle = await _client.GetFromJsonAsync<List<VehicleListResponse>>("/api/Vehicle");
            var id = vehicle!.First().Id;

            var result = await _client.GetFromJsonAsync<VehicleResponse>($"/api/Vehicle/{id}");
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetById_ShouldReturn404_WhenNotExists()
        {
            var response = await _client.GetAsync($"/api/Vehicle/{Guid.NewGuid()}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddVehicle_ShouldReturn201_WhenValid()
        {
            var model = new VehicleViewModel
            {
                Type = VehicleType.SUV,
                Manufacturer = "Test",
                Model = "Tucson",
                Year = 2023,
                StartingBid = 12000,
                NumberOfSeats = 5
            };

            var response = await _client.PostAsJsonAsync("/api/Vehicle", model);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task AddVehicle_ShouldReturn400_WhenInvalid()
        {
            var model = new VehicleViewModel();
            var response = await _client.PostAsJsonAsync("/api/Vehicle", model);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string[]>>();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateVehicle_ShouldReturn200_WhenValid()
        {
            var id = VehicleSeeder.SuvId;

            var model = new VehicleViewModel
            {                
                Type = VehicleType.Sedan,
                Manufacturer = "Toyota",
                Model = "Corolla",
                Year = 2022,
                StartingBid = 15500,
                NumberOfDoors = 4
            };

            var response = await _client.PutAsJsonAsync($"/api/Vehicle/{id}", model);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Search_ShouldReturnFilteredResult()
        {
            var response = await _client.GetFromJsonAsync<List<VehicleListResponse>>("/api/Vehicle/search?Year=2021");
            response.Should().NotBeEmpty();
            response!.All(x => x.Year == 2021).Should().BeTrue();
        }

        [Theory]
        [InlineData("{ \"type\": \"Plane\", \"manufacturer\": \"Ford\", \"model\": \"Focus\", \"year\": 2020, \"startingBid\": 10000, \"numberOfDoors\": 4 }", "Invalid vehicle type. Allowed values: Sedan, Hatchback, SUV, Truck.")]
        [InlineData("{ \"type\": \"Truck\", \"manufacturer\": \"Volvo\", \"model\": \"FH\", \"year\": 2021, \"startingBid\": 18000, \"loadCapacity\": \"abc\" }", "Load Capacity must be a number.")]
        [InlineData("{ \"type\": \"SUV\", \"manufacturer\": \"Hyundai\", \"model\": \"Creta\", \"year\": \"xyz\", \"startingBid\": 15000, \"numberOfSeats\": 5 }", "Year must be a number.")]
        [InlineData("{ \"type\": \"Sedan\", \"manufacturer\": \"Ford\", \"model\": 123, \"year\": 2022, \"startingBid\": 13000, \"numberOfDoors\": 4 }", "Model must be text (string type).")]
        [InlineData("{ \"type\": \"Sedan\", \"manufacturer\": 123, \"model\": \"Fiesta\", \"year\": 2022, \"startingBid\": 13000, \"numberOfDoors\": 4 }", "Manufacturer must be text (string type).")]
        [InlineData("{ \"type\": \"Sedan\", \"manufacturer\": \"Ford\", \"model\": \"Fiesta\", \"year\": 2022, \"startingBid\": \"abc\", \"numberOfDoors\": 4 }", "Starting bid must be a number.")]
        [InlineData("{ \"type\": \"Sedan\", \"manufacturer\": \"Ford\", \"model\": \"Fiesta\", \"year\": 2022, \"startingBid\": 13000, \"numberOfDoors\": \"abc\" }", "Number of doors must be a number.")]
        [InlineData("{ \"type\": \"SUV\", \"manufacturer\": \"Hyundai\", \"model\": \"Creta\", \"year\": 2022, \"startingBid\": 15000, \"numberOfSeats\": \"abc\" }", "Number of seats must be a number.")]
        public async Task Post_InvalidVehicle_ShouldReturnValidationError(string json, string expectedError)
        {
            // Arrange
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Vehicle", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var body = await response.Content.ReadFromJsonAsync<ValidationErrorResponse>();
            body.Should().NotBeNull();
            body!.Errors.Should().Contain(expectedError);
        }
    }
}
