using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Implementations;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.DepartmentalExpenditure;
using PTS_CORE.Domain.DataTransferObject.RequestModel.DepartmentalSale;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentalSaleController : ControllerBase
    {
        private readonly IDepartmentalSaleService _departmentalSaleService;
        public DepartmentalSaleController(IDepartmentalSaleService departmentalSaleService)
        {
            _departmentalSaleService = departmentalSaleService;
        }

        [HttpPost]
        [Route("AddBudget")]
        public async Task<IActionResult> AddBudget(CreateDepartmentalSale model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _departmentalSaleService.Create(model);
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
                var result = await _departmentalSaleService.Get(id);
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
                var result = await _departmentalSaleService.GetAllDepartmentalSales(cancellationToken);
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
                var result = await _departmentalSaleService.SearchDepartmentalSales(keyword, cancellationToken);
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
                var result = await _departmentalSaleService.GetInactiveDepartmentalSales(cancellationToken);
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
                await _departmentalSaleService.Delete(id);
                return Ok(new { Message = "Budget Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateBudget")]
        public async Task<IActionResult> UpdateBudget(UpdateDepartmentalSaleRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _departmentalSaleService.UpdateDepartmentalSale(model);
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
                var result = await _departmentalSaleService.ActivateDepartmentalSale(id);
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
