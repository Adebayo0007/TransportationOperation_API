using Microsoft.AspNetCore.Http;
using PTS_BUSINESS.Common;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.StoreItemRequest;
using PTS_CORE.Domain.Entities;
using PTS_CORE.Domain.Entities.Enum;
using PTS_DATA.Repository.Interfaces;
using System.Security.Claims;

namespace PTS_BUSINESS.Services.Implementations
{
    public class StoreItemRequestService : IStoreItemRequestService
    {
        private readonly IStoreItemRequestRepository _storeItemRequestRepository;
        private readonly IStoreItemRepository _storeIterRepository;
        private readonly IStoreAssetRepository _storeAssetRepository;
        private readonly ITerminalRepository _terminalRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoreItemRequestService(IStoreItemRequestRepository storeItemRequestRepository,
            IStoreItemRepository storeIterRepository,
             IHttpContextAccessor httpContextAccessor,
             IStoreAssetRepository storeAssetRepository,
             ITerminalRepository terminalRepository)
        {
            _storeItemRequestRepository = storeItemRequestRepository;
            _httpContextAccessor = httpContextAccessor;
            _storeIterRepository = storeIterRepository;
            _storeAssetRepository = storeAssetRepository;
            _terminalRepository = terminalRepository;
        }
        public async Task<bool> ActivateStoreItemRequest(string id)
        {
            if (id != null)
            {
                try
                {
                    var terminal = await _storeItemRequestRepository.GetModelByIdAsync(id.Trim());

                    if (terminal == null)
                        return false;
                    terminal.IsDeleted = false;
                    terminal.IsModified = true;
                    terminal.LastModified = DateTime.Now;
                    terminal.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    terminal.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _storeItemRequestRepository.UpdateAsync(terminal);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> ApproveRequest(string id)
        {
            if (id != null)
            {
                try
                {
                    var storeItemRequest = await _storeItemRequestRepository.GetModelByIdAsync(id.Trim());
                    var terminal = await _terminalRepository.GetModelByNameAsync(storeItemRequest.TerminalName);
                    var storeAsset = await _storeAssetRepository.GetStoreAssetByTerminalIdAndStoreItemid(terminal.Id, storeItemRequest.StoreItemId);
                    if (terminal == null || storeItemRequest == null || storeAsset == null) return false;
                    if(storeAsset.TotalQuantity < storeItemRequest.Quantity) return false;
                    storeAsset.TotalQuantity -= storeItemRequest.Quantity;
                    storeAsset.IsModified = true;
                    storeAsset.LastModified = DateTime.Now;
                    storeAsset.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    storeAsset.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    if (storeItemRequest == null)
                        return false;

                    // Update user properties based on your model
               
                    storeItemRequest.IsResolved = true;
                    storeItemRequest.IsModified = true;
                    storeItemRequest.LastModified = DateTime.Now;
                    storeItemRequest.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    storeItemRequest.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _storeItemRequestRepository.UpdateAsync(storeItemRequest);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateStoreItemRequestRequestModel model)
        {
            if (model != null)
            {
                StoreItem storeItem = null;
                if (model.StoreItemId != null)
                {
                   storeItem = await _storeIterRepository.GetModelByIdAsync(model.StoreItemId);
                }
                var storeItemRequest = new StoreItemRequest
                {
                    Description = !string.IsNullOrWhiteSpace(model.Description.Trim()) ? model.Description.Trim() : null,
                    TerminalName = !string.IsNullOrWhiteSpace(model.TerminalName.Trim()) ? model.TerminalName.Trim() : null,
                    StoreItemId = !string.IsNullOrWhiteSpace(model.StoreItemId.Trim()) ? model.StoreItemId.Trim() : null,
                    StoreItemName = !string.IsNullOrWhiteSpace(storeItem.Name.Trim()) ? storeItem.Name : null,
                    ReasonForRequest = !string.IsNullOrWhiteSpace(model.ReasonForRequest.Trim()) ? model.ReasonForRequest.Trim() : null,
                    VehicleRegistrationNumber = model.VehicleRegistrationNumber != null ? model.VehicleRegistrationNumber.Trim() : null,
                    StoreItemType = model.StoreItemType != null ? model.StoreItemType.Value : StoreItemType.Unknown,
                    RequestType = RequestType.NotFinancial,
                    Quantity = model.Quantity > 0 ? model.Quantity : 0,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now
                };

                var result = await _storeItemRequestRepository.CreateAsync(storeItemRequest);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if terminal creation fails
                    Console.WriteLine($"There is error creating terminal");
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
                    var storeItemRequest = await _storeItemRequestRepository.GetModelByIdAsync(id.Trim());

                    if (storeItemRequest == null)
                        return false;
                    storeItemRequest.IsDeleted = true;
                    storeItemRequest.DeletedDate = DateTime.Now;
                    storeItemRequest.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    storeItemRequest.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _storeItemRequestRepository.UpdateAsync(storeItemRequest);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<StoreItemRequestResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var storeItemRequest = await _storeItemRequestRepository.GetByIdAsync(id.Trim());
                if (storeItemRequest == null) return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"store item request having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"store item having id {id} retrieved successfully",
                        Data = storeItemRequest.Select(x => ReturnStoreItemRequestResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<StoreItemRequestResponseModel>>> GetAllStoreItemRequests(CancellationToken cancellationToken = default)
        {
            try
            {
                var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                IEnumerable<StoreItemRequest> storeItemRequest = null;
                if(role == RoleConstant.StoreManager)
                   storeItemRequest = await _storeItemRequestRepository.GetAllForStore(cancellationToken);
                if (role == RoleConstant.Auditor)
                    storeItemRequest = await _storeItemRequestRepository.GetAllForAuditor(cancellationToken);
                if (role == RoleConstant.DDP)
                    storeItemRequest = await _storeItemRequestRepository.GetAllForDDP(cancellationToken);
                if (role == RoleConstant.Chairman || role == RoleConstant.Administrator)
                    storeItemRequest = await _storeItemRequestRepository.GetAllForChirman(cancellationToken);


                if (storeItemRequest != null)
                {
                    return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"store item request retrieved successfully",
                        Data = storeItemRequest.Select(x => ReturnStoreItemRequestResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"store item Request failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving store item requests: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<StoreItemRequestResponseModel>>> GetInactiveStoreItemRequest(CancellationToken cancellationToken = default)
        {
            try
            {
                var storeItemRequest = await _storeItemRequestRepository.InactiveStoreItemRequest(cancellationToken);

                if (storeItemRequest != null)
                {
                    return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"store item request retrieved successfully",
                        Data = storeItemRequest.Select(x => ReturnStoreItemRequestResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"store item Request failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving store item requests: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<StoreItemRequestResponseModel>>> MystoreItemRequest(CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (userId == null) { throw new NullReferenceException(); }
                var storeItemRequest = await _storeItemRequestRepository.MystoreItemRequest(userId.Trim(), cancellationToken);
                if (storeItemRequest == null) return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"store item request having  id {userId} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"store item having id {userId} retrieved successfully",
                        Data = storeItemRequest.Select(x => ReturnStoreItemRequestResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async  Task<BaseResponse<IEnumerable<StoreItemRequestResponseModel>>> ResolvedStoreItemRequest(CancellationToken cancellationToken = default)
        {
            try
            {
                var storeItemRequest = await _storeItemRequestRepository.GetAllAsync(cancellationToken);

                if (storeItemRequest != null)
                {
                    return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"store item request retrieved successfully",
                        Data = storeItemRequest.Select(x => ReturnStoreItemRequestResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"store item Request failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving store item requests: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<StoreItemRequestResponseModel>>> SearchStoreItemRequests(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var storeItemRequest = await _storeItemRequestRepository.SearchStoreItemRequest(keyword,cancellationToken);

                if (storeItemRequest != null)
                {
                    return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"store item request retrieved successfully",
                        Data = storeItemRequest.Select(x => ReturnStoreItemRequestResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"store item Request failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<StoreItemRequestResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving store item requests: {ex.Message}",
                };
            }
        }

        public async Task<bool> UpdateStoreItemRequest(UpdateStoreItemRequestRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var storeItemRequest = await _storeItemRequestRepository.GetModelByIdAsync(updateModel.Id.Trim());
                  /*  StoreItem storeItem = null;
                    if (updateModel.StoreItemId != null)
                    {
                        storeItem = await _storeIterRepository.GetModelByIdAsync(updateModel.StoreItemId);
                    }*/
                    // var role = updateModel.RoleName != null ? await _roleManager.FindByNameAsync(updateModel.RoleName.Trim()) : null;

                    if (storeItemRequest == null)
                        return false;

                    // Update user properties based on your model
                    storeItemRequest.Description = !string.IsNullOrWhiteSpace(updateModel.Description.Trim()) && updateModel.Description != null ? updateModel.Description.Trim() : storeItemRequest.Description;
                  //  storeItemRequest.TerminalName = !string.IsNullOrWhiteSpace(updateModel.TerminalName.Trim()) ? updateModel.TerminalName.Trim() : storeItemRequest.TerminalName;
                  //  storeItemRequest.StoreItemId = !string.IsNullOrWhiteSpace(updateModel.StoreItemId.Trim()) ? updateModel.StoreItemId.Trim() : storeItemRequest.StoreItemId;
                    storeItemRequest.ReasonForRequest = !string.IsNullOrWhiteSpace(updateModel.ReasonForRequest.Trim()) && updateModel.ReasonForRequest != null ? updateModel.ReasonForRequest.Trim() : storeItemRequest.ReasonForRequest;
                    //storeItemRequest.VehicleRegistrationNumber = !string.IsNullOrWhiteSpace(updateModel.VehicleRegistrationNumber.Trim()) ? updateModel.VehicleRegistrationNumber.Trim() : storeItemRequest.VehicleRegistrationNumber;
                    storeItemRequest.DDPComment = updateModel.DDPComment != null ? updateModel.DDPComment.Trim() : $"Awaiting {RoleConstant.DDP}'s Comment";
                    storeItemRequest.IsDDPCommented = updateModel.DDPComment != null && updateModel.DDPComment != $"Awaiting {RoleConstant.DDP}'s Comment" ? true : storeItemRequest.IsDDPCommented;
                    storeItemRequest.AuditorComment = updateModel.AuditorComment != null ? updateModel.AuditorComment.Trim() : $"Awaiting {RoleConstant.Auditor}'s Comment";
                    storeItemRequest.IsAuditorCommented = updateModel.AuditorComment != null && updateModel.AuditorComment != $"Awaiting {RoleConstant.Auditor}'s Comment" ? true : storeItemRequest.IsAuditorCommented;
                    //storeItemRequest.StoreItemName = storeItem != null ? storeItem.Name : storeItemRequest.StoreItemName;
                    //storeItemRequest.StoreItemType = updateModel.StoreItemType != null || updateModel.StoreItemType != 0 ? updateModel.StoreItemType.Value : storeItemRequest.StoreItemType;
                    storeItemRequest.Quantity = updateModel.Quantity >0 ? updateModel.Quantity.Value : storeItemRequest.Quantity;
                    storeItemRequest.IsModified = true;
                    storeItemRequest.LastModified = DateTime.Now;
                    storeItemRequest.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    storeItemRequest.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                    string role = null;
                    role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                    if (role == RoleConstant.StoreManager)
                    {
                        storeItemRequest.AvailabilityType = updateModel.AvailabilityType != null || updateModel.AvailabilityType != 0 ? updateModel.AvailabilityType.Value : storeItemRequest.AvailabilityType;
                    }

                    // Update other properties as needed
                    await _storeItemRequestRepository.UpdateAsync(storeItemRequest);
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
        private StoreItemRequestResponseModel ReturnStoreItemRequestResponseModel(StoreItemRequest model)
        {
            return new StoreItemRequestResponseModel
            {
                Id = model.Id,
                TerminalName = model.TerminalName,
                StoreItemId = model.StoreItemId,
                StoreItemName = model.StoreItemName,
                ReasonForRequest = model.ReasonForRequest,
                Quantity = model.Quantity,
                StoreItemType = model.StoreItemType,
                VehicleRegistrationNumber = model.VehicleRegistrationNumber,
                Description = model.Description,

                IsChairmanApproved = model.IsChairmanApproved,
                RequestType = model.RequestType,
                AvailabilityType = model.AvailabilityType,
                IsDDPCommented = model.IsDDPCommented,
                DDPComment = model.DDPComment,
                IsAuditorCommented = model.IsAuditorCommented,
                AuditorComment = model.AuditorComment,
                IsResolved = model.IsResolved,
                IsAvailable = model.IsAvailable,
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
