using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;
using PTS_CORE.Domain.DataTransferObject.StaffAsset;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffAssetController : ControllerBase
    {
        private readonly IStaffAssetService _staffAssetService;
        public StaffAssetController(IStaffAssetService staffAssetService)
        {
           _staffAssetService = staffAssetService;
        }
        [HttpPost]
        [Route("AddStaffAsset")]
        public async Task<IActionResult> AddStaffAsset(CreateStaffAssetRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _staffAssetService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Staff asset created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetStaffAssetById/{id}")]
        public async Task<IActionResult> GetStaffAssetById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _staffAssetService.Get(id);
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
        [Route("GetAllStaffAssets")]
        public async Task<IActionResult> GetAllStaffAssets(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _staffAssetService.GetAllStaffAsset(cancellationToken);
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
        [Route("SearchStaffAssets/{keyword}")]
        public async Task<IActionResult> SearchStaffAssets(string? keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _staffAssetService.SearchStaffAsset(keyword, cancellationToken);
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
        [Route("DeactivatedStaffAssets")]
        public async Task<IActionResult> DeactivatedStaffAssets(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _staffAssetService.GetInactiveStaffAsset(cancellationToken);
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
        [Route("DeleteStaffAsset/{id}")]
        public async Task<IActionResult> DeleteStaffAsset(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Store item identity can't be null"); }
                await _staffAssetService.Delete(id);
                return Ok(new { Message = "Staff Asset Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("ActivateStaffAsset/{id}")]
        public async Task<IActionResult> ActivateStaffAsset(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _staffAssetService.ActivateStaffAsset(id);
                if (result == true)
                {
                    return Ok(new { Message = "staff asset activated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating staff asset" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
