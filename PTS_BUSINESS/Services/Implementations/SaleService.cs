using Microsoft.AspNetCore.Http;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Sale;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;
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
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentalSaleRepository _departmentalSaleRepository;
        public SaleService(ISaleRepository saleRepository,
             IHttpContextAccessor httpContextAccessor,
             IEmployeeRepository employeeRepository,
             IDepartmentalSaleRepository departmentalSaleRepository)
        {
            _saleRepository = saleRepository;
            _httpContextAccessor = httpContextAccessor;
            _employeeRepository = employeeRepository;
            _departmentalSaleRepository = departmentalSaleRepository;
        }
        public async Task<bool> ActivateSale(string id)
        {
            if (id.Trim() != null)
            {
                try
                {
                    var sale = await _saleRepository.GetModelByIdAsync(id.Trim());

                    if (sale == null)
                        return false;
                    sale.IsDeleted = false;
                    sale.IsModified = true;
                    sale.LastModified = DateTime.Now;
                    sale.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    sale.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _saleRepository.UpdateAsync(sale);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateSaleRequestModel model)
        {
            if (model != null)
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var employee = await _employeeRepository.GetModelByUserIdAsync(userId);
                var departmentalSale = await _departmentalSaleRepository.GetModelByDepartmentIdAsync(employee.DepartmentId);
                if (departmentalSale != null) departmentalSale.ActualAmount += Convert.ToDecimal(model.Quantity * model.UnitPrice);
                await _departmentalSaleRepository.UpdateAsync(departmentalSale);
                var sale = new Sales
                {
                    Quantity = model.Quantity > 0? model.Quantity: 1,
                    UnitPrice = model.UnitPrice > 0? model.UnitPrice: 0,
                    TotalAmount = model.Quantity * model.UnitPrice,
                    Description = !string.IsNullOrWhiteSpace(model.Description.Trim()) ? model.Description.Trim() : null,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now
                };

                var result = await _saleRepository.CreateAsync(sale);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if terminal creation fails
                    Console.WriteLine($"There is error creating sale");
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
                    var sale = await _saleRepository.GetModelByIdAsync(id.Trim());

                    if (sale == null)
                        return false;
                    sale.IsDeleted = true;
                    sale.DeletedDate = DateTime.Now;
                    sale.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    sale.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _saleRepository.UpdateAsync(sale);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<SaleResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var sale = await _saleRepository.GetByIdAsync(id.Trim());
                if (sale == null) return new BaseResponse<IEnumerable<SaleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"sale having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<SaleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"store item having id {id} retrieved successfully",
                        Data = sale.Select(x => ReturnSaleResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<SaleResponseModel>>> GetAllSales(CancellationToken cancellationToken = default)
        {
            try
            {
                var sale = await _saleRepository.GetAllAsync(cancellationToken);

                if (sale != null)
                {
                    return new BaseResponse<IEnumerable<SaleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Sale retrieved successfully",
                        Data = sale.Select(x => ReturnSaleResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<SaleResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Sale failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<SaleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving sale: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<SaleResponseModel>>> GetInactiveSales(CancellationToken cancellationToken = default)
        {
            try
            {
                var sale = await _saleRepository.InactiveSaleRequest(cancellationToken);

                if (sale != null)
                {
                    return new BaseResponse<IEnumerable<SaleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Sale retrieved successfully",
                        Data = sale.Select(x => ReturnSaleResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<SaleResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Sale failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<SaleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving sale: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<SaleResponseModel>>> SearchSales(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var sale = await _saleRepository.SearchSale(keyword,cancellationToken);

                if (sale != null)
                {
                    return new BaseResponse<IEnumerable<SaleResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Sale retrieved successfully",
                        Data = sale.Select(x => ReturnSaleResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<SaleResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Sale failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<SaleResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving sale: {ex.Message}",
                };
            }
        }

        public async Task<bool> Update(UpdateSaleRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var sale = await _saleRepository.GetModelByIdAsync(updateModel.Id.Trim());
                    // var role = updateModel.RoleName != null ? await _roleManager.FindByNameAsync(updateModel.RoleName.Trim()) : null;

                    if (sale == null)
                        return false;

                    sale.Description = !string.IsNullOrWhiteSpace(updateModel.Description.Trim()) ? updateModel.Description.Trim() : sale.Description;
                    sale.Quantity = updateModel.Quantity > 0 ? updateModel.Quantity.Value : sale.Quantity;
                    sale.UnitPrice = updateModel.UnitPrice > 0 ? updateModel.UnitPrice.Value : sale.UnitPrice;
                    sale.IsModified = true;
                    sale.LastModified = DateTime.Now;
                    sale.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    sale.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;


                    // Update other properties as needed
                    await _saleRepository.UpdateAsync(sale);
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
        private SaleResponseModel ReturnSaleResponseModel(Sales model)
        {
            return new SaleResponseModel
            {
                Id = model.Id,
                Quantity = model.Quantity,
                UnitPrice = model.UnitPrice,
                TotalAmount = model.TotalAmount,
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
