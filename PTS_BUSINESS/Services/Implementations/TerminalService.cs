using Microsoft.AspNetCore.Http;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Terminal;
using PTS_CORE.Domain.Entities;
using PTS_DATA.Repository.Interfaces;
using System.Security.Claims;

namespace PTS_BUSINESS.Services.Implementations
{
    public class TerminalService : ITerminalService
    {
        private readonly ITerminalRepository _terminalRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TerminalService(ITerminalRepository terminalRepository,
             IHttpContextAccessor httpContextAccessor)
        {
            _terminalRepository = terminalRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> ActivateTerminal(string terminalId)
        {
            if (terminalId != null)
            {
                try
                {
                    var terminal = await _terminalRepository.GetModelByIdAsync(terminalId.Trim());

                    if (terminal == null)
                        return false;
                    terminal.IsDeleted = false;
                    terminal.IsModified = true;
                    terminal.LastModified = DateTime.Now;
                    terminal.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    terminal.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _terminalRepository.UpdateAsync(terminal);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateTerminalRequestModel model)
        {
            if (model != null)
            {
                var terminal = new Terminal
                {

                    Name = !string.IsNullOrWhiteSpace(model.Name.Trim()) ? model.Name.Trim() : null,
                    Code = !string.IsNullOrWhiteSpace(model.Code.Trim()) ? model.Code.Trim() : null,
                    Address = !string.IsNullOrWhiteSpace(model.Address.Trim()) ? model.Address.Trim() : null,
                    ContactPerson = !string.IsNullOrWhiteSpace(model.ContactPerson.Trim()) ? model.ContactPerson.Trim() : null,
                    ContactPerson2 = !string.IsNullOrWhiteSpace(model.ContactPerson2.Trim()) ? model.ContactPerson2.Trim() : null,
                    State = !string.IsNullOrWhiteSpace(model.State.Trim()) ? model.State.Trim() : null,
                    Longitude = model.Longitude.ToString().Length > 1 ? model.Longitude : 0,
                    Latitude = model.Latitude.ToString().Length > 1 ? model.Latitude : 0,
                    TerminalStartingDate = model.TerminalStartingDate != null ? model.TerminalStartingDate : DateTime.Now,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now,
                    IsCommission = true
                };

                var result = await _terminalRepository.CreateAsync(terminal);

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

        public async Task<bool> Delete(string terminalId)
        {
            if (terminalId != null)
            {
                try
                {
                    var terminal = await _terminalRepository.GetModelByIdAsync(terminalId.Trim());

                    if (terminal == null)
                        return false;
                    terminal.IsDeleted = true;
                    terminal.DeletedDate = DateTime.Now;
                    terminal.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    terminal.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _terminalRepository.UpdateAsync(terminal);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<BaseResponse<IEnumerable<TerminalResponseModel>>> Get(string terminalId)
        {
            try
            {

                if (terminalId == null) { throw new NullReferenceException(); }
                var vehicle = await _terminalRepository.GetByIdAsync(terminalId.Trim());
                if (vehicle == null) return new BaseResponse<IEnumerable<TerminalResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"terminal having  id {terminalId} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<TerminalResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"terminal having id {terminalId} retrieved successfully",
                        Data = vehicle.Select(x => ReturnTerminalResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<TerminalResponseModel>>> GetAllTerminals(CancellationToken cancellationToken = default)
        {
            try
            {
                var terminals = await _terminalRepository.GetAllAsync(cancellationToken);

                if (terminals != null)
                {
                    return new BaseResponse<IEnumerable<TerminalResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Terminal retrieved successfully",
                        Data = terminals.Select(x => ReturnTerminalResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<TerminalResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Terminals failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<TerminalResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving terminals: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<TerminalResponseModel>>> GetInactiveTerminals(CancellationToken cancellationToken = default)
        {
            try
            {
                var terminals = await _terminalRepository.InactiveTerminal(cancellationToken);

                if (terminals != null)
                {
                    return new BaseResponse<IEnumerable<TerminalResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Terminal retrieved successfully",
                        Data = terminals.Select(x => ReturnTerminalResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<TerminalResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Terminals failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<TerminalResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving terminals: {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<TerminalResponseModel>>> SearchTerminal(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var terminals = await _terminalRepository.SearchTerminal(keyword,cancellationToken);

                if (terminals != null)
                {
                    return new BaseResponse<IEnumerable<TerminalResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Terminal retrieved successfully",
                        Data = terminals.Select(x => ReturnTerminalResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<TerminalResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Terminals failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<TerminalResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving terminals: {ex.Message}",
                };
            }
        }

        public async Task<bool> UpdateTerminal(UpdateTerminalRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var terminal = await _terminalRepository.GetModelByIdAsync(updateModel.Id.Trim());
                    // var role = updateModel.RoleName != null ? await _roleManager.FindByNameAsync(updateModel.RoleName.Trim()) : null;

                    if (terminal == null)
                        return false;

                    // Update user properties based on your model
                    terminal.Name = !string.IsNullOrWhiteSpace(updateModel.Name.Trim()) ? updateModel.Name.Trim() : terminal.Name;
                    terminal.Code = !string.IsNullOrWhiteSpace(updateModel.Code.Trim()) ? updateModel.Code.Trim() : terminal.Code;
                    terminal.Address = !string.IsNullOrWhiteSpace(updateModel.Address.Trim()) ? updateModel.Address.Trim() : terminal.Address;
                    terminal.ContactPerson = !string.IsNullOrWhiteSpace(updateModel.ContactPerson.Trim()) ? updateModel.ContactPerson.Trim() : terminal.ContactPerson;
                    terminal.ContactPerson2 = !string.IsNullOrWhiteSpace(updateModel.ContactPerson2.Trim()) ? updateModel.ContactPerson2.Trim() : terminal.ContactPerson2;
                   // terminal.State = !string.IsNullOrWhiteSpace(updateModel.State.Trim()) ? updateModel.State.Trim() : terminal.State;
                    terminal.Longitude = updateModel.Longitude > 0 ? updateModel.Longitude.Value : terminal.Longitude;
                    terminal.Latitude = updateModel.Latitude > 0 ? updateModel.Latitude.Value : terminal.Latitude;
                    terminal.TerminalStartingDate = updateModel.TerminalStartingDate!= null && updateModel.TerminalStartingDate.Value != DateTime.MinValue ? updateModel.TerminalStartingDate.Value : terminal.TerminalStartingDate;
                   // terminal.IsCommission = updateModel.IsCommission.Value != null ? updateModel.IsCommission.Value : terminal.IsCommission;
                    terminal.IsModified = true;
                    terminal.LastModified = DateTime.Now;
                    terminal.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    terminal.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _terminalRepository.UpdateAsync(terminal);
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
        private TerminalResponseModel ReturnTerminalResponseModel(Terminal model)
        {
            return new TerminalResponseModel
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code,
                Address = model.Address,
                ContactPerson = model.ContactPerson,
                ContactPerson2 = model.ContactPerson2,
                Longitude = model.Longitude,
                Latitude = model.Latitude,
                TerminalStartingDate = model.TerminalStartingDate,
                State = model.State,
                IsCommission = model.IsCommission,
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
