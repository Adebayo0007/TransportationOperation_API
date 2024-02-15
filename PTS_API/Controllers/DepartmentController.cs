using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Implementations;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Depratment;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpPost]
        [Route("AddDepartment")]
        public async Task<IActionResult> AddDepartment(CreateDepartmentalSaleRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _departmentService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Department created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDepartmentById/{id}")]
        public async Task<IActionResult> GetDepartmentById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _departmentService.Get(id);
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
        [Route("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartments(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _departmentService.GetAllDepartments(cancellationToken);
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
        [Route("SearchDepartment/{keyword}")]
        public async Task<IActionResult> SearchDepartment(string? keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _departmentService.SearchDepartment(keyword, cancellationToken);
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
        [Route("DeactivatedDepartments")]
        public async Task<IActionResult> DeactivatedDepartments(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _departmentService.GetInactiveDepartments(cancellationToken);
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
        [Route("DeleteDepartment/{id}")]
        public async Task<IActionResult> DeleteDepartment(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Department identity can't be null"); }
                await _departmentService.Delete(id);
                return Ok(new { Message = "Department Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment(UpdateDepartmentRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _departmentService.UpdateDepartment(model);
                if (result == true)
                {
                    return Ok(new { Message = "Department updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating department" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateDepartment/{id}")]
        public async Task<IActionResult> ActivateDepartment(string id)
        {
            try
            {
                if (id == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _departmentService.ActivateDepartment(id);
                if (result == true)
                {
                    return Ok(new { Message = "Department activated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating department" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
