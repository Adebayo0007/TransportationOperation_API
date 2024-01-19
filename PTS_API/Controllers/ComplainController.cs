using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Complain;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Leave;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComplainController : ControllerBase
    {
        private readonly IComplainService _complainService;
        public ComplainController(IComplainService complainService)
        {
            _complainService = complainService;
        }
        [HttpPost]
        [Route("AddComplain")]
        public async Task<IActionResult> AddComplain(CreateComplainRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _complainService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! complain created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetComplainById/{id}")]
        public async Task<IActionResult> GetComplainById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _complainService.Get(id);
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
        [Route("GetAllComplains")]
        public async Task<IActionResult> GetAllComplains(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _complainService.GetAllComplains(cancellationToken);
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
        [Route("DeactivatedComplains")]
        public async Task<IActionResult> DeactivatedComplains(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _complainService.GetInactiveComplains(cancellationToken);
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
        [Route("DeleteComplain/{id}")]
        public async Task<IActionResult> DeleteComplain(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("complain identity can't be null"); }
                await _complainService.Delete(id);
                return Ok(new { Message = "complain Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateComplain")]
        public async Task<IActionResult> UpdateComplain(UpdateComplainRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _complainService.Update(model);
                if (result == true)
                {
                    return Ok(new { Message = "complain updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating leave" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateComplain/{id}")]
        public async Task<IActionResult> ActivateComplain(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _complainService.ActivateComplains(id);
                if (result == true)
                {
                    return Ok(new { Message = "complain activated successfully" });
                }
                return BadRequest(new { Message = "internal error when activating complain" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("SearchComplain/{keyword}")]
        public async Task<IActionResult> SearchComplain(string keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _complainService.SearchComplains(keyword, cancellationToken);
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
