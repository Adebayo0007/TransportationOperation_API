using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Terminal;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoreItemController : ControllerBase
    {
        private readonly IStoreItemService _storeItemService;
        public StoreItemController(IStoreItemService storeItemService)
        {
            _storeItemService = storeItemService;
        }
        [HttpPost]
        [Route("AddStoreItem")]
        public async Task<IActionResult> AddStoreItem(CreateStoreItemRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _storeItemService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Store item created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetStoreItemById/{id}")]
        public async Task<IActionResult> GetStoreItemById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _storeItemService.Get(id);
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
        [Route("GetAllStoreItems")]
        public async Task<IActionResult> GetAllStoreItems(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _storeItemService.GetAllStoreItems(cancellationToken);
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
        [Route("SearchStoreItems/{keyword}")]
        public async Task<IActionResult> SearchStoreItems(string? keyword,CancellationToken cancellationToken)
        {
            try
            {
                var result = await _storeItemService.SearchStoreItems(keyword,cancellationToken);
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
        [Route("DeactivatedStoreItems")]
        public async Task<IActionResult> DeactivatedStoreItems(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _storeItemService.GetInactiveStoreItem(cancellationToken);
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
        [Route("DeleteStoreItem/{storeItemId}")]
        public async Task<IActionResult> DeleteStoreItem(string storeItemId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(storeItemId)) { return BadRequest("Store item identity can't be null"); }
                await _storeItemService.Delete(storeItemId);
                return Ok(new { Message = "Store item Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateStoreItem")]
        public async Task<IActionResult> UpdateStoreItem(UpdateStoreItemRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _storeItemService.UpdateStoreItem(model);
                if (result == true)
                {
                    return Ok(new { Message = "store item updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating store item" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateStoreItem/{storeItemId}")]
        public async Task<IActionResult> ActivateStoreItem(string storeItemId)
        {
            try
            {
                if (storeItemId == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _storeItemService.ActivateStoreItem(storeItemId);
                if (result == true)
                {
                    return Ok(new { Message = "store item activated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating store item" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
