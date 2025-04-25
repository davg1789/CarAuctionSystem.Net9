using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Application.ViewModel;
using Car.AuctionSystem.Test.IntegrationTest.Configuration;
using Car.AuctionSystem.Test.IntegrationTest.Seed;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Car.AuctionSystem.Test.IntegrationTest
{
    public class BidIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BidIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClientWithHttps();
        }

        [Fact]
        public async Task PlaceBid_ShouldReturnCreated()
        {
            var model = new BidCreateViewModel
            {
                AuctionId = AuctionSeeder.AuctionIdActive,
                Amount = 60500,
                Bidder = "Daniel"
            };

            var response = await _client.PostAsJsonAsync("/api/Bid", model);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var bid = await response.Content.ReadFromJsonAsync<BidResponse>();
            bid.Should().NotBeNull();
            bid!.Bidder.Should().Be("Daniel");
        }

        [Fact]
        public async Task GetBidById_ShouldReturnOk()
        {
            var bidId = Guid.Parse("189c078e-a08f-45cf-ae14-308569216836");

            var response = await _client.GetAsync($"/api/Bid/{bidId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var bid = await response.Content.ReadFromJsonAsync<BidResponse>();
            bid.Should().NotBeNull();
            bid!.Id.Should().Be(bidId);
        }

        [Fact]
        public async Task GetByAuction_ShouldReturnList()
        {
            var auctionId = AuctionSeeder.AuctionId;

            var response = await _client.GetAsync($"/api/Bid/auction/{auctionId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var bids = await response.Content.ReadFromJsonAsync<List<BidResponse>>();
            bids.Should().NotBeNull();
            bids.Should().Contain(b => b.AuctionId == auctionId);
        }
    }
}
