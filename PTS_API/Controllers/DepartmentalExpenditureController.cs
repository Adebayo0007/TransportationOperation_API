using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Implementations;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.BudgetTraching;
using PTS_CORE.Domain.DataTransferObject.RequestModel.DepartmentalExpenditure;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentalExpenditureController : ControllerBase
    {
        private readonly IDepartmentalExpenditureBudgetService _departmentalExpenditureBudgetService;
        public DepartmentalExpenditureController(IDepartmentalExpenditureBudgetService departmentalExpenditureBudgetService)
        {
            _departmentalExpenditureBudgetService = departmentalExpenditureBudgetService;
        }

        [HttpPost]
        [Route("AddBudget")]
        public async Task<IActionResult> AddBudget(CreateDepartmentalExpenditure model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _departmentalExpenditureBudgetService.Create(model);
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
                var result = await _departmentalExpenditureBudgetService.Get(id);
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
                var result = await _departmentalExpenditureBudgetService.GetAllDepartmentalExpenditure(cancellationToken);
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
        [Route("SearchBudgets/{keyword}")]
        public async Task<IActionResult> SearchBudgets(string? keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _departmentalExpenditureBudgetService.SearchDepartmentalExpenditures(keyword, cancellationToken);
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
                var result = await _departmentalExpenditureBudgetService.GetInactiveDepartmentalExpenditure(cancellationToken);
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
                await _departmentalExpenditureBudgetService.Delete(id);
                return Ok(new { Message = "Budget Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateBudget")]
        public async Task<IActionResult> UpdateBudget(UpdateDepartmentalExpenditureRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _departmentalExpenditureBudgetService.UpdateDepartmentalExpenditure(model);
                if (result == true)
                {
                    return Ok(new { Message = "Budget updated successfully" });
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
                var result = await _departmentalExpenditureBudgetService.ActivateDepartmentalExpenditure(id);
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
