using Microsoft.AspNetCore.Http;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;
using PTS_CORE.Domain.DataTransferObject.StaffAsset;
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
    public class StaffAssetService : IStaffAssetService
    {
        private readonly IStaffAssetRepository _staffAssetRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StaffAssetService(IStaffAssetRepository staffAssetRepository,
             IHttpContextAccessor httpContextAccessor)
        {
            _staffAssetRepository = staffAssetRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> ActivateStaffAsset(string id)
        {
            if (id.Trim() != null)
            {
                try
                {
                    var staffAsset = await _staffAssetRepository.GetModelByIdAsync(id.Trim());

                    if (staffAsset == null)
                        return false;
                    staffAsset.IsDeleted = false;
                    staffAsset.IsModified = true;
                    staffAsset.LastModified = DateTime.Now;
                    staffAsset.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    staffAsset.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _staffAssetRepository.UpdateAsync(staffAsset);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateStaffAssetRequestModel model)
        {
            if (model != null)
            {
                var staffAsset = new StaffAssets
                {

                    StoreItemName = !string.IsNullOrWhiteSpace(model.StoreItemName.Trim()) ? model.StoreItemName.Trim() : null,
                    Owner = !string.IsNullOrWhiteSpace(model.Owner.Trim()) ? model.Owner.Trim() : null,
                    Quantity = model.Quantity > 0 ? model.Quantity : 0,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now
                };

                var result = await _staffAssetRepository.CreateAsync(staffAsset);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if terminal creation fails
                    Console.WriteLine($"There is error creating staff asset");
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
                    var staffAsset = await _staffAssetRepository.GetModelByIdAsync(id.Trim());

                    if (staffAsset == null)
                        return false;
                    staffAsset.IsDeleted = true;
                    staffAsset.DeletedDate = DateTime.Now;
                    staffAsset.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    staffAsset.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _staffAssetRepository.UpdateAsync(staffAsset);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<StaffAssetResponseModel>>> Get(string id)
        {
            try
            {
                if (id == null) { throw new NullReferenceException(); }
                var staffAsset = await _staffAssetRepository.GetByIdAsync(id.Trim());
                if (staffAsset == null) return new BaseResponse<IEnumerable<StaffAssetResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"staff asset having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<StaffAssetResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"staff asset having id {id} retrieved successfully",
                        Data = staffAsset.Select(x => ReturnStaffAssetResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<StaffAssetResponseModel>>> GetAllStaffAsset(CancellationToken cancellationToken = default)
        {
            try
            {
                var staffAsset = await _staffAssetRepository.GetAllAsync(cancellationToken);

                if (staffAsset != null)
                {
                    return new BaseResponse<IEnumerable<StaffAssetResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Staff asset retrieved successfully",
                        Data = staffAsset.Select(x => ReturnStaffAssetResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<StaffAssetResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Staff asset failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<StaffAssetResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving staff assets: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<StaffAssetResponseModel>>> GetInactiveStaffAsset(CancellationToken cancellationToken = default)
        {
            try
            {
                var staffAsset = await _staffAssetRepository.InactiveStaffAsset(cancellationToken);

                if (staffAsset != null)
                {
                    return new BaseResponse<IEnumerable<StaffAssetResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Staff asset retrieved successfully",
                        Data = staffAsset.Select(x => ReturnStaffAssetResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<StaffAssetResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Staff asset failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<StaffAssetResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving staff asset: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<StaffAssetResponseModel>>> SearchStaffAsset(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var staffAsset = await _staffAssetRepository.SearchStaffAsset(keyword,cancellationToken);

                if (staffAsset != null)
                {
                    return new BaseResponse<IEnumerable<StaffAssetResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Staff asset retrieved successfully",
                        Data = staffAsset.Select(x => ReturnStaffAssetResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<StaffAssetResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Staff asset failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<StaffAssetResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving staff asset: {ex.Message}",
                };
            }
        }

        /// <summary>
        /// /////////////
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        private StaffAssetResponseModel ReturnStaffAssetResponseModel(StaffAssets model)
        {
            return new StaffAssetResponseModel
            {
                Id = model.Id,
                StoreItemName = model.StoreItemName,
                Owner = model.Owner,
                Quantity = model.Quantity,
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
