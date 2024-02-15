using Microsoft.AspNetCore.Http;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.DepartmentalExpenditure;
using PTS_CORE.Domain.DataTransferObject.RequestModel.DepartmentalSale;
using PTS_CORE.Domain.Entities;
using PTS_DATA.Repository.Interfaces;
using System.Security.Claims;

namespace PTS_BUSINESS.Services.Implementations
{
    public class DepartmentalSaleService : IDepartmentalSaleService
    {

        private readonly IDepartmentalSaleRepository _departmentalSaleRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DepartmentalSaleService(IDepartmentalSaleRepository departmentalSaleRepository,
             IHttpContextAccessor httpContextAccessor,
             IDepartmentRepository departmentRepository)
        {
            _departmentalSaleRepository = departmentalSaleRepository;
            _httpContextAccessor = httpContextAccessor;
            _departmentRepository = departmentRepository;
        }
        public async Task<bool> ActivateDepartmentalSale(string id)
        {
            if (id.Trim() != null)
            {
                try
                {
                    var budget = await _departmentalSaleRepository.GetModelByIdAsync(id.Trim());

                    if (budget == null)
                        return false;
                    budget.IsDeleted = false;
                    budget.IsModified = true;
                    budget.LastModified = DateTime.Now;
                    budget.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    budget.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _departmentalSaleRepository.UpdateAsync(budget);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateDepartmentalSale model)
        {
            if (model != null)
            {
                var department = await _departmentRepository.GetModelByIdAsync(model.DepartmentId);
                var budget = new DepartmentalSale
                {
                    DepartmentId = model.DepartmentId != null ? model.DepartmentId : null,
                    DepartmentName = department.Name,
                    StartDate = model.StartDate != null && model.StartDate != DateTime.MinValue ? model.StartDate : DateTime.Now,
                    EndDate = model.EndDate != null && model.EndDate != DateTime.MinValue ? model.EndDate : DateTime.Now,
                    BudgetedAmount = model.BudgetedAmount > 0 && model.BudgetedAmount != 0 ? model.BudgetedAmount : 0,
                    ActualAmount = 0,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now
                };

                var result = await _departmentalSaleRepository.CreateAsync(budget);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if terminal creation fails
                    Console.WriteLine($"There is error creating budget");
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
                    var budget = await _departmentalSaleRepository.GetModelByIdAsync(id.Trim());

                    if (budget == null)
                        return false;
                    budget.IsDeleted = true;
                    budget.DeletedDate = DateTime.Now;
                    budget.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    budget.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _departmentalSaleRepository.UpdateAsync(budget);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var budget = await _departmentalSaleRepository.GetByIdAsync(id.Trim());
                if (budget == null) return new BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"budget having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"budget having id {id} retrieved successfully",
                        Data = budget.Select(x => ReturnDepartmentalSaleResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>> GetAllDepartmentalSales(CancellationToken cancellationToken = default)
        {
            try
            {
                var budget = await _departmentalSaleRepository.GetAllAsync(cancellationToken);

                if (budget != null)
                {
                    return new BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Budget retrieved successfully",
                        Data = budget.Select(x => ReturnDepartmentalSaleResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Budget failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving budget: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>> GetInactiveDepartmentalSales(CancellationToken cancellationToken = default)
        {
            try
            {
                var budget = await _departmentalSaleRepository.InactiveBudgetTrackings(cancellationToken);

                if (budget != null)
                {
                    return new BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Budget retrieved successfully",
                        Data = budget.Select(x => ReturnDepartmentalSaleResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Budget failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving budget: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>> SearchDepartmentalSales(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var budget = await _departmentalSaleRepository.SearchBudgetTrackings(keyword,cancellationToken);

                if (budget != null)
                {
                    return new BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Budget retrieved successfully",
                        Data = budget.Select(x => ReturnDepartmentalSaleResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Budget failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving budget: {ex.Message}",
                };
            }
        }

        public async Task<bool> UpdateDepartmentalSale(UpdateDepartmentalSaleRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var budget = await _departmentalSaleRepository.GetModelByIdAsync(updateModel.Id.Trim());
                    // var role = updateModel.RoleName != null ? await _roleManager.FindByNameAsync(updateModel.RoleName.Trim()) : null;

                    if (budget == null)
                        return false;

                    // Update user properties based on your model
                    budget.StartDate = updateModel.StartDate != null && updateModel.StartDate != DateTime.MinValue ? updateModel.StartDate.Value : budget.StartDate;
                    budget.EndDate = updateModel.EndDate != null && updateModel.EndDate != DateTime.MinValue ? updateModel.EndDate.Value : budget.EndDate;
                    budget.BudgetedAmount = updateModel.BudgetedAmount != null && updateModel.BudgetedAmount.Value != 0 ? updateModel.BudgetedAmount.Value : budget.BudgetedAmount;
                    budget.IsModified = true;
                    budget.LastModified = DateTime.Now;
                    budget.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    budget.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;


                    // Update other properties as needed
                    await _departmentalSaleRepository.UpdateAsync(budget);
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
        private DepartmentalSaleResponseModel ReturnDepartmentalSaleResponseModel(DepartmentalSale model)
        {
            return new DepartmentalSaleResponseModel
            {
                Id = model.Id,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ActualAmount = model.ActualAmount.Value,
                BudgetedAmount = model.BudgetedAmount,
                Difference = model.BudgetedAmount - model.ActualAmount.Value,
                DepartmentId = model.DepartmentId,
                DepartmentName = model.DepartmentName,
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
