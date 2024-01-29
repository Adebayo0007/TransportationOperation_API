using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.BudgetTraching;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BudgetTrackingController : ControllerBase
    {
        private readonly IBudgetTrackingService _budgetTrackingervice;
        public BudgetTrackingController(IBudgetTrackingService budgetTrackingervice)
        {
           _budgetTrackingervice = budgetTrackingervice;
        }
        [HttpPost]
        [Route("AddBudget")]
        public async Task<IActionResult> AddBudget(CreateBudgetTrackingRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _budgetTrackingervice.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Budget created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetBudgetById/{id}")]
        public async Task<IActionResult> GetBudgetById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _budgetTrackingervice.Get(id);
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
        [Route("GetAllBudgets")]
        public async Task<IActionResult> GetAllBudgets(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _budgetTrackingervice.GetAllBudgetTrackings(cancellationToken);
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

        [HttpPost]
        [Route("SearchBudgets")]
        public async Task<IActionResult> SearchBudgets(SearchBudgetRequestModel model, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _budgetTrackingervice.SearchBudgetTrackings(model.Start,model.End,cancellationToken);
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
        [Route("DeactivatedBudgets")]
        public async Task<IActionResult> DeactivatedBudgets(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _budgetTrackingervice.GetInactiveBudgetTrackings(cancellationToken);
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
        [Route("DeleteBudget/{id}")]
        public async Task<IActionResult> DeleteBudget(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Budget identity can't be null"); }
                await _budgetTrackingervice.Delete(id);
                return Ok(new { Message = "Store item Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateBudget")]
        public async Task<IActionResult> UpdateBudget(UpdateBudgetTrackingRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _budgetTrackingervice.UpdateBudgetTracking(model);
                if (result == true)
                {
                    return Ok(new { Message = "budget updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating budget" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateBudget/{id}")]
        public async Task<IActionResult> ActivateBudget(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _budgetTrackingervice.ActivateBudgetTracking(id);
                if (result == true)
                {
                    return Ok(new { Message = "budget activated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating budget" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
