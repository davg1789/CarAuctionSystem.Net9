using Car.AuctionSystem.Application.Interface;
using Car.AuctionSystem.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Car.AuctionSystem.Api.Controllers
{        
        [Route("api/[controller]")]
        public class VehicleController : ControllerBase
        {
            private readonly IVehicleAppService _vehicleAppService;

            public VehicleController(IVehicleAppService vehicleAppService)
            {
                _vehicleAppService = vehicleAppService;
            }

        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleViewModel model)
        {
            var result = await _vehicleAppService.AddVehicleAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _vehicleAppService.GetByIdAsync(id);
            return result == null ? NotFound(new { message = "Vehicle not found." }) : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _vehicleAppService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] VehicleSearchViewModel filter)
        {
            var result = await _vehicleAppService.SearchAsync(filter);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] VehicleViewModel model)
        {
            var result = await _vehicleAppService.UpdateVehicleAsync(id, model);
            return Ok(result);
        }

    }
}
