using Microsoft.AspNetCore.Http;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Terminal;
using PTS_CORE.Domain.Entities;
using PTS_DATA.Repository.Implementations;
using PTS_DATA.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PTS_BUSINESS.Services.Implementations
{
    public class StoreItemService : IStoreItemService
    {
        private readonly IStoreItemRepository _storeIterRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoreItemService(IStoreItemRepository storeIterRepository,
             IHttpContextAccessor httpContextAccessor)
        {
            _storeIterRepository = storeIterRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> ActivateStoreItem(string id)
        {

            if (id.Trim() != null)
            {
                try
                {
                    var storeItem = await _storeIterRepository.GetModelByIdAsync(id.Trim());

                    if (storeItem == null)
                        return false;
                    storeItem.IsDeleted = false;
                    storeItem.IsModified = true;
                    storeItem.LastModified = DateTime.Now;
                    storeItem.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    storeItem.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _storeIterRepository.UpdateAsync(storeItem);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateStoreItemRequestModel model)
        {
            if (model != null)
            {
                var storeItem = new StoreItem
                {

                    Name = !string.IsNullOrWhiteSpace(model.Name.Trim()) ? model.Name.Trim() : null,
                    Description = !string.IsNullOrWhiteSpace(model.Description.Trim()) ? model.Description.Trim() : null,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now
                };

                var result = await _storeIterRepository.CreateAsync(storeItem);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if terminal creation fails
                    Console.WriteLine($"There is error creating store item");
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
                    var storeItem = await _storeIterRepository.GetModelByIdAsync(id.Trim());

                    if (storeItem == null)
                        return false;
                    storeItem.IsDeleted = true;
                    storeItem.DeletedDate = DateTime.Now;
                    storeItem.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    storeItem.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _storeIterRepository.UpdateAsync(storeItem);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<StoreItemResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var storeItem = await _storeIterRepository.GetByIdAsync(id.Trim());
                if (storeItem == null) return new BaseResponse<IEnumerable<StoreItemResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"store item having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<StoreItemResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"store item having id {id} retrieved successfully",
                        Data = storeItem.Select(x => ReturnStoreItemResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<StoreItemResponseModel>>> GetAllStoreItems(CancellationToken cancellationToken = default)
        {
            try
            {
                var storeItem = await _storeIterRepository.GetAllAsync(cancellationToken);

                if (storeItem != null)
                {
                    return new BaseResponse<IEnumerable<StoreItemResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Store Item retrieved successfully",
                        Data = storeItem.Select(x => ReturnStoreItemResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<StoreItemResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Store Item failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<StoreItemResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving store items: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<StoreItemResponseModel>>> GetInactiveStoreItem(CancellationToken cancellationToken = default)
        {
            try
            {
                var storeItem = await _storeIterRepository.InactiveStoreItem(cancellationToken);

                if (storeItem != null)
                {
                    return new BaseResponse<IEnumerable<StoreItemResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Store Item retrieved successfully",
                        Data = storeItem.Select(x => ReturnStoreItemResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<StoreItemResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Store Item failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<StoreItemResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving store items: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<StoreItemResponseModel>>> SearchStoreItems(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var storeItem = await _storeIterRepository.SearchStoreItems(keyword,cancellationToken);

                if (storeItem != null)
                {
                    return new BaseResponse<IEnumerable<StoreItemResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Store Item retrieved successfully",
                        Data = storeItem.Select(x => ReturnStoreItemResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<StoreItemResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Store Item failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<StoreItemResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving store items: {ex.Message}",
                };
            }
        }

        public async Task<bool> UpdateStoreItem(UpdateStoreItemRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var storeItem = await _storeIterRepository.GetModelByIdAsync(updateModel.Id.Trim());
                    // var role = updateModel.RoleName != null ? await _roleManager.FindByNameAsync(updateModel.RoleName.Trim()) : null;

                    if (storeItem == null)
                        return false;

                    // Update user properties based on your model
                    storeItem.Name = !string.IsNullOrWhiteSpace(updateModel.Name.Trim()) ? updateModel.Name.Trim() : storeItem.Name;
                    storeItem.Description = !string.IsNullOrWhiteSpace(updateModel.Description.Trim()) ? updateModel.Description.Trim() : storeItem.Description;
                    storeItem.IsModified = true;
                    storeItem.LastModified = DateTime.Now;
                    storeItem.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    storeItem.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                

                    // Update other properties as needed
                    await _storeIterRepository.UpdateAsync(storeItem);
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
                private StoreItemResponseModel ReturnStoreItemResponseModel(StoreItem model)
                {
                    return new StoreItemResponseModel
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
