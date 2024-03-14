using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Fuel;
using Microsoft.AspNetCore.Http;
using PTS_DATA.Repository.Interfaces;
using System.Security.Claims;
using PTS_CORE.Domain.Entities;

namespace PTS_BUSINESS.Services.Implementations
{
    public class FuelService : IFuelService
    {
        private readonly IFuelRepository _fuelRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FuelService(IFuelRepository fuelRepository,
             IHttpContextAccessor httpContextAccessor)
        {
            _fuelRepository = fuelRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> ActivateFuel(string id)
        {
            if (id.Trim() != null)
            {
                try
                {
                    var fuel = await _fuelRepository.GetModelByIdAsync(id.Trim());

                    if (fuel == null)
                        return false;
                    fuel.IsDeleted = false;
                    fuel.IsModified = true;
                    fuel.LastModified = DateTime.Now;
                    fuel.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    fuel.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _fuelRepository.UpdateAsync(fuel);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateFuelRequestModel model)
        {

            if (model != null)
            {
                var fuel = new Fuel
                {

                    VehicleRegNumber = !string.IsNullOrWhiteSpace(model.VehicleRegNumber.Trim()) ? model.VehicleRegNumber.Trim() : null,
                    Quantity = model.Quantity > 0 ? model.Quantity : 1,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now
                };

                var result = await _fuelRepository.CreateAsync(fuel);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if terminal creation fails
                    Console.WriteLine($"There is error creating fuel");
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
                    var fuel = await _fuelRepository.GetModelByIdAsync(id.Trim());

                    if (fuel == null)
                        return false;
                    fuel.IsDeleted = true;
                    fuel.DeletedDate = DateTime.Now;
                    fuel.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    fuel.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _fuelRepository.UpdateAsync(fuel);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<FuelResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var fuel = await _fuelRepository.GetByIdAsync(id.Trim());
                if (fuel == null) return new BaseResponse<IEnumerable<FuelResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"fuel having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<FuelResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"fuel having id {id} retrieved successfully",
                        Data = fuel.Select(x => ReturnFuelResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<FuelResponseModel>>> GetAllFuels(CancellationToken cancellationToken = default)
        {
            try
            {
                var fuels = await _fuelRepository.GetAllAsync(cancellationToken);

                if (fuels != null)
                {
                    return new BaseResponse<IEnumerable<FuelResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Fuel retrieved successfully",
                        Data = fuels.Select(x => ReturnFuelResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<FuelResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Fuel failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<FuelResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving fuels: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<FuelResponseModel>>> GetInactiveFuels(CancellationToken cancellationToken = default)
        {
            try
            {
                var fuels = await _fuelRepository.InactiveFuels(cancellationToken);

                if (fuels != null)
                {
                    return new BaseResponse<IEnumerable<FuelResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Fuel retrieved successfully",
                        Data = fuels.Select(x => ReturnFuelResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<FuelResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Fuel failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<FuelResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving fuels: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<FuelResponseModel>>> SearchFuels(DateTime? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var fuels = await _fuelRepository.SearchFuels(keyword,cancellationToken);

                if (fuels != null)
                {
                    return new BaseResponse<IEnumerable<FuelResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Fuel retrieved successfully",
                        Data = fuels.Select(x => ReturnFuelResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<FuelResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Fuel failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<FuelResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving fuels: {ex.Message}",
                };
            }
        }

        public async Task<bool> UpdateFuel(UpdateFuelRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var fuel = await _fuelRepository.GetModelByIdAsync(updateModel.Id.Trim());
                    // var role = updateModel.RoleName != null ? await _roleManager.FindByNameAsync(updateModel.RoleName.Trim()) : null;

                    if (fuel == null)
                        return false;

                    // Update user properties based on your model
                    fuel.VehicleRegNumber = !string.IsNullOrWhiteSpace(updateModel.VehicleRegNumber.Trim()) ? updateModel.VehicleRegNumber.Trim() : fuel.VehicleRegNumber;
                    fuel.Quantity = updateModel.Quantity != null && updateModel.Quantity.Value > 0 ? updateModel.Quantity.Value : fuel.Quantity;
                    fuel.IsModified = true;
                    fuel.LastModified = DateTime.Now;
                    fuel.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    fuel.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;


                    // Update other properties as needed
                    await _fuelRepository.UpdateAsync(fuel);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        private FuelResponseModel ReturnFuelResponseModel(Fuel model)
        {
            return new FuelResponseModel
            {
                Id = model.Id,
                VehicleRegNumber = model.VehicleRegNumber,
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
