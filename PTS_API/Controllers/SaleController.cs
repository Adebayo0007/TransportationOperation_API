using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Sale;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }
        [HttpPost]
        [Route("AddSale")]
        public async Task<IActionResult> AddSale(CreateSaleRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _saleService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Sale created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetSaleById/{id}")]
        public async Task<IActionResult> GetSaleById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _saleService.Get(id);
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
        [Route("GetAllSales")]
        public async Task<IActionResult> GetAllSales(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _saleService.GetAllSales(cancellationToken);
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
        [Route("SearchSales/{keyword}")]
        public async Task<IActionResult> SearchSales(string? keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _saleService.SearchSales(keyword, cancellationToken);
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
        [Route("DeactivatedSales")]
        public async Task<IActionResult> DeactivatedSales(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _saleService.GetInactiveSales(cancellationToken);
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
        [Route("DeleteSale/{id}")]
        public async Task<IActionResult> DeleteSale(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Store item identity can't be null"); }
                await _saleService.Delete(id);
                return Ok(new { Message = "Sale Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateSale")]
        public async Task<IActionResult> UpdateSale(UpdateSaleRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _saleService.Update(model);
                if (result == true)
                {
                    return Ok(new { Message = "sale updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating sale" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateSale/{id}")]
        public async Task<IActionResult> ActivateSale(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _saleService.ActivateSale(id);
                if (result == true)
                {
                    return Ok(new { Message = "sale activated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating sale" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
