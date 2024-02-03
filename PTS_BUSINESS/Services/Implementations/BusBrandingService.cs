using Microsoft.AspNetCore.Http;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.BusBranding;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;
using PTS_CORE.Domain.Entities;
using PTS_CORE.Domain.Entities.Enum;
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
    public class BusBrandingService : IBusBrandingService
    {
        private readonly IBusBrandingRepository _busBrandingRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BusBrandingService(IBusBrandingRepository busBrandingRepository,
             IHttpContextAccessor httpContextAccessor, ISaleRepository saleRepository)
        {
            _busBrandingRepository = busBrandingRepository;
            _httpContextAccessor = httpContextAccessor;
            _busBrandingRepository = busBrandingRepository;
            _saleRepository = saleRepository;
        }
        public async Task<bool> ActivateBusBranding(string id)
        {
            if (id.Trim() != null)
            {
                try
                {
                    var busBranding = await _busBrandingRepository.GetModelByIdAsync(id.Trim());

                    if (busBranding == null)
                        return false;
                    busBranding.IsDeleted = false;
                    busBranding.IsModified = true;
                    busBranding.LastModified = DateTime.Now;
                    busBranding.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    busBranding.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _busBrandingRepository.UpdateAsync(busBranding);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Approve(string id)
        {
            if (id.Trim() != null)
            {
                try
                {
                    var busBranding = await _busBrandingRepository.GetModelByIdAsync(id.Trim());

                    if (busBranding == null)
                        return false;
                    busBranding.IsApprove = true;
                    busBranding.IsModified = true;
                    busBranding.LastModified = DateTime.Now;
                    busBranding.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    busBranding.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                    var sale = new Sales();
                    sale.UnitPrice = Convert.ToDecimal(busBranding.Amount);
                    sale.Quantity = 1;
                    sale.TotalAmount = Convert.ToDecimal(busBranding.Amount) * 1;
                    sale.Description = $"Branding of Bus by {busBranding.PartnerName} from {busBranding.BrandStartDate.ToString().Substring(0,9)} to {busBranding.BrandEndDate.ToString().Substring(0, 9)}";
                    sale.CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    sale.CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                    sale.DateCreated = DateTime.Now;

                    var saleResponse = await _saleRepository.CreateAsync(sale);
                    // Update other properties as needed
                    await _busBrandingRepository.UpdateAsync(busBranding);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(BusBrandingRequestModel model)
        {
            if (model != null)
            {
                var busBranding = new BusBranding
                {

                    PartnerName = !string.IsNullOrWhiteSpace(model.PartnerName.Trim()) ? model.PartnerName.Trim() : null,
                    Reciept = !string.IsNullOrWhiteSpace(model.Reciept.Trim()) ? model.Reciept.Trim() : null,
                    NumberOfVehicle = model.NumberOfVehicle > 0 ? model.NumberOfVehicle : 1,
                    Amount = model.Amount > 0 ? model.Amount : 0,
                    BrandStartDate = model.BrandStartDate != null ? model.BrandStartDate : DateTime.Now,
                    BrandEndDate = model.BrandEndDate != null ? model.BrandEndDate : DateTime.Now,
                    IsActive = true,
                    OperationType = model.OperationType != null && (int)model.OperationType > 0? model.OperationType : OperationType.Unknown,
                    VehicleType = model.VehicleType != null && (int)model.VehicleType > 0? model.VehicleType : VehicleType.Unknown,
                    IsApprove = false,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now
                };

                var result = await _busBrandingRepository.CreateAsync(busBranding);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if terminal creation fails
                    Console.WriteLine($"There is error creating bus branding");
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
                    var busBranding = await _busBrandingRepository.GetModelByIdAsync(id.Trim());

                    if (busBranding == null)
                        return false;
                    busBranding.IsDeleted = true;
                    busBranding.DeletedDate = DateTime.Now;
                    busBranding.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    busBranding.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _busBrandingRepository.UpdateAsync(busBranding);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<BusBrandingResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var busBranding = await _busBrandingRepository.GetByIdAsync(id.Trim());
                if (busBranding == null) return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"bus branding having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"bus branding having id {id} retrieved successfully",
                        Data = busBranding.Select(x => ReturnBusBrandingResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<BusBrandingResponseModel>>> GetAllBusBrandings(CancellationToken cancellationToken = default)
        {
            try
            {
                var busBranding = await _busBrandingRepository.GetAllAsync(cancellationToken);

                if (busBranding != null)
                {
                    return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Bus Branding retrieved successfully",
                        Data = busBranding.Select(x => ReturnBusBrandingResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Bus Branding failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving Bus Branding: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<BusBrandingResponseModel>>> GetInactiveBusBranding(CancellationToken cancellationToken = default)
        {
            try
            {
                var busBranding = await _busBrandingRepository.InactiveBranding(cancellationToken);

                if (busBranding != null)
                {
                    return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Bus branding retrieved successfully",
                        Data = busBranding.Select(x => ReturnBusBrandingResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Bus Branding failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving branding: {ex.Message}",
                };
            }
        }

        public async Task MarkExpiredBrandAsDeleted()
        {
            var models = await _busBrandingRepository.MarkExpiredBrandAsDeleted();
            foreach (var model in models)
            {
                model.IsDeleted = true;
            }
            await _busBrandingRepository.UpdateAsync(new BusBranding());
        }

        public async Task<BaseResponse<IEnumerable<BusBrandingResponseModel>>> SearchBusBranding(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var busBranding = await _busBrandingRepository.SearchBranding(keyword,cancellationToken);

                if (busBranding != null)
                {
                    return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Bus branding retrieved successfully",
                        Data = busBranding.Select(x => ReturnBusBrandingResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Bus Branding failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving branding: {ex.Message}",
                };
            }
        }

        public async  Task<BaseResponse<IEnumerable<BusBrandingResponseModel>>> UnApprovedBranding(CancellationToken cancellationToken = default)
        {
            try
            {
                var busBranding = await _busBrandingRepository.UnApprovedBranding(cancellationToken);

                if (busBranding != null)
                {
                    return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Bus Branding retrieved successfully",
                        Data = busBranding.Select(x => ReturnBusBrandingResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Bus Branding failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<BusBrandingResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving Bus Branding: {ex.Message}",
                };
            }
        }

        public async  Task<bool> UpdateBusBranding(UpdateBusBrandingRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var busBranding = await _busBrandingRepository.GetModelByIdAsync(updateModel.Id.Trim());
                    // var role = updateModel.RoleName != null ? await _roleManager.FindByNameAsync(updateModel.RoleName.Trim()) : null;

                    if (busBranding == null)
                        return false;

                    // Update user properties based on your model
                    busBranding.NumberOfVehicle = updateModel.NumberOfVehicle != null ? updateModel.NumberOfVehicle.Value : busBranding.NumberOfVehicle;
                    busBranding.BrandStartDate = updateModel.BrandStartDate != null ? updateModel.BrandStartDate.Value : busBranding.BrandStartDate;
                    busBranding.BrandEndDate = updateModel.BrandEndDate != null ? updateModel.BrandEndDate.Value : busBranding.BrandEndDate;
                    busBranding.OperationType = updateModel.OperationType != null && (int)updateModel.OperationType.Value > 0 ? updateModel.OperationType.Value : busBranding.OperationType;
                    busBranding.VehicleType = updateModel.VehicleType != null && (int)updateModel.VehicleType.Value > 0 ? updateModel.VehicleType.Value : busBranding.VehicleType;
                    busBranding.Reciept = !string.IsNullOrWhiteSpace(updateModel.Reciept.Trim()) ? updateModel.Reciept.Trim() : busBranding.Reciept;
                    busBranding.IsModified = true;
                    busBranding.LastModified = DateTime.Now;
                    busBranding.Amount = updateModel.Amount != null ? updateModel.Amount.Value : busBranding.Amount;
                    busBranding.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    busBranding.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;


                    // Update other properties as needed
                    await _busBrandingRepository.UpdateAsync(busBranding);
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
        private BusBrandingResponseModel ReturnBusBrandingResponseModel(BusBranding model)
        {
            return new BusBrandingResponseModel
            {
                Id = model.Id,
                PartnerName = model.PartnerName,
                 NumberOfVehicle = model.NumberOfVehicle,
               BrandStartDate = model.BrandStartDate,
                BrandEndDate = model.BrandEndDate,
                IsActive = model.IsActive,
                OperationType = model.OperationType,
                VehicleType = model.VehicleType,
                 Reciept = model.Reciept,
                 Amount = model.Amount,
                  IsApprove = model.IsApprove,
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
