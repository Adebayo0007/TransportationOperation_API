using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Implementations;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Fuel;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Leave;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet]
        [Route("GetFuelById/{id}")]
        public async Task<IActionResult> GetFuelById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _fuelService.Get(id);
                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        [Route("GetAllFuels")]
        public async Task<IActionResult> GetAllFuels(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _fuelService.GetAllFuels(cancellationToken);
                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("DeactivatedFuels")]
        public async Task<IActionResult> DeactivatedFuels(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _fuelService.GetInactiveFuels(cancellationToken);
                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteFuel/{id}")]
        public async Task<IActionResult> DeleteFuel(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("fuel identity can't be null"); }
                await _fuelService.Delete(id);
                return Ok(new { Message = "Fuel Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPatch]
        [Route("ActivateFuel/{id}")]
        public async Task<IActionResult> ActivateFuel(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _fuelService.ActivateFuel(id);
                if (result == true)
                {
                    return Ok(new { Message = "fuel activated successfully" });
                }
                return BadRequest(new { Message = "internal error when activating fuel" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("SearchFuel/{keyword}")]
        public async Task<IActionResult> SearchFuel(string keyword, CancellationToken? cancellationToken)
        {
            try
            {
                var result = await _fuelService.SearchFuels(Convert.ToDateTime(keyword), cancellationToken.Value);
                if (result.IsSuccess == true)
                {
                    return Ok(result);
                }
                return BadRequest(new { Message = "internal error, please try again later..." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch]
        [Route("UpdateFuel")]
        public async Task<IActionResult> UpdateFuel(UpdateFuelRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _fuelService.UpdateFuel(model);
                if (result == true)
                {
                    return Ok(new { Message = "fuel updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating fuel" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
