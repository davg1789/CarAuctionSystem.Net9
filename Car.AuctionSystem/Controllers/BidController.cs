using Microsoft.AspNetCore.Mvc;
using Car.AuctionSystem.Application.Interface;
using Car.AuctionSystem.Application.ViewModel;

namespace Car.AuctionSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly IBidAppService _bidAppService;

        public BidController(IBidAppService bidAppService)
        {
            _bidAppService = bidAppService;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceBid([FromBody] BidCreateViewModel model)
        {
            var bid = await _bidAppService.PlaceBidAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = bid.Id }, bid);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var bid = await _bidAppService.GetByIdAsync(id);
            return bid == null ? NotFound() : Ok(bid);
        }

        [HttpGet("auction/{auctionId:guid}")]
        public async Task<IActionResult> GetByAuction(Guid auctionId)
        {
            
            var bids = await _bidAppService.GetByAuctionIdAsync(auctionId);
            return Ok(bids);
        }
    }
}
