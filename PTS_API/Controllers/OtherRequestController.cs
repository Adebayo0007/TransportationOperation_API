using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Implementations;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.OtherRequest;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OtherRequestController : ControllerBase
    {
        private readonly IOtherRequestService _otherRequestService;
        public OtherRequestController(IOtherRequestService otherRequestService)
        {
            _otherRequestService = otherRequestService;
        }
        [HttpPost]
        [Route("AddOtherRequest")]
        public async Task<IActionResult> AddOtherRequest(CreateOtherRequestRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _otherRequestService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Other request created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetOtherRequestById/{id}")]
        public async Task<IActionResult> GetOtherRequestById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _otherRequestService.Get(id);
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
        [Route("GetAllOtherRequests")]
        public async Task<IActionResult> GetAllOtherRequests(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _otherRequestService.GetAllOtherRequests(cancellationToken);
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
        [Route("SearchOtherRequests/{keyword}")]
        public async Task<IActionResult> SearchOtherRequests(string? keyword, CancellationToken cancellationToken)
        {
             try
             {
                 var result = await _otherRequestService.SearchOtherRequests(keyword, cancellationToken);
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
        [Route("MyRequest/{keyword}")]
        public async Task<IActionResult> MyRequest(string? keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _otherRequestService.MyRequest(keyword, cancellationToken);
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
        [Route("DeactivatedOtherRequests")]
        public async Task<IActionResult> DeactivatedOtherRequests(CancellationToken cancellationToken = default)
        {
             try
             {
                 var result = await _otherRequestService.GetInactiveOtherRequests(cancellationToken);
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
        [Route("DeleteOtherRequest/{id}")]
        public async Task<IActionResult> DeleteOtherRequest(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("other request identity can't be null"); }
                await _otherRequestService.Delete(id);
                return Ok(new { Message = "Other request Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateOtherRequest")]
        public async Task<IActionResult> UpdateOtherRequest(UpdateOtheRequestRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _otherRequestService.Update(model);
                if (result == true)
                {
                    return Ok(new { Message = "store item updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating other request" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateOtherRequest/{id}")]
        public async Task<IActionResult> ActivateOtherRequest(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _otherRequestService.ActivateOtherRequests(id);
                if (result == true)
                {
                    return Ok(new { Message = "other request activated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating other request" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch]
        [Route("ChairmanApproving/{id}")]
        public async Task<IActionResult> ChairmanApproving(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _otherRequestService.ChairmanApproving(id);
                if (result == true)
                {
                    return Ok(new { Message = "other request activated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating other request" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
