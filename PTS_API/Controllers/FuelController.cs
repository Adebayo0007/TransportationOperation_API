using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Fuel;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelController : ControllerBase
    {
        private readonly IFuelService _fuelService;
        public FuelController(IFuelService fuelService)
        {
            _fuelService = fuelService;
        }
        [HttpPost]
        [Route("AddFuel")]
        public async Task<IActionResult> AddFuel(CreateFuelRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _fuelService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Fuel created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
