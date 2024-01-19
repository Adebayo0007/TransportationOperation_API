using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Leave;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Terminal;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }
        [HttpPost]
        [Route("AddLeave")]
        public async Task<IActionResult> AddLeave(CreateLeaveRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _leaveService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Leave created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetLeaveById/{id}")]
        public async Task<IActionResult> GetLeaveById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _leaveService.Get(id);
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
        [Route("GetAllLeaves")]
        public async Task<IActionResult> GetAllLeaves(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _leaveService.GetAllLeaves(cancellationToken);
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
        [Route("DeactivatedLeaves")]
        public async Task<IActionResult> DeactivatedLeaves(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _leaveService.GetInactiveLeaves(cancellationToken);
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
        [Route("DeleteLeave/{id}")]
        public async Task<IActionResult> DeleteLeave(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("leaave identity can't be null"); }
                await _leaveService.Delete(id);
                return Ok(new { Message = "Leave Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPatch]
        [Route("IsOpened/{id}")]
        public async Task<IActionResult> IsOpened(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("leaave identity can't be null"); }
                await _leaveService.IsOpened(id);
                return Ok(new { Message = "Leave Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("IsGranted/{id}")]
        public async Task<IActionResult> IsGranted(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("leaave identity can't be null"); }
                await _leaveService.IsGranted(id);
                return Ok(new { Message = "Leave Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("IsDenied/{id}")]
        public async Task<IActionResult> IsDenied(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("leaave identity can't be null"); }
                await _leaveService.IsDenied(id);
                return Ok(new { Message = "Leave Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateLeave")]
        public async Task<IActionResult> UpdateLeave(UpdateLeaveRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _leaveService.Update(model);
                if (result == true)
                {
                    return Ok(new { Message = "leave updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating leave" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateLeave/{id}")]
        public async Task<IActionResult> ActivateLeave(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _leaveService.ActivateLeave(id);
                if (result == true)
                {
                    return Ok(new { Message = "levae activated successfully" });
                }
                return BadRequest(new { Message = "internal error when activating leave" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("SearchLeave/{keyword}")]
        public async Task<IActionResult> SearchLeave(string keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _leaveService.SearchLeaves(keyword, cancellationToken);
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
    }
}
