using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreAsset;
using PTS_CORE.Domain.DataTransferObject.StaffAsset;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoreAssetController : ControllerBase
    {
        private readonly IStoreAssetService _storeAssetService;
        public StoreAssetController(IStoreAssetService storeAssetService)
        {
           _storeAssetService = storeAssetService;
        }
        [HttpPost]
        [Route("AddStoreAsset")]
        public async Task<IActionResult> AddStoreAsset(CreateStoreAssetRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _storeAssetService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Store asset created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetStoreAssetById/{id}")]
        public async Task<IActionResult> GetStoreAssetById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _storeAssetService.Get(id);
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
        [Route("GetAllStoreAssets")]
        public async Task<IActionResult> GetAllStoreAssets(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _storeAssetService.GetAllStoreAsset(cancellationToken);
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
        [Route("SearchStoreAssets/{keyword}")]
        public async Task<IActionResult> SearchStoreAssets(string? keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _storeAssetService.SearchStoreAsset(keyword, cancellationToken);
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
