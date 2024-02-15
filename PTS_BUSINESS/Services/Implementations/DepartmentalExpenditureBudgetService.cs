﻿using Microsoft.AspNetCore.Http;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.BudgetTraching;
using PTS_CORE.Domain.DataTransferObject.RequestModel.DepartmentalExpenditure;
using PTS_CORE.Domain.Entities;
using PTS_DATA.Repository.Implementations;
using PTS_DATA.Repository.Interfaces;
using System.Security.Claims;

namespace PTS_BUSINESS.Services.Implementations
{
    public class DepartmentalExpenditureBudgetService : IDepartmentalExpenditureBudgetService
    {
        private readonly IDepartmentalExpenditureBudgetRepository _departmentalExpendituerRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DepartmentalExpenditureBudgetService(IDepartmentalExpenditureBudgetRepository departmentalExpendituerRepository,
             IHttpContextAccessor httpContextAccessor,
             IDepartmentRepository departmentRepository)
        {
            _departmentalExpendituerRepository = departmentalExpendituerRepository;
            _httpContextAccessor = httpContextAccessor;
            _departmentRepository = departmentRepository;
        }

        public async Task<bool> ActivateDepartmentalExpenditure(string id)
        {
            if (id.Trim() != null)
            {
                try
                {
                    var budget = await _departmentalExpendituerRepository.GetModelByIdAsync(id.Trim());

                    if (budget == null)
                        return false;
                    budget.IsDeleted = false;
                    budget.IsModified = true;
                    budget.LastModified = DateTime.Now;
                    budget.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    budget.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _departmentalExpendituerRepository.UpdateAsync(budget);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateDepartmentalExpenditure model)
        {
            if (model != null)
            {
                var department = await _departmentRepository.GetModelByIdAsync(model.DepartmentId);
                var budget = new DepartmentalExpenditureBudget
                {
                    DepartmentId = model.DepartmentId != null? model.DepartmentId : null,
                    DepartmentName = department.Name,
                    StartDate = model.StartDate != null && model.StartDate != DateTime.MinValue ? model.StartDate : DateTime.Now,
                    EndDate = model.EndDate != null && model.EndDate != DateTime.MinValue ? model.EndDate : DateTime.Now,
                    BudgetedAmount = model.BudgetedAmount > 0 && model.BudgetedAmount != 0 ? model.BudgetedAmount : 0,
                    ActualAmount = 0,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now
                };

                var result = await _departmentalExpendituerRepository.CreateAsync(budget);

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
                    var budget = await _departmentalExpendituerRepository.GetModelByIdAsync(id.Trim());

                    if (budget == null)
                        return false;
                    budget.IsDeleted = true;
                    budget.DeletedDate = DateTime.Now;
                    budget.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    budget.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _departmentalExpendituerRepository.UpdateAsync(budget);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var budget = await _departmentalExpendituerRepository.GetByIdAsync(id.Trim());
                if (budget == null) return new BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"budget having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"budget having id {id} retrieved successfully",
                        Data = budget.Select(x => ReturnDepartmentalExpenditureResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>> GetAllDepartmentalExpenditure(CancellationToken cancellationToken = default)
        {
            try
            {
                var budget = await _departmentalExpendituerRepository.GetAllAsync(cancellationToken);

                if (budget != null)
                {
                    return new BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Budget retrieved successfully",
                        Data = budget.Select(x => ReturnDepartmentalExpenditureResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Budget failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving budget: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>> GetInactiveDepartmentalExpenditure(CancellationToken cancellationToken = default)
        {
            try
            {
                var budget = await _departmentalExpendituerRepository.InactiveBudgetTrackings(cancellationToken);

                if (budget != null)
                {
                    return new BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Budget retrieved successfully",
                        Data = budget.Select(x => ReturnDepartmentalExpenditureResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Budget failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving budget: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>> SearchDepartmentalExpenditures(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var budget = await _departmentalExpendituerRepository.SearchBudgetTrackings(keyword,cancellationToken);

                if (budget != null)
                {
                    return new BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Budget retrieved successfully",
                        Data = budget.Select(x => ReturnDepartmentalExpenditureResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Budget failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving budget: {ex.Message}",
                };
            }
        }

        public async Task<bool> UpdateDepartmentalExpenditure(UpdateDepartmentalExpenditureRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var budget = await _departmentalExpendituerRepository.GetModelByIdAsync(updateModel.Id.Trim());
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
                    await _departmentalExpendituerRepository.UpdateAsync(budget);
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
        private DepartmentalExpenditureResponseModel ReturnDepartmentalExpenditureResponseModel(DepartmentalExpenditureBudget model)
        {
            return new DepartmentalExpenditureResponseModel
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
