using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Employee;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Vehicle;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        [HttpPost]
        [Route("AddVehicle")]
        public async Task<IActionResult> AddVehicle(CreateVehicleRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _vehicleService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Vehicle created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetVehicleById/{id}")]
        public async Task<IActionResult> GetVehicleById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _vehicleService.Get(id);
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
        [Route("GetAllVehicles")]
        public async Task<IActionResult> GetAllVehicles(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _vehicleService.GetAllVehicles(cancellationToken);
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
        [Route("GetAllTerminalVehicles/{terminalId}")]
        public async Task<IActionResult> GetAllTerminalVehicles(string terminalId,CancellationToken cancellationToken)
        {
            try
            {
                var result = await _vehicleService.GetTerminalVehicle(terminalId,cancellationToken);
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
        [Route("DeactivatedVehicles")]
        public async Task<IActionResult> DeactivatedVehicles(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _vehicleService.GetInactiveVehicle(cancellationToken);
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
        [Route("DeleteVehicle/{vehicleId}")]
        public async Task<IActionResult> DeleteVehicle(string vehicleId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vehicleId)) { return BadRequest("Vehicle identity can't be null"); }
                await _vehicleService.Delete(vehicleId);
                return Ok(new { Message = "Vehicle Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateVehicle")]
        public async Task<IActionResult> UpdateVehicle(UpdateVehicleRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _vehicleService.UpdateVehicle(model);
                if (result == true)
                {
                    return Ok(new { Message = "vehicle updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating vehicle" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateVehicle")]
        public async Task<IActionResult> ActivateVehicle(string vehicleId)
        {
            try
            {
                if (vehicleId == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _vehicleService.ActivateVehicle(vehicleId);
                if (result == true)
                {
                    return Ok(new { Message = "vehicle activated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating user" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
