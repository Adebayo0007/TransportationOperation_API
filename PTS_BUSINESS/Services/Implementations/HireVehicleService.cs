using Microsoft.AspNetCore.Http;
using PTS_BUSINESS.Common;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.HireVehicle;
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
    public class HireVehicleService : IHireVehicleService
    {
        private readonly IHireVehicleRepository _hireVehicleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISaleRepository _saleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentalSaleRepository _departmentalSaleRepository;
        private readonly IFuelRepository _fuelRepository;
        public HireVehicleService(IHireVehicleRepository hireVehicleRepository,
             IHttpContextAccessor httpContextAccessor,
             ISaleRepository saleRepository,
             IEmployeeRepository employeeRepository,
             IDepartmentalSaleRepository departmentalSaleRepository,
             IFuelRepository fuelRepository)
        {
            _hireVehicleRepository = hireVehicleRepository;
            _httpContextAccessor = httpContextAccessor;
            _saleRepository = saleRepository;
            _employeeRepository = employeeRepository;
            _departmentalSaleRepository = departmentalSaleRepository;
            _fuelRepository = fuelRepository;
        }
        public async  Task<bool> ActivateHireVehicle(string id)
        {
            if (id.Trim() != null)
            {
                try
                {
                    var hiredVehicle = await _hireVehicleRepository.GetModelByIdAsync(id.Trim());

                    if (hiredVehicle == null)
                        return false;
                    hiredVehicle.IsDeleted = false;
                    hiredVehicle.IsModified = true;
                    hiredVehicle.LastModified = DateTime.Now;
                    hiredVehicle.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    hiredVehicle.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _hireVehicleRepository.UpdateAsync(hiredVehicle);
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
                    var hiredVehicle = await _hireVehicleRepository.GetModelByIdAsync(id.Trim());
                    var employee = await _employeeRepository.GetModelByUserIdAsync(hiredVehicle.CreatorId);
                    var departmentalSale = await _departmentalSaleRepository.GetModelByDepartmentIdAsync(employee.DepartmentId);

                    if (hiredVehicle == null)
                        return false;
                    if (departmentalSale != null) departmentalSale.ActualAmount += Convert.ToDecimal(hiredVehicle.Amount);
                    hiredVehicle.IsChairmanApprove = true;
                    hiredVehicle.IsModified = true;
                    hiredVehicle.LastModified = DateTime.Now;
                    hiredVehicle.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    hiredVehicle.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                    var sale = new Sales();
                    sale.UnitPrice = Convert.ToDecimal(hiredVehicle.Amount);
                    sale.Quantity = 1;
                    sale.TotalAmount = Convert.ToDecimal(hiredVehicle.Amount) * 1;
                    sale.Description = $"Hiring of Bus by {hiredVehicle.Customer} from {hiredVehicle.DeapartureDate.ToString().Substring(0, 9)} to {hiredVehicle.ArrivalDate.ToString().Substring(0, 9)}";
                    sale.CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    sale.CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                    sale.DateCreated = DateTime.Now;

                    var saleResponse = await _saleRepository.CreateAsync(sale);

                    // Update other properties as needed
                    await _hireVehicleRepository.UpdateAsync(hiredVehicle);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateHireVehicleRequestModel model)
        {
            if (model != null)
            {
                var hireVehicle = new HireVehicle
                {
                    Customer = !string.IsNullOrWhiteSpace(model.Customer.Trim()) ? model.Customer.Trim() : null,
                    DepartureAddress = !string.IsNullOrWhiteSpace(model.DepartureAddress.Trim()) ? model.DepartureAddress.Trim() : null,
                    DestinationAddress = !string.IsNullOrWhiteSpace(model.DestinationAddress.Trim()) ? model.DestinationAddress.Trim() : null,
                    RecieptAndInvoice = !string.IsNullOrWhiteSpace(model.RecieptAndInvoice.Trim()) ? model.RecieptAndInvoice.Trim() : null,
                    DepartureTerminalName = !string.IsNullOrWhiteSpace(model.DepartureTerminalName.Trim()) ? model.DepartureTerminalName.Trim() : null,
                    DeapartureDate = model.DeapartureDate != null && model.DeapartureDate > DateTime.MinValue ? model.DeapartureDate : DateTime.Now,
                    ArrivalDate = model.ArrivalDate != null && model.ArrivalDate > DateTime.MinValue ? model.ArrivalDate : DateTime.Now,
                    OperationType = model.OperationType != null && (int)model.OperationType > 0? model.OperationType : OperationType.Unknown,
                    Amount = model.Amount != null && model.Amount > 0? model.Amount : 0,
                    Profit = model.Profit != null && model.Profit > 0? model.Profit : 0,
                    Kilometer = model.Kilometer != null && model.Kilometer > 0? model.Kilometer : 1,
                    CostOfExacution = model.CostOfExacution != null && model.CostOfExacution > 0? model.CostOfExacution : 0,
                    Fuel = model.Fuel != null && model.Fuel > 0? model.Fuel : 0,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now
                };

                var result = await _hireVehicleRepository.CreateAsync(hireVehicle);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if terminal creation fails
                    Console.WriteLine($"There is error creating hired vehicle");
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
                    var hireVehicle = await _hireVehicleRepository.GetModelByIdAsync(id.Trim());

                    if (hireVehicle == null)
                        return false;
                    hireVehicle.IsDeleted = true;
                    hireVehicle.DeletedDate = DateTime.Now;
                    hireVehicle.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    hireVehicle.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _hireVehicleRepository.UpdateAsync(hireVehicle);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<HireVehicleResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var hireVehicle = await _hireVehicleRepository.GetByIdAsync(id.Trim());
                if (hireVehicle == null) return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"hire vehicle having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"hire vehicle having id {id} retrieved successfully",
                        Data = hireVehicle.Select(x => ReturnHireVehicleResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<HireVehicleResponseModel>>> GetAllHireVehicles(CancellationToken cancellationToken = default)
        {
            try
            {
                var hireVehicles = await _hireVehicleRepository.GetAllAsync(cancellationToken);

                if (hireVehicles != null)
                {
                    return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"hired vehicles retrieved successfully",
                        Data = hireVehicles.Select(x => ReturnHireVehicleResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Hired Vehicles failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving hired vehicle: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<HireVehicleResponseModel>>> GetInactiveSHireVehicles(CancellationToken cancellationToken = default)
        {
            try
            {
                var hireVehicles = await _hireVehicleRepository.InactiveHiredVeehicle(cancellationToken);

                if (hireVehicles != null)
                {
                    return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"hired vehicles retrieved successfully",
                        Data = hireVehicles.Select(x => ReturnHireVehicleResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Hired Vehicles failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving hired vehicle: {ex.Message}",
                };
            }
        }

        public async Task<bool> ResolvedByDepo(string id)
        {
            if (id.Trim() != null)
            {
                try
                {
                    var hiredVehicle = await _hireVehicleRepository.GetModelByIdAsync(id.Trim());
                    await _fuelRepository.CreateAsync(new Fuel
                    {
                        Quantity = hiredVehicle.Fuel,
                        VehicleRegNumber = hiredVehicle.VehicleId,
                        CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                        CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                        DateCreated = DateTime.Now
                    });

                    if (hiredVehicle == null)
                        return false;
                    hiredVehicle.ResolvedByDepo = true;
                    hiredVehicle.IsModified = true;
                    hiredVehicle.LastModified = DateTime.Now;
                    hiredVehicle.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    hiredVehicle.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _hireVehicleRepository.UpdateAsync(hiredVehicle);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<HireVehicleResponseModel>>> SearchHireVehicles(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var hireVehicles = await _hireVehicleRepository.SearchHiredVehicles(keyword,cancellationToken);

                if (hireVehicles != null)
                {
                    return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"hired vehicles retrieved successfully",
                        Data = hireVehicles.Select(x => ReturnHireVehicleResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Hired Vehicles failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving hired vehicle: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<HireVehicleResponseModel>>> UnApprovedHiring(CancellationToken cancellationToken = default)
        {
            try
            {
                IEnumerable<HireVehicle> hireVehicles = null;
                var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
                if (role == RoleConstant.Chairman || role == RoleConstant.Administrator)
                    hireVehicles = await _hireVehicleRepository.GetAllForChairman(cancellationToken);
                if (role == RoleConstant.HeadOfOperation)
                    hireVehicles = await _hireVehicleRepository.GetAllForOperation(cancellationToken);
                if (role == RoleConstant.HeadOfDepo)
                    hireVehicles = await _hireVehicleRepository.GetAllForDepo(cancellationToken);
               

                if (hireVehicles != null)
                {
                    return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"hired vehicles retrieved successfully",
                        Data = hireVehicles.Select(x => ReturnHireVehicleResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Hired Vehicles failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<HireVehicleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving hired vehicle: {ex.Message}",
                };
            }
        }

        public async Task<bool> UpdateHireVehicle(UpdateHireVehicleRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var hireVehicle = await _hireVehicleRepository.GetModelByIdAsync(updateModel.Id.Trim());
                    // var role = updateModel.RoleName != null ? await _roleManager.FindByNameAsync(updateModel.RoleName.Trim()) : null;

                    if (hireVehicle == null)
                        return false;

                    // Update user properties based on your model
                    hireVehicle.DepartureAddress = !string.IsNullOrWhiteSpace(updateModel.DepartureAddress.Trim()) ? updateModel.DepartureAddress.Trim() : hireVehicle.DepartureAddress;
                    hireVehicle.DestinationAddress = !string.IsNullOrWhiteSpace(updateModel.DestinationAddress.Trim()) ? updateModel.DestinationAddress.Trim() : hireVehicle.DestinationAddress;
                    hireVehicle.VehicleId = !string.IsNullOrWhiteSpace(updateModel.VehicleId.Trim()) ? updateModel.VehicleId.Trim() : hireVehicle.VehicleId;
                    hireVehicle.ResolvedByOperation = !string.IsNullOrWhiteSpace(updateModel.VehicleId.Trim()) ? true : hireVehicle.ResolvedByOperation;
                    hireVehicle.Amount = updateModel.Amount != null && updateModel.Amount.Value > 0 ? updateModel.Amount.Value : hireVehicle.Amount;
                    hireVehicle.Profit = updateModel.Profit != null && updateModel.Profit.Value > 0 ? updateModel.Profit.Value : hireVehicle.Profit;
                    hireVehicle.CostOfExacution = updateModel.CostOfExacution != null && updateModel.CostOfExacution.Value > 0 ? updateModel.CostOfExacution.Value : hireVehicle.CostOfExacution;
                    hireVehicle.Fuel = updateModel.Fuel != null && updateModel.Fuel.Value > 0 ? updateModel.Fuel.Value : hireVehicle.Fuel;
                    hireVehicle.DeapartureDate = updateModel.DeapartureDate != null && updateModel.DeapartureDate.Value != DateTime.MaxValue ? updateModel.DeapartureDate.Value : hireVehicle.DeapartureDate;
                    hireVehicle.ArrivalDate = updateModel.ArrivalDate != null && updateModel.ArrivalDate.Value != DateTime.MaxValue ? updateModel.ArrivalDate.Value : hireVehicle.ArrivalDate;
                    hireVehicle.OperationType = updateModel.OperationType != null && updateModel.OperationType.Value > 0 ? updateModel.OperationType.Value : hireVehicle.OperationType;
                    hireVehicle.IsModified = true;
                    hireVehicle.LastModified = DateTime.Now;
                    hireVehicle.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    hireVehicle.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;


                    // Update other properties as needed
                    await _hireVehicleRepository.UpdateAsync(hireVehicle);
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
        private HireVehicleResponseModel ReturnHireVehicleResponseModel(HireVehicle model)
        {
            return new HireVehicleResponseModel
            {
                Id = model.Id,
                Customer = model.Customer,
                DepartureAddress = model.DepartureAddress,
                DestinationAddress = model.DestinationAddress,
                Amount = model.Amount,
                VehicleId = model.VehicleId,
                Profit = model.Profit,
                CostOfExacution = model.CostOfExacution,
                Fuel = model.Fuel,
                RecieptAndInvoice = model.RecieptAndInvoice,
                DriverUserId = model.DriverUserId,
                DeapartureDate = model.DeapartureDate,
                ArrivalDate = model.ArrivalDate,
                DepartureTerminalName = model.DepartureTerminalName,
                IsChairmanApprove = model.IsChairmanApprove,
                ResolvedByOperation = model.ResolvedByOperation,
                ResolvedByDepo = model.ResolvedByDepo,
                OperationType = model.OperationType,
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
                Kilometer = model.Kilometer
            };
        }
    }
}
