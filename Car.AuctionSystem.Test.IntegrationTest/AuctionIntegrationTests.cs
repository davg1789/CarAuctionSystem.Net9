using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Application.ViewModel;
using Car.AuctionSystem.Test.IntegrationTest.Configuration;
using Car.AuctionSystem.Test.IntegrationTest.Seed;
using Car.AuctionSystem.Test.IntegrationTest.Seed.IntegrationTests.VehicleTests.Seeders;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Car.AuctionSystem.Test.IntegrationTest
{
    public class AuctionIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuctionIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClientWithHttps();
        }

        [Fact]
        public async Task CreateAuction_ShouldReturnCreated()
        {
            var model = new AuctionCreateViewModel
            {
                VehicleId = VehicleSeeder.TruckId,
                StartTime = DateTime.UtcNow.AddMinutes(10)
            };

            var response = await _client.PostAsJsonAsync("/api/Auction", model);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var result = await response.Content.ReadFromJsonAsync<AuctionResponse>();
            result.Should().NotBeNull();
            result!.StartTime.Should().NotBeNull();
        }

        [Fact]
        public async Task StartAuction_ShouldReturnOk()
        {
            var auctionId = AuctionSeeder.AuctionId;

            var response = await _client.PutAsync($"/api/Auction/{auctionId}/start", null);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var auction = await response.Content.ReadFromJsonAsync<AuctionResponse>();
            auction.Should().NotBeNull();
            auction!.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task CloseAuction_ShouldReturnOk()
        {
            var auctionId = AuctionSeeder.AuctionIdActiveToClose; 

            var response = await _client.PutAsync($"/api/Auction/{auctionId}/close", null);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var auction = await response.Content.ReadFromJsonAsync<AuctionResponse>();
            auction.Should().NotBeNull();
            auction!.IsActive.Should().BeFalse();
            auction.EndTime.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAuctionById_ShouldReturnOk()
        {
            var auctionId = AuctionSeeder.AuctionId;

            var response = await _client.GetAsync($"/api/Auction/{auctionId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var auction = await response.Content.ReadFromJsonAsync<AuctionResponse>();
            auction.Should().NotBeNull();
            auction!.Id.Should().Be(auctionId);
        }

        [Fact]
        public async Task GetAllAuctions_ShouldReturnList()
        {
            var response = await _client.GetAsync("/api/Auction");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var auctions = await response.Content.ReadFromJsonAsync<List<AuctionResponse>>();
            auctions.Should().NotBeNull();
            auctions.Should().HaveCountGreaterThan(0);
        }
    }
}
