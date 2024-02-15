using Microsoft.AspNetCore.Http;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Depratment;
using PTS_CORE.Domain.Entities;
using PTS_DATA.Repository.Interfaces;
using System.Security.Claims;

namespace PTS_BUSINESS.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DepartmentService(IDepartmentRepository departmentRepository,
             IHttpContextAccessor httpContextAccessor)
        {
            _departmentRepository = departmentRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> ActivateDepartment(string id)
        {
            if (id.Trim() != null)
            {
                try
                {
                    var department = await _departmentRepository.GetModelByIdAsync(id.Trim());

                    if (department == null)
                        return false;
                    department.IsDeleted = false;
                    department.IsModified = true;
                    department.LastModified = DateTime.Now;
                    department.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    department.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _departmentRepository.UpdateAsync(department);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateDepartmentalSaleRequestModel model)
        {
            if (model != null)
            {
                var department = new Department
                {

                    Name = !string.IsNullOrWhiteSpace(model.Name.Trim()) ? model.Name.Trim() : null,
                    Description = !string.IsNullOrWhiteSpace(model.Description.Trim()) ? model.Description.Trim() : null,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now
                };

                var result = await _departmentRepository.CreateAsync(department);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if terminal creation fails
                    Console.WriteLine($"There is error creating department");
                    return false;
                }
            }
            else
                return false;
        }

        public async Task<bool> Delete(string id)
        {
            if (id != null)
            {
                try
                {
                    var department = await _departmentRepository.GetModelByIdAsync(id.Trim());

                    if (department == null)
                        return false;
                    department.IsDeleted = true;
                    department.DeletedDate = DateTime.Now;
                    department.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    department.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _departmentRepository.UpdateAsync(department);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<DepartmentResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var department = await _departmentRepository.GetByIdAsync(id.Trim());
                if (department == null) return new BaseResponse<IEnumerable<DepartmentResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"department having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<DepartmentResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Department having id {id} retrieved successfully",
                        Data = department.Select(x => ReturnDepartmentResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<DepartmentResponseModel>>> GetAllDepartments(CancellationToken cancellationToken = default)
        {
            try
            {
                var department = await _departmentRepository.GetAllAsync(cancellationToken);

                if (department != null)
                {
                    return new BaseResponse<IEnumerable<DepartmentResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Department retrieved successfully",
                        Data = department.Select(x => ReturnDepartmentResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<DepartmentResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Department failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<DepartmentResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving department: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<DepartmentResponseModel>>> GetInactiveDepartments(CancellationToken cancellationToken = default)
        {
            try
            {
                var department = await _departmentRepository.InactiveDepartments(cancellationToken);

                if (department != null)
                {
                    return new BaseResponse<IEnumerable<DepartmentResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Department retrieved successfully",
                        Data = department.Select(x => ReturnDepartmentResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<DepartmentResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Department failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<DepartmentResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving department: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<DepartmentResponseModel>>> SearchDepartment(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var department = await _departmentRepository.SearchDepartments(keyword,cancellationToken);

                if (department != null)
                {
                    return new BaseResponse<IEnumerable<DepartmentResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Department retrieved successfully",
                        Data = department.Select(x => ReturnDepartmentResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<DepartmentResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Department failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<DepartmentResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving department: {ex.Message}",
                };
            }
        }

        public async Task<bool> UpdateDepartment(UpdateDepartmentRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var department = await _departmentRepository.GetModelByIdAsync(updateModel.Id.Trim());
                    // var role = updateModel.RoleName != null ? await _roleManager.FindByNameAsync(updateModel.RoleName.Trim()) : null;

                    if (department == null)
                        return false;

                    // Update user properties based on your model
                    department.Name = !string.IsNullOrWhiteSpace(updateModel.Name.Trim()) ? updateModel.Name.Trim() : department.Name;
                    department.Description = !string.IsNullOrWhiteSpace(updateModel.Description.Trim()) ? updateModel.Description.Trim() : department.Description;
                    department.IsModified = true;
                    department.LastModified = DateTime.Now;
                    department.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    department.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;


                    // Update other properties as needed
                    await _departmentRepository.UpdateAsync(department);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        /// <summary>
        /// /////////////
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        private DepartmentResponseModel ReturnDepartmentResponseModel(Department model)
        {
            return new DepartmentResponseModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsDeleted = model.IsDeleted,
                DeletedDate = model.DeletedDate,
                DeletedBy = model.DeletedBy,
                CreatorName = model.CreatorName,
                CreatorId = model.CreatorId,
                DateCreated = model.DateCreated,
                IsModified = model.IsModified,
                ModifierName = model.ModifierName,
                ModifierId = model.ModifierId,
                LastModified = model.LastModified
            };
        }
    }
}
