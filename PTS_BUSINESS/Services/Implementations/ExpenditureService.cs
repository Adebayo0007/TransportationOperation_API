using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Expenditure;
using Microsoft.AspNetCore.Http;
using PTS_DATA.Repository.Interfaces;
using System.Security.Claims;
using PTS_CORE.Domain.Entities;
using PTS_CORE.Domain.Entities.Enum;
using PTS_BUSINESS.Common;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreAsset;
using PTS_DATA.Repository.Implementations;

namespace PTS_BUSINESS.Services.Implementations
{
    public class ExpenditureService : IExpenditureService
    {
        private readonly IExpenditureRepository _expenditureRepository;
        private readonly ITerminalRepository _terminalRepository;
        private readonly IStoreItemRepository _storeItemRepository;
        private readonly IStoreAssetService _storeAssetService;
        private readonly IBudgetTrackingRepository _budgetTrackingRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExpenditureService(IExpenditureRepository expenditureRepository,
             IHttpContextAccessor httpContextAccessor,
             ITerminalRepository terminalRepository,
             IStoreItemRepository storeItemRepository,
             IBudgetTrackingRepository budgetTrackingRepository,
             IStoreAssetService storeAssetService)
        {
            _expenditureRepository = expenditureRepository;
            _httpContextAccessor = httpContextAccessor;
            _terminalRepository = terminalRepository;
            _storeItemRepository = storeItemRepository;
            _budgetTrackingRepository = budgetTrackingRepository;
            _storeAssetService = storeAssetService;
        }
        public async Task<bool> ActivateExpenditure(string id)
        {
            if (id.Trim() != null)
            {
                try
                {
                    var expenditure = await _expenditureRepository.GetModelByIdAsync(id.Trim());

                    if (expenditure == null)
                        return false;
                    expenditure.IsDeleted = false;
                    expenditure.IsModified = true;
                    expenditure.LastModified = DateTime.Now;
                    expenditure.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    expenditure.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _expenditureRepository.UpdateAsync(expenditure);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateExpenditureRequestModel model)
        {
            if (model != null)
            {
                StoreItem storeItem = null;
                Terminal terminal = null;
                if (!string.IsNullOrWhiteSpace(model.StoreItemId) && model.StoreItemId != "" && !string.IsNullOrWhiteSpace(model.TerminalId) && model.TerminalId != "")
                {
                    storeItem = await _storeItemRepository.GetModelByIdAsync(model.StoreItemId);
                    terminal = await _terminalRepository.GetModelByIdAsync(model.TerminalId);
                }
                var expenditure = new Expenditure();
                // {
                expenditure.Purpose = !string.IsNullOrWhiteSpace(model.Purpose.Trim()) ? model.Purpose.Trim() : null;
                expenditure.UnitPrice = model.UnitPrice != null ? model.UnitPrice : null;
                expenditure.IsProcurementApproved = model.UnitPrice != null && model.Purpose != null ? true : false;
                expenditure.IsAuditorCommented = model.UnitPrice != null && model.Purpose != null ? true : false;
                expenditure.IsDDPCommented = model.UnitPrice != null && model.Purpose != null ? true : false;
                expenditure.ItemQuantity = model.ItemQuantity != null ? model.ItemQuantity.Value : 1;
                expenditure.TerminalId = model.TerminalId != null ? model.TerminalId : null;
                expenditure.TerminalName = terminal != null ? terminal.Name : null;
                expenditure.StoreItemId = model.StoreItemId != null ? model.StoreItemId : null;
                expenditure.StoreItemName = storeItem != null ? storeItem.Name : null;
                expenditure.AuditorComment = $"Awaiting {RoleConstant.Auditor}'s Comment";
                expenditure.DDPComment = $"Awaiting {RoleConstant.DDP}'s Comment";
                expenditure.RequestType = RequestType.Financial;
                expenditure.CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                expenditure.CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                expenditure.DateCreated = DateTime.Now;
                // };

                var result = await _expenditureRepository.CreateAsync(expenditure);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if terminal creation fails
                    Console.WriteLine($"There is error creating expenditure");
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
                    var expenditure = await _expenditureRepository.GetModelByIdAsync(id.Trim());

                    if (expenditure == null)
                        return false;
                    expenditure.IsDeleted = true;
                    expenditure.DeletedDate = DateTime.Now;
                    expenditure.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    expenditure.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _expenditureRepository.UpdateAsync(expenditure);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<ExpenditureResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var expenditure = await _expenditureRepository.GetByIdAsync(id.Trim());
                if (expenditure == null) return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"expenditure having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"expenditure having id {id} retrieved successfully",
                        Data = expenditure.Select(x => ReturnExpenditureResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<ExpenditureResponseModel>>> GetAllExpenditures(CancellationToken cancellationToken = default)
        {
            try
            {
                var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                IEnumerable<Expenditure> expenditure = null;
                if (role == RoleConstant.ProcurementOfficer)
                    expenditure = await _expenditureRepository.GetAllForProcumentOfficer(cancellationToken);
                if (role == RoleConstant.Auditor)
                    expenditure = await _expenditureRepository.GetAllForAuditor(cancellationToken);
                if (role == RoleConstant.DDP)
                    expenditure = await _expenditureRepository.GetAllForDDP(cancellationToken);
                if (role == RoleConstant.Chairman || role == RoleConstant.Administrator)
                    expenditure = await _expenditureRepository.GetAllForChairman(cancellationToken);
                if (role == RoleConstant.FinanceManager)
                    expenditure = await _expenditureRepository.GetAllForFinance(cancellationToken);

                if (expenditure != null)
                {
                    return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Expenditure retrieved successfully",
                        Data = expenditure.Select(x => ReturnExpenditureResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Expenditure failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving expenditure: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<ExpenditureResponseModel>>> GetInactiveExpenditures(CancellationToken cancellationToken = default)
        {
            try
            {
                var expenditure = await _expenditureRepository.InactiveExpenditure(cancellationToken);

                if (expenditure != null)
                {
                    return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Expenditure retrieved successfully",
                        Data = expenditure.Select(x => ReturnExpenditureResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Expenditure failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving expenditure: {ex.Message}",
                };
            }
        }

        public async Task IsDenied(string id)
        {
            if (id != null)
            {
                var expenditure = await _expenditureRepository.GetModelByIdAsync(id.Trim());
                expenditure.IsDenied = true;
                expenditure.IsModified = true;
                expenditure.LastModified = DateTime.Now;
                expenditure.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                expenditure.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                await _expenditureRepository.UpdateAsync(expenditure);
            }
        }

        public async Task IsGranted(string id)
        {
            if (id != null)
            {
                var expenditure = await _expenditureRepository.GetModelByIdAsync(id.Trim());
                expenditure.IsChairmanApproved = true;
                expenditure.IsModified = true;
                expenditure.LastModified = DateTime.Now;
                expenditure.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                expenditure.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                await _expenditureRepository.UpdateAsync(expenditure);
            }
        }

        public async Task IsVerified(string id)
        {
            if (id != null)
            {
                var expenditure = await _expenditureRepository.GetModelByIdAsync(id.Trim());
                expenditure.IsVerified = true;
                expenditure.IsModified = true;
                expenditure.LastModified = DateTime.Now;
                expenditure.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                expenditure.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                await _expenditureRepository.UpdateAsync(expenditure);
            }
        }

        public async Task Resolve(string id)
        {
            if (id != null)
            {
                var expenditure = await _expenditureRepository.GetModelByIdAsync(id.Trim());
                var budget = await _budgetTrackingRepository.FindBudget(expenditure.LastModified.Value);
                budget.ActualAmount += expenditure.ItemQuantity.Value * expenditure.UnitPrice.Value;
                if (expenditure.TerminalName == null && expenditure.StoreItemName == null ||
                    expenditure.TerminalId == null && expenditure.StoreItemId == null)
                {
                    expenditure.IsResolved = true;
                    expenditure.IsModified = true;
                    expenditure.LastModified = DateTime.Now;
                    expenditure.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    expenditure.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                }
                else
                {
                    expenditure.IsResolved = true;
                    expenditure.IsModified = true;
                    expenditure.LastModified = DateTime.Now;
                    expenditure.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    expenditure.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    var asset = new CreateStoreAssetRequestModel
                    {
                        StoreItemId = expenditure.StoreItemId,
                        TerminalId = expenditure.TerminalId,
                        TotalQuantity = expenditure.ItemQuantity.Value
                    };
                    var storeAsset = await _storeAssetService.Create(asset);
                }
                await _expenditureRepository.UpdateAsync(expenditure);
            }
        }

        public async Task<BaseResponse<IEnumerable<ExpenditureResponseModel>>> ResolvedRequest(CancellationToken cancellationToken = default)
        {
            try
            {
                var expenditure = await _expenditureRepository.ResolvedRequest(cancellationToken);

                if (expenditure != null)
                {
                    return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Expenditure retrieved successfully",
                        Data = expenditure.Select(x => ReturnExpenditureResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Expenditure failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving expenditure: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<ExpenditureResponseModel>>> SearchExpenditures(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var expenditure = await _expenditureRepository.SearchExpenditure(keyword, cancellationToken);

                if (expenditure != null)
                {
                    return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Expenditure retrieved successfully",
                        Data = expenditure.Select(x => ReturnExpenditureResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Expenditure failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<ExpenditureResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving expenditure: {ex.Message}",
                };
            }
        }

        public async Task<bool> Update(UpdateExpenditureRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var expenditure = await _expenditureRepository.GetModelByIdAsync(updateModel.Id.Trim());

                    if (expenditure == null)
                        return false;

                    // Update user properties based on your model
                    expenditure.Purpose = !string.IsNullOrWhiteSpace(updateModel.Purpose.Trim()) && updateModel.Purpose != null ? updateModel.Purpose.Trim() : expenditure.Purpose;
                    expenditure.UnitPrice = updateModel.UnitPrice != null && updateModel.UnitPrice.Value > 0 ? updateModel.UnitPrice.Value : expenditure.UnitPrice;
                    string role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                    if (role == RoleConstant.ProcurementOfficer && updateModel.UnitPrice != null && updateModel.UnitPrice.Value > 0)
                    {
                        expenditure.IsProcurementApproved = true;
                    }
                    expenditure.ItemQuantity = updateModel.ItemQuantity != null && updateModel.ItemQuantity.Value > 0 ? updateModel.ItemQuantity.Value : expenditure.ItemQuantity;
                    expenditure.DDPComment = updateModel.DDPComment.Trim() != $"Awaiting {RoleConstant.DDP}'s Comment" && updateModel.DDPComment != null && updateModel.DDPComment != "" && updateModel.DDPComment != " " ? updateModel.DDPComment.Trim() : $"Awaiting {RoleConstant.DDP}'s Comment";
                    expenditure.IsDDPCommented = updateModel.DDPComment != null && updateModel.DDPComment.Trim() != "" && updateModel.DDPComment.Trim() != $"Awaiting {RoleConstant.DDP}'s Comment" ? true : expenditure.IsDDPCommented;
                    expenditure.AuditorComment = updateModel.AuditorComment.Trim() != $"Awaiting {RoleConstant.Auditor}'s Comment" && updateModel.AuditorComment != null && updateModel.AuditorComment != "" && updateModel.AuditorComment != " " ? updateModel.AuditorComment.Trim() : $"Awaiting {RoleConstant.Auditor}'s Comment";
                    expenditure.IsAuditorCommented = updateModel.AuditorComment != null && updateModel.AuditorComment != "" && updateModel.AuditorComment.Trim() != $"Awaiting {RoleConstant.Auditor}'s Comment" ? true : expenditure.IsAuditorCommented;
                    expenditure.IsModified = true;
                    expenditure.LastModified = DateTime.Now;
                    expenditure.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    expenditure.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                    // Update other properties as needed
                    await _expenditureRepository.UpdateAsync(expenditure);
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
        private ExpenditureResponseModel ReturnExpenditureResponseModel(Expenditure model)
        {
            return new ExpenditureResponseModel
            {
                Id = model.Id,
                Purpose = model.Purpose,
                UnitPrice = model.UnitPrice,
                ItemQuantity = model.ItemQuantity,
                StoreItemId = model.StoreItemId,
                StoreItemName = model.StoreItemName,
                TotalAmount = model.UnitPrice * model.ItemQuantity,
                TerminalId = model.TerminalId,
                TerminalName = model.TerminalName,

                IsChairmanApproved = model.IsChairmanApproved,
                RequestType = model.RequestType,
                IsDDPCommented = model.IsDDPCommented,
                DDPComment = model.DDPComment,
                IsAuditorCommented = model.IsAuditorCommented,
                AuditorComment = model.AuditorComment,
                IsProcurementApproved = model.IsProcurementApproved,
                IsResolved = model.IsResolved,
                IsDenied = model.IsDenied,
                IsApproved = model.IsApproved,
                IsVerified = model.IsVerified,

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
