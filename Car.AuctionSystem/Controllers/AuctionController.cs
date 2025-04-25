using Car.AuctionSystem.Application.Interface;
using Car.AuctionSystem.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Car.AuctionSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionAppService _auctionAppService;

        public AuctionController(IAuctionAppService auctionAppService)
        {
            _auctionAppService = auctionAppService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuctionCreateViewModel model)
        {
            var auction = await _auctionAppService.CreateAuctionAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = auction.Id }, auction);
        }

        [HttpPut("{id}/start")]
        public async Task<IActionResult> Start(Guid id)
        {
            var auction = await _auctionAppService.StartAuctionAsync(id);
            return Ok(auction);
        }

        [HttpPut("{id}/close")]
        public async Task<IActionResult> Close(Guid id)
        {
            var auction = await _auctionAppService.CloseAuctionAsync(id);
            return Ok(auction);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var auction = await _auctionAppService.GetByIdAsync(id);
            return auction == null ? NotFound() : Ok(auction);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var auctions = await _auctionAppService.GetAllAsync();
            return Ok(auctions);
        }
    }
}
