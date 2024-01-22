using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTS_BUSINESS.Services.Implementations;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Account;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Employee;

namespace PTS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee(CreateEmployeeRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _employeeService.Create(model);
                if (result == true)
                {
                    return Ok(new { Message = "Congratulations...! Employee created successfully" });
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetEmployeeById/{id}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id)) { return BadRequest("Id can't be null"); }
                var result = await _employeeService.Get(id);
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
        [Route("GetEmployeeByEmail/{mail}")]
        public async Task<IActionResult> GetEmployeeByEmail(string mail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mail)) { return BadRequest("Mail can't be null"); }
                var result = await _employeeService.GetByEmail(mail);
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
        [Route("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _employeeService.GetAllEmployees(cancellationToken);
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
        [Route("EmployeeBirthdayNotification")]
        public async Task<IActionResult> EmployeeBirthdayNotification(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _employeeService.EmployeeBirthdayNotification(cancellationToken);
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
        [Route("SearchEmployee/{keyword}")]
        public async Task<IActionResult> SearchEmployee(string keyword, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _employeeService.SearchEmployee(keyword,cancellationToken);
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
        [Route("DeactivatedEmployees")]
        public async Task<IActionResult> DeactivatedEmployees(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _employeeService.GetInactiveEmployees(cancellationToken);
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
        [Route("DeactivateEmployee/{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(string employeeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeId)) { return BadRequest("Employee identity can't be null"); }
                 await _employeeService.Delete(employeeId);
                    return Ok(new { Message = "Employee Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _employeeService.UpdateEmployeeAccount(model);
                if (result == true)
                {
                    return Ok(new { Message = "employee updated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating user" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("ActivateEmployee/{employeeId}")]
        public async Task<IActionResult> ActivateEmployee(string employeeId)
        {
            try
            {
                if (employeeId == null) { return BadRequest("Your credentials are not valid, please check your inputs"); }
                var result = await _employeeService.ActivateEmployee(employeeId);
                if (result == true)
                {
                    return Ok(new { Message = "employee activated successfully" });
                }
                return BadRequest(new { Message = "internal error when updating user" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
