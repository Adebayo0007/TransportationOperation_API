using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Employee;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Vehicle;
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
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VehicleService(IVehicleRepository vehicleRepository,
             IHttpContextAccessor httpContextAccessor)
        {
            _vehicleRepository = vehicleRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> ActivateVehicle(string vehicleId)
        {
            if (vehicleId != null)
            {
                try
                {
                    var vehicle = await _vehicleRepository.GetModelByIdAsync(vehicleId.Trim());

                    if (vehicle == null)
                        return false;
                    vehicle.IsDeleted = false;
                    vehicle.IsModified = true;
                    vehicle.LastModified = DateTime.Now;
                    vehicle.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    vehicle.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _vehicleRepository.UpdateAsync(vehicle);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateVehicleRequestModel model)
        {
            if (model != null)
            {
                var vehicle = new Vehicle
                {

                    Name = !string.IsNullOrWhiteSpace(model.Name.Trim()) ? model.Name.Trim() : null,
                    DriverId = !string.IsNullOrWhiteSpace(model.DriverId.Trim()) ? model.DriverId.Trim() : null,
                    TerminalId = !string.IsNullOrWhiteSpace(model.DriverId.Trim()) ? model.TerminalId.Trim() : null,
                    OperationType = model.OperationType,
                    VehicleStatus = model.VehicleStatus,
                    RegistrationNumber = !string.IsNullOrWhiteSpace(model.RegistrationNumber.Trim()) ? model.Name.Trim() : null,
                    EngineNumber = !string.IsNullOrWhiteSpace(model.EngineNumber.Trim()) ? model.EngineNumber.Trim() : null,
                    IMEINumber = !string.IsNullOrWhiteSpace(model.IMEINumber.Trim()) ? model.IMEINumber.Trim() : null,
                    VehicleModel = !string.IsNullOrWhiteSpace(model.VehicleModel.Trim()) ? model.VehicleModel.Trim() : null,
                    NumberOfSeat = model.NumberOfSeat > 0 ? model.NumberOfSeat : 0,
                    PurchaseDate = model.PurchaseDate != null ? model.PurchaseDate : DateTime.Now,
                    LicenseDate = model.LicenseDate != null ? model.LicenseDate.Value : null,
                    LicenseExpirationDate = model.LicenseExpirationDate != null ? model.LicenseExpirationDate.Value : null,
                    InsuranceDate = model.InsuranceDate != null ? model.InsuranceDate.Value : null,
                    RoadWorthinessDate = model.RoadWorthinessDate != null ? model.RoadWorthinessDate.Value : null,
                    RoadWorthinessExpirationDate = model.RoadWorthinessExpirationDate  != null ? model.RoadWorthinessExpirationDate.Value : null,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now,
                };

                var result = await _vehicleRepository.CreateAsync(vehicle);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if vehicle creation fails
                    Console.WriteLine($"There is error creating vehicle");
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
                    var vehicle = await _vehicleRepository.GetModelByIdAsync(id.Trim());

                    if (vehicle == null)
                        return false;
                    vehicle.IsDeleted = true;
                    vehicle.DeletedDate = DateTime.Now;
                    vehicle.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    vehicle.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _vehicleRepository.UpdateAsync(vehicle);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<VehicleResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var vehicle = await _vehicleRepository.GetByIdAsync(id.Trim());
                if (vehicle == null) return new BaseResponse<IEnumerable<VehicleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"vehicle having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<VehicleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"vehicle having id {id} retrieved successfully",
                        Data = vehicle.Select(x => ReturnVehicleResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<VehicleResponseModel>>> GetAllVehicles(CancellationToken cancellationToken = default)
        {
            try
            {
                var vehicles = await _vehicleRepository.GetAllAsync(cancellationToken);

                if (vehicles != null)
                {
                    return new BaseResponse<IEnumerable<VehicleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Vehicles retrieved successfully",
                        Data = vehicles.Select(x => ReturnVehicleResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<VehicleResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Vehicles failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<VehicleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving vehicles: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<VehicleResponseModel>>> GetInactiveVehicle(CancellationToken cancellationToken = default)
        {
            try
            {
                var vehicles = await _vehicleRepository.InactiveVehicle(cancellationToken);

                if (vehicles != null)
                {
                    return new BaseResponse<IEnumerable<VehicleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Vehicles retrieved successfully",
                        Data = vehicles.Select(x => ReturnVehicleResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<VehicleResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Vehicles failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<VehicleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving vehicles: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<VehicleResponseModel>>> GetTerminalVehicle(string terminalId,CancellationToken cancellationToken = default)
        {
            try
            {
                var vehicles = await _vehicleRepository.GetTerminalVehicles(terminalId,cancellationToken);

                if (vehicles != null)
                {
                    return new BaseResponse<IEnumerable<VehicleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Vehicles retrieved successfully",
                        Data = vehicles.Select(x => ReturnVehicleResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<VehicleResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Vehicles failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<VehicleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving vehicles: {ex.Message}",
                };
            }
        }

        public async Task<bool> UpdateVehicle(UpdateVehicleRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var vehicle = await _vehicleRepository.GetModelByIdAsync(updateModel.Id.Trim());
                   // var role = updateModel.RoleName != null ? await _roleManager.FindByNameAsync(updateModel.RoleName.Trim()) : null;

                    if (vehicle == null)
                        return false;

                    // Update user properties based on your model
                    vehicle.Name = !string.IsNullOrWhiteSpace(updateModel.Name.Trim()) ? updateModel.Name.Trim() : vehicle.Name;
                    vehicle.DriverId = !string.IsNullOrWhiteSpace(updateModel.DriverId.Trim()) ? updateModel.DriverId.Trim() : vehicle.DriverId;
                    vehicle.TerminalId = !string.IsNullOrWhiteSpace(updateModel.TerminalId.Trim()) ? updateModel.TerminalId.Trim() : vehicle.TerminalId;
                    vehicle.OperationType = updateModel.OperationType != null ? updateModel.OperationType : vehicle.OperationType;
                    vehicle.VehicleStatus = updateModel.VehicleStatus != null ? updateModel.VehicleStatus : vehicle.VehicleStatus;
                    vehicle.RegistrationNumber = !string.IsNullOrWhiteSpace(updateModel.RegistrationNumber.Trim()) ? updateModel.RegistrationNumber.Trim() : vehicle.RegistrationNumber;
                    vehicle.EngineNumber = !string.IsNullOrWhiteSpace(updateModel.EngineNumber.Trim()) ? updateModel.EngineNumber.Trim() : vehicle.EngineNumber;
                    vehicle.IMEINumber = !string.IsNullOrWhiteSpace(updateModel.IMEINumber.Trim()) ? updateModel.IMEINumber.Trim() : vehicle.IMEINumber;
                    vehicle.VehicleModel = !string.IsNullOrWhiteSpace(updateModel.VehicleModel.Trim()) ? updateModel.VehicleModel.Trim() : vehicle.VehicleModel;
                    vehicle.NumberOfSeat = updateModel.NumberOfSeat.Value > 0 ? updateModel.NumberOfSeat.Value : vehicle.NumberOfSeat;
                    vehicle.LicenseDate = updateModel.LicenseDate.Value != null ? updateModel.LicenseDate.Value : vehicle.LicenseDate;
                    vehicle.LicenseExpirationDate = updateModel.LicenseExpirationDate.Value != null ? updateModel.LicenseExpirationDate.Value : vehicle.LicenseExpirationDate;
                    vehicle.InsuranceDate = updateModel.InsuranceDate.Value != null ? updateModel.InsuranceDate.Value : vehicle.InsuranceDate;
                    vehicle.RoadWorthinessDate = updateModel.RoadWorthinessDate.Value != null ? updateModel.RoadWorthinessDate.Value : vehicle.RoadWorthinessDate;
                    vehicle.RoadWorthinessExpirationDate = updateModel.RoadWorthinessExpirationDate.Value != null ? updateModel.RoadWorthinessExpirationDate.Value : vehicle.RoadWorthinessExpirationDate;
                    vehicle.IsModified = true;
                    vehicle.LastModified = DateTime.Now;
                    vehicle.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    vehicle.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _vehicleRepository.UpdateAsync(vehicle);
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
        private VehicleResponseModel ReturnVehicleResponseModel(Vehicle model)
        {
            return new VehicleResponseModel
            {
                    Id = model.Id,
                    Name = model.Name,
                    DriverId = model.DriverId,
                    TerminalId = model.TerminalId,
                    OperationType = model.OperationType.Value,
                    VehicleStatus = model.VehicleStatus.Value,
                    RegistrationNumber = model.RegistrationNumber,
                    EngineNumber = model.RegistrationNumber,
                    IMEINumber = model.IMEINumber,
                    VehicleModel = model.VehicleModel,
                    NumberOfSeat = model.NumberOfSeat,
                    PurchaseDate = model.PurchaseDate,
                    LicenseDate = model.LicenseDate,
                    LicenseExpirationDate = model.LicenseExpirationDate,
                    InsuranceDate = model.InsuranceDate,
                    RoadWorthinessDate = model.RoadWorthinessDate,
                   RoadWorthinessExpirationDate = model.RoadWorthinessExpirationDate,
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
