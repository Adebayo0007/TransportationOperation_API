using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Employee;
using PTS_CORE.Domain.Entities;
using PTS_DATA.Repository.Interfaces;
using System.Security.Claims;

namespace PTS_BUSINESS.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeService(IEmployeeRepository employeeRepository, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager,
             IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Create(CreateEmployeeRequestModel model)
        {
            if (model != null)
            {
                if (await _userManager.FindByEmailAsync(model.Email.Trim()) != null) return false;
                var role = await _roleManager.FindByNameAsync(model.RoleName.Trim());
                var user = new ApplicationUser
                {
                    UserName = model.Email.Trim(),
                    Email = model.Email.Trim(),
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    ApplicationRoleId = role.Id.Trim(),
                    RoleName = model.RoleName.Trim(),
                    DateCreated = DateTime.Now,
                    PhoneNumber = model.Phonenumber.Trim(),
                    PhoneNumberConfirmed = !string.IsNullOrWhiteSpace(model.Phonenumber.Trim()) ? true : false,
                    EmailConfirmed = !string.IsNullOrWhiteSpace(model.Email.Trim()) ? true : false,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender.Trim(),
                    TerminalId = model.TerminalId.Trim() ?? string.Empty,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName.Trim());
                    var employee = new Employee
                    {
                        ApplicatioUserId = user.Id,
                        ApplicationUser = user,
                        StaffIdentityCardNumber = model.StaffIdentityCardNumber.Trim() ?? string.Empty,
                        TerminalId = model.TerminalId.Trim() ?? string.Empty,
                        CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                        CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null
                    };
                   var response = await _employeeRepository.CreateAsync(employee);
                    if (response == false) return false;
                    else return true;
                }
                else
                {
                    // Handle errors if user creation fails
                    Console.WriteLine($"There is error creating user {result.Errors}");
                    return false;
                }
            }
            else
                return false;
        }

        public async Task<bool> ActivateEmployee(string employeeId)
        {

            if (employeeId != null)
            {
                try
                {
                    var employee = await _employeeRepository.GetModelByIdAsync(employeeId.Trim());

                    if (employee == null)
                        return false;
                    employee.IsDeleted = false;
                    employee.ApplicationUser.IsDeleted = false;
                    employee.ApplicationUser.IsModified = true;
                    employee.ApplicationUser.LastModified = DateTime.Now;
                    employee.ApplicationUser.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    employee.ApplicationUser.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                    employee.IsModified = true;
                    employee.LastModified = DateTime.Now;
                    employee.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    employee.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _employeeRepository.UpdateAsync(employee);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task Delete(string id)
        {
            if(id != null)
            {
                var employee = await _employeeRepository.GetModelByIdAsync(id.Trim());
                employee.IsDeleted = true;
                employee.ApplicationUser.IsDeleted = true;  
                await _employeeRepository.DeleteAsync();
            }
        }

        public async Task<BaseResponse<IEnumerable<EmployeeResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var employee = await _employeeRepository.GetByIdAsync(id.Trim());
                if(employee == null)return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"employee having  id {id} is not found"
                };
                else
                return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                {
                    IsSuccess = true,
                    Message = $"employeer having id {id} retrieved successfully",
                    Data = employee.Select(x => ReturnEmployeeResponseModel(x)).ToList()
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<EmployeeResponseModel>>> GetAllEmployees(CancellationToken cancellationToken = default)
        {
            try
            {
                var employees = await _employeeRepository.GetAllAsync(cancellationToken);

                if (employees != null)
                {
                    return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Employees retrieved successfully",
                        Data = employees.Select(x => ReturnEmployeeResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Employees failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving employees: {ex.Message}",
                };
            }
        }


        public async Task<BaseResponse<IEnumerable<EmployeeResponseModel>>> GetInactiveEmployees(CancellationToken cancellationToken = default)
        {
            try
            {
                var employees = await _employeeRepository.InactiveEmployees(cancellationToken);
                if (employees != null)
                    return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"employees retrieved successfully",
                        Data = employees.Select(x => ReturnEmployeeResponseModel(x)).ToList()
                    };
                else return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"employess failed to retrieved successfully",
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateEmployeeAccount(UpdateEmployeeRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var employee = await _employeeRepository.GetModelByIdAsync(updateModel.EmployeeId.Trim());
                    ApplicationRole role = null;
                    if(updateModel.RoleName != "" && updateModel.RoleName != " " && updateModel.RoleName != null)
                    {
                       role = await _roleManager.FindByNameAsync(updateModel.RoleName.Trim());
                    }

                    if (employee == null)
                        return false;

                    // Update user properties based on your model
                    employee.ApplicationUser.FirstName = !string.IsNullOrWhiteSpace(updateModel.FirstName.Trim()) ? updateModel.FirstName.Trim() : employee.ApplicationUser.FirstName;
                    employee.ApplicationUser.LastName = !string.IsNullOrWhiteSpace(updateModel.LastName.Trim()) ? updateModel.LastName.Trim() : employee.ApplicationUser.LastName;
                    employee.ApplicationUser.Email = !string.IsNullOrWhiteSpace(updateModel.Email.Trim()) ? updateModel.Email.Trim() : employee.ApplicationUser.Email;
                    employee.ApplicationUser.UserName = !string.IsNullOrWhiteSpace(updateModel.Email.Trim()) ? updateModel.Email.Trim() : employee.ApplicationUser.Email;
                    employee.ApplicationUser.RoleName = role != null ? role.Name : employee.ApplicationUser.RoleName;
                    employee.ApplicationUser.ApplicationRoleId = role != null ? role.Id : employee.ApplicationUser.ApplicationRoleId;
                    employee.ApplicationUser.PhoneNumber = !string.IsNullOrWhiteSpace(updateModel.Phonenumber.Trim()) ? updateModel.Phonenumber.Trim() : employee.ApplicationUser.PhoneNumber;
                    employee.TerminalId = updateModel.TerminalId != null ? updateModel.TerminalId.Trim() : employee.TerminalId;
                    employee.StaffIdentityCardNumber = !string.IsNullOrWhiteSpace(updateModel.StaffIdentityCardNumber.Trim()) ? updateModel.StaffIdentityCardNumber.Trim() : employee.StaffIdentityCardNumber;
                    employee.ApplicationUser.IsModified = true;
                    employee.ApplicationUser.LastModified = DateTime.Now;
                    employee.ApplicationUser.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    employee.ApplicationUser.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                    employee.IsModified = true;
                    employee.LastModified = DateTime.Now;
                    employee.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    employee.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _employeeRepository.UpdateAsync(employee);
                        return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<EmployeeResponseModel>>> GetByEmail(string email)
        {
            try
            {

                if (email.Trim() == null) { throw new NullReferenceException(); }
                var employee = await _employeeRepository.GetByEmailAsync(email.Trim());
                if (employee == null) return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"employee having  email {email} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"employeer having email {email} retrieved successfully",
                        Data = employee.Select(x => ReturnEmployeeResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<BaseResponse<IEnumerable<EmployeeResponseModel>>> EmployeeBirthdayNotification(CancellationToken cancellationToken = default)
        {
            try
            {
                var employees = await _employeeRepository.EmployeeBirthdayNotification(cancellationToken);

                if (employees != null)
                {
                    return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Employees retrieved successfully",
                        Data = employees.Select(x => ReturnEmployeeResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Employees failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving employees: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<EmployeeResponseModel>>> SearchEmployee(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var employees = await _employeeRepository.SearchEmployees(keyword,cancellationToken);

                if (employees != null)
                {
                    return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Employees retrieved successfully",
                        Data = employees.Select(x => ReturnEmployeeResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Employees failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<EmployeeResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving employees: {ex.Message}",
                };
            }
        }


        /// <summary>
        /// /////////////
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        private EmployeeResponseModel ReturnEmployeeResponseModel(Employee model)
        {
            return new EmployeeResponseModel
            {
                Id = model.Id,
                UserId = model.ApplicatioUserId,
                StaffIdentityCardNumber = model.StaffIdentityCardNumber,
                FirstName = model.ApplicationUser.FirstName,
                LastName = model.ApplicationUser.LastName,
                RoleName = model.ApplicationUser.RoleName,
                Phonenumber = model.ApplicationUser.PhoneNumber,
                DateOfBirth = model.ApplicationUser.DateOfBirth,
                Email = model.ApplicationUser.Email,
                Gender = model.ApplicationUser.Gender,
                IsDeleted = model.IsDeleted,
                DeletedDate = model.DeletedDate,
                DeletedBy = model.DeletedBy,
                CreatorName = model.CreatorName,
                CreatorId = model.CreatorId,
                DateCreated = model.DateCreated,
                IsModified = model.IsModified,
                ModifierName = model.ModifierName,
                ModifierId = model.ModifierId,
                LastModified = model.LastModified,
                TerminalId = model.TerminalId

            };
        }

       
    }
}
