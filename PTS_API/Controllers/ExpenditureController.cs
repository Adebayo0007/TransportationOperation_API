using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Expenditure;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Leave;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenditureController : ControllerBase
    {
        private readonly IExpenditureService _expenditureService;
        public ExpenditureController(IExpenditureService expenditureService)
        {
            _expenditureService = expenditureService;
        }
        [HttpPost]
        [Route("AddExpenditure")]
        public async Task<IActionResult> AddExpenditure(CreateExpenditureRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _expenditureService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Expenditure created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetExpenditureById/{id}")]
        public async Task<IActionResult> GetExpenditureById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _expenditureService.Get(id);
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
        [Route("GetAllExpenditure")]
        public async Task<IActionResult> GetAllExpenditure(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _expenditureService.GetAllExpenditures(cancellationToken);
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
        [Route("Expenditures")]
        public async Task<IActionResult> Expenditures(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _expenditureService.Expenditures(cancellationToken);
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
        [Route("DeactivatedExpenditure")]
        public async Task<IActionResult> DeactivatedExpenditure(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _expenditureService.GetInactiveExpenditures(cancellationToken);
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
        [Route("ResolvedRequest")]
        public async Task<IActionResult> ResolvedRequest(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _expenditureService.ResolvedRequest(cancellationToken);
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
        [Route("DeleteExpenditure/{id}")]
        public async Task<IActionResult> DeleteExpenditure(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("expenditure identity can't be null"); }
                await _expenditureService.Delete(id);
                return Ok(new { Message = "Expenditure Deleted successfully" });
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
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("expenditure identity can't be null"); }
                await _expenditureService.IsGranted(id);
                return Ok(new { Message = "Expenditure Deleted successfully" });
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
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("expenditure identity can't be null"); }
                await _expenditureService.IsDenied(id);
                return Ok(new { Message = "Expenditure Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("Resolve/{id}")]
        public async Task<IActionResult> Resolve(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("expenditure identity can't be null"); }
                await _expenditureService.Resolve(id);
                return Ok(new { Message = "Expenditure resolved successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("Verified/{id}")]
        public async Task<IActionResult> Verified(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("expenditure identity can't be null"); }
                await _expenditureService.IsVerified(id);
                return Ok(new { Message = "Expenditure Verified successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpPatch]
        [Route("UpdateExpenditure")]
        public async Task<IActionResult> UpdateExpenditure(UpdateExpenditureRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _expenditureService.Update(model);
                if (result == true)
                {
                    return Ok(new { Message = "expenditure updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating expenditure" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateExpenditure/{id}")]
        public async Task<IActionResult> ActivateExpenditure(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _expenditureService.ActivateExpenditure(id);
                if (result == true)
                {
                    return Ok(new { Message = "expenditure activated successfully" });
                }
                return BadRequest(new { Message = "internal error when activating expenditure" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("SearchExpenditure/{keyword}")]
        public async Task<IActionResult> SearchExpenditure(string keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _expenditureService.SearchExpenditures(keyword, cancellationToken);
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
