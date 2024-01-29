using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.BusBranding;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BusBrandingController : ControllerBase
    {
        private readonly IBusBrandingService _busBrandingService;
        public BusBrandingController(IBusBrandingService busBrandingService)
        {
            _busBrandingService = busBrandingService;
        }

        [HttpPost]
        [Route("AddBusBranding")]
        public async Task<IActionResult> AddBusBranding(BusBrandingRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _busBrandingService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Bus Branding created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetBusBrandingById/{id}")]
        public async Task<IActionResult> GetBusBrandingById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _busBrandingService.Get(id);
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
        [Route("GetAllBusBrandings")]
        public async Task<IActionResult> GetAllBusBrandings(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _busBrandingService.GetAllBusBrandings(cancellationToken);
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
        [Route("UnApprovedBrands")]
        public async Task<IActionResult> UnApprovedBrands(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _busBrandingService.UnApprovedBranding(cancellationToken);
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
        [Route("SearchBusBrandings/{keyword}")]
        public async Task<IActionResult> SearchBusBrandings(string? keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _busBrandingService.SearchBusBranding(keyword, cancellationToken);
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
        [Route("DeactivatedBranding")]
        public async Task<IActionResult> DeactivatedBranding(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _busBrandingService.GetInactiveBusBranding(cancellationToken);
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
        [Route("DeleteBranding/{id}")]
        public async Task<IActionResult> DeleteBranding(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Store item identity can't be null"); }
                await _busBrandingService.Delete(id);
                return Ok(new { Message = "Bus Branding Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateBusBranding")]
        public async Task<IActionResult> UpdateBusBranding(UpdateBusBrandingRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _busBrandingService.UpdateBusBranding(model);
                if (result == true)
                {
                    return Ok(new { Message = "bus branding updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating bus branding" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateBranding/{id}")]
        public async Task<IActionResult> ActivateBranding(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _busBrandingService.ActivateBusBranding(id);
                if (result == true)
                {
                    return Ok(new { Message = "bus branding activated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating bus branding" });
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
                var result = await _busBrandingService.Approve(id);
                if (result == true)
                {
                    return Ok(new { Message = "bus branding approved successfully" });
                }
                return BadRequest(new { Message = "internal error when approve bus branding" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
