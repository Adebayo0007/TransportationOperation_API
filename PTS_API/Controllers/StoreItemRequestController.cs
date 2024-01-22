using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS_API.Authentication;
using PTS_API.GateWay.Email;
using PTS_BUSINESS.Common;
using PTS_BUSINESS.Services.Implementations;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Terminal;
using PTS_CORE.Domain.DataTransferObject.StoreItemRequest;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoreItemRequestController : ControllerBase
    {
        private readonly IStoreItemRequestService _storeItemRequestService;


        public StoreItemRequestController(IStoreItemRequestService storeItemRequestService)
        {
           _storeItemRequestService = storeItemRequestService;
        }

        [HttpPost]
        [Route("AddStoreItemRequest")]
        public async Task<IActionResult> Add(CreateStoreItemRequestRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _storeItemRequestService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Store Item Request created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetStoreItemRequestById/{id}")]
        public async Task<IActionResult> GetStoreItemRequestById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _storeItemRequestService.Get(id);
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
        [Route("GetAllStoreItemRequests")]
        public async Task<IActionResult> GetAllStoreItemRequests(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _storeItemRequestService.GetAllStoreItemRequests(cancellationToken);
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
        [Route("DeactivatedStoreItemRequests")]
        public async Task<IActionResult> DeactivatedStoreItemRequests(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _storeItemRequestService.GetInactiveStoreItemRequest(cancellationToken);
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
        [Route("ResolvedStoreItemRequest")]
        public async Task<IActionResult> ResolvedStoreItemRequest(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _storeItemRequestService.ResolvedStoreItemRequest(cancellationToken);
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
        [Route("DeleteStoreItemRequest/{id}")]
        public async Task<IActionResult> DeleteStoreItemRequest(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Store item request identity can't be null"); }
                await _storeItemRequestService.Delete(id);
                return Ok(new { Message = "Store Item Request Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateStoreItemRequest")]
        public async Task<IActionResult> UpdateStoreItemRequest(UpdateStoreItemRequestRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _storeItemRequestService.UpdateStoreItemRequest(model);
                if (result == true)
                {
                    return Ok(new { Message = "store item request updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating terminal" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateStoreItemRequest/{id}")]
        public async Task<IActionResult> ActivateStoreItemRequest(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _storeItemRequestService.ActivateStoreItemRequest(id);
                if (result == true)
                {
                    return Ok(new { Message = "store item request activated successfully" });
                }
                return BadRequest(new { Message = "internal error when activating store item request" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ApproveRequest/{id}")]
        [Authorize(Roles = $"{RoleConstant.Chairman}, {RoleConstant.Administrator}")]
        public async Task<IActionResult> ApproveRequest(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _storeItemRequestService.ApproveRequest(id);
                if (result == true)
                {
                    return Ok(new { Message = "store item request activated successfully" });
                }
                return BadRequest(new { Message = "internal error when activating store item request" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("SearchStoreItemRequest/{keyword}")]
        public async Task<IActionResult> SearchStoreItemRequest(string keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _storeItemRequestService.SearchStoreItemRequests(keyword, cancellationToken);
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
        [Route("MystoreItemRequest")]
        public async Task<IActionResult> MystoreItemRequest(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _storeItemRequestService.MystoreItemRequest(cancellationToken);
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
