using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Implementations;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Terminal;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Vehicle;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TerminalController : ControllerBase
    {
        private readonly ITerminalService _terminalService;
        public TerminalController(ITerminalService terminalService)
        {
            _terminalService = terminalService;
        }
        [HttpPost]
        [Route("AddTerminal")]
        public async Task<IActionResult> AddTerminal(CreateTerminalRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _terminalService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Terminal created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetTerminalById/{id}")]
        public async Task<IActionResult> GetTerminalById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _terminalService.Get(id);
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
        [Route("GetAllTerminals")]
        public async Task<IActionResult> GetAllTerminals(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _terminalService.GetAllTerminals(cancellationToken);
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
        [Route("DeactivatedTerminals")]
        public async Task<IActionResult> DeactivatedTerminals(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _terminalService.GetInactiveTerminals(cancellationToken);
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
        [Route("DeleteTerminal/{terminalId}")]
        public async Task<IActionResult> DeleteTerminal(string terminalId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(terminalId)) { return BadRequest("Terminal identity can't be null"); }
                await _terminalService.Delete(terminalId);
                return Ok(new { Message = "Terminal Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateTerminal")]
        public async Task<IActionResult> UpdateTerminal(UpdateTerminalRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _terminalService.UpdateTerminal(model);
                if (result == true)
                {
                    return Ok(new { Message = "terminal updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating terminal" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateTerminal/{terminalId}")]
        public async Task<IActionResult> ActivateTerminal(string terminalId)
        {
            try
            {
                if (terminalId == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _terminalService.ActivateTerminal(terminalId);
                if (result == true)
                {
                    return Ok(new { Message = "terminal activated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating terminal" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("SearchTerminal/{keyword}")]
        public async Task<IActionResult> SearchTerminal(string keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _terminalService.SearchTerminal(keyword, cancellationToken);
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
