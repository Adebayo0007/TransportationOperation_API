using Microsoft.AspNetCore.Http;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreAsset;
using PTS_CORE.Domain.Entities;
using PTS_DATA.Repository.Interfaces;
using System.Security.Claims;

namespace PTS_BUSINESS.Services.Implementations
{
    public class StoreAssetService : IStoreAssetService
    {
        private readonly IStoreAssetRepository _storeAssetRepository;
        private readonly IStoreItemRepository _storeItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITerminalRepository _terminalRepository;
        public StoreAssetService(IStoreAssetRepository storeAssetRepository,
             IHttpContextAccessor httpContextAccessor,
             IStoreItemRepository storeItemRepository,
             ITerminalRepository terminalRepository)
        {
            _storeAssetRepository = storeAssetRepository;
            _httpContextAccessor = httpContextAccessor;
            _storeItemRepository = storeItemRepository;
            _terminalRepository = terminalRepository;
        }
        public async Task<bool> ActivateStoreAsset(string id)
        {
            if (id.Trim() != null)
            {
                try
                {
                    var storeAsset = await _storeAssetRepository.GetModelByIdAsync(id.Trim());

                    if (storeAsset == null)
                        return false;
                    storeAsset.IsDeleted = false;
                    storeAsset.IsModified = true;
                    storeAsset.LastModified = DateTime.Now;
                    storeAsset.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    storeAsset.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _storeAssetRepository.UpdateAsync(storeAsset);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateStoreAssetRequestModel model)
        {
            var storeAssetModel = await _storeAssetRepository.GetStoreAssetByTerminalIdAndStoreItemid(model.TerminalId, model.StoreItemId);
            if (storeAssetModel == null)
            {
                if (model != null)
                {
                    Terminal terminal = null;
                    StoreItem storeItem = null;
                    if (model.TerminalId != null)
                        terminal = await _terminalRepository.GetModelByIdAsync(model.TerminalId.Trim());
                    if (model.StoreItemId != null)
                        storeItem = await _storeItemRepository.GetModelByIdAsync(model.StoreItemId.Trim());

                    var storeAsset = new StoreAsset
                    {
                        StoreItemDescription = storeItem != null ? storeItem.Description : null,
                        StoreItemId = !string.IsNullOrWhiteSpace(model.StoreItemId.Trim()) ? model.StoreItemId.Trim() : null,
                        TerminalId = !string.IsNullOrWhiteSpace(model.TerminalId.Trim()) ? model.TerminalId.Trim() : null,
                        TerminalName = terminal != null ? terminal.Name : null,
                        StoreItemName = storeItem != null ? storeItem.Name : null,
                        TotalQuantity = model.TotalQuantity > 0 ? model.TotalQuantity : 0,
                        LastQuantityAdded = model.TotalQuantity,
                        CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                        CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                        DateCreated = DateTime.Now
                    };

                    var result = await _storeAssetRepository.CreateAsync(storeAsset);

                    if (result == true)
                    {
                        return true;
                    }
                    else
                    {
                        // Handle errors if terminal creation fails
                        Console.WriteLine($"There is error creating store asset");
                        return false;
                    }
                }
                else
                    return false;
            }
            else
            {
                storeAssetModel.LastQuantityAdded = model.TotalQuantity;
                storeAssetModel.TotalQuantity += model.TotalQuantity;
                storeAssetModel.IsModified = true;
                storeAssetModel.LastModified = DateTime.Now;
                storeAssetModel.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                storeAssetModel.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                await _storeAssetRepository.UpdateAsync(storeAssetModel);
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(string id)
        {
            if (id != null)
            {
                try
                {
                    var storeAsset = await _storeAssetRepository.GetModelByIdAsync(id.Trim());

                    if (storeAsset == null)
                        return false;
                    storeAsset.IsDeleted = true;
                    storeAsset.DeletedDate = DateTime.Now;
                    storeAsset.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    storeAsset.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _storeAssetRepository.UpdateAsync(storeAsset);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<StoreAssetResponseModel>>> Get(string id)
        {
            try
            {
                if (id == null) { throw new NullReferenceException(); }
                var storeAsset = await _storeAssetRepository.GetByIdAsync(id.Trim());
                if (storeAsset == null) return new BaseResponse<IEnumerable<StoreAssetResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"store asset having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<StoreAssetResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"store asset having id {id} retrieved successfully",
                        Data = storeAsset.Select(x => ReturnStoreAssetResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<StoreAssetResponseModel>>> GetAllStoreAsset(CancellationToken cancellationToken = default)
        {
            try
            {
                var storeAsset = await _storeAssetRepository.GetAllAsync(cancellationToken);

                if (storeAsset != null)
                {
                    return new BaseResponse<IEnumerable<StoreAssetResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Store Asset retrieved successfully",
                        Data = storeAsset.Select(x => ReturnStoreAssetResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<StoreAssetResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Store Asset failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<StoreAssetResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving store asset: {ex.Message}",
                };
            }
        }

        public Task<BaseResponse<IEnumerable<StoreAssetResponseModel>>> GetInactiveStoreAsset(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<IEnumerable<StoreAssetResponseModel>>> SearchStoreAsset(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var storeAsset = await _storeAssetRepository.SearchStoreAsset(keyword,cancellationToken);

                if (storeAsset != null)
                {
                    return new BaseResponse<IEnumerable<StoreAssetResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Store Asset retrieved successfully",
                        Data = storeAsset.Select(x => ReturnStoreAssetResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<StoreAssetResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Store Asset failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<StoreAssetResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving store asset: {ex.Message}",
                };
            }
        }

        /// <summary>
        /// /////////////
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        private StoreAssetResponseModel ReturnStoreAssetResponseModel(StoreAsset model)
        {
            return new StoreAssetResponseModel
            {
                Id = model.Id,
                StoreItemDescription = model.StoreItemDescription,
                StoreItemId = model.StoreItemId,
                StoreItemName = model.StoreItemName,
                TerminalId = model.TerminalId,
                TerminalName = model.TerminalName,
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
                LastQuantityAdded = model.LastQuantityAdded,
                TotalQuantity = model.TotalQuantity
            };
        }
    }
}
