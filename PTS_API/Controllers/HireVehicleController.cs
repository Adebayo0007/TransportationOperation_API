using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.HireVehicle;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HireVehicleController : ControllerBase
    {
        private readonly IHireVehicleService _hireVehicleService;
        public HireVehicleController(IHireVehicleService hireVehicleService)
        {
            _hireVehicleService = hireVehicleService;
        }
        [HttpPost]
        [Route("AddHireVehicle")]
        public async Task<IActionResult> AddHireVehicle(CreateHireVehicleRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _hireVehicleService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Hire vehicle created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetHireVehicleById/{id}")]
        public async Task<IActionResult> GetHireVehicleById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _hireVehicleService.Get(id);
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
        [Route("GetAllHiredVehicles")]
        public async Task<IActionResult> GetAllHiredVehicles(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _hireVehicleService.GetAllHireVehicles(cancellationToken);
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
        [Route("UnApproveHiringVehicles")]
        public async Task<IActionResult> UnApproveHiringVehicles(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _hireVehicleService.UnApprovedHiring(cancellationToken);
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
        [Route("SearchHiredVehicle/{keyword}")]
        public async Task<IActionResult> SearchHiredVehicle(string? keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _hireVehicleService.SearchHireVehicles(keyword, cancellationToken);
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
        [Route("DeactivatedHiredVehicle")]
        public async Task<IActionResult> DeactivatedHiredVehicle(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _hireVehicleService.GetInactiveSHireVehicles(cancellationToken);
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
        [Route("DeleteHiredVehicle/{id}")]
        public async Task<IActionResult> DeleteHiredVehicle(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Hired Vehicle identity can't be null"); }
                await _hireVehicleService.Delete(id);
                return Ok(new { Message = "Hired vehicle Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateHiredVehicle")]
        public async Task<IActionResult> UpdateHiredVehicle(UpdateHireVehicleRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _hireVehicleService.UpdateHireVehicle(model);
                if (result == true)
                {
                    return Ok(new { Message = "hire vehicle updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating hire vehicle" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateHiredVehicle/{id}")]
        public async Task<IActionResult> ActivateHiredVehicle(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _hireVehicleService.ActivateHireVehicle(id);
                if (result == true)
                {
                    return Ok(new { Message = "hired vehicle activated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating hired vehicle" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch]
        [Route("Approve/{id}")]
        public async Task<IActionResult> Approve(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _hireVehicleService.Approve(id);
                if (result == true)
                {
                    return Ok(new { Message = "hire vehicle approved successfully" });
                }
                return BadRequest(new { Message = "internal error when approving hired vehicle" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch]
        [Route("ResolveByDepo/{id}")]
        public async Task<IActionResult> ResolveByDepo(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _hireVehicleService.ResolvedByDepo(id);
                if (result == true)
                {
                    return Ok(new { Message = "hire vehicle resolved successfully" });
                }
                return BadRequest(new { Message = "internal error when resolving hired vehicle" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
