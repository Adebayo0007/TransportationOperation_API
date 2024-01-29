using Microsoft.AspNetCore.Http;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Leave;
using PTS_CORE.Domain.Entities;
using PTS_DATA.Repository.Interfaces;
using System.Security.Claims;

namespace PTS_BUSINESS.Services.Implementations
{
    public class LeaveService : ILeaveService
    {
       
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILeaveRepository _leaveRepository;

        public LeaveService(ILeaveRepository leaveRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _leaveRepository = leaveRepository;
        }
        public async Task<bool> ActivateLeave(string id)
        {
            if (!string.IsNullOrWhiteSpace(id.Trim()))
            {
                try
                {
                    var leave = await _leaveRepository.GetModelByIdAsync(id.Trim());

                    if (leave == null)
                        return false;

                    // Delete user properties based on your model
                    leave.IsDeleted = false;
                    leave.LastModified = DateTime.Now;
                    leave.IsDeleted = false;
                    leave.IsModified = true;
                    leave.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    leave.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                    await _leaveRepository.UpdateAsync(leave);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateLeaveRequestModel model)
        {
            if (model != null)
            {
                var leave = new Leave
                {
                    Subject = !string.IsNullOrWhiteSpace(model.Subject.Trim()) ? model.Subject.Trim() : null,
                    LeaveType = model.LeaveType,
                    ReasonForLeave = !string.IsNullOrWhiteSpace(model.ReasonForLeave.Trim()) ? model.ReasonForLeave.Trim() : null,
                    ReasonForDenial = "Not provided",
                    Startdate = model.Startdate != null ? model.Startdate : DateTime.Now,
                    Enddate = model.Enddate != null ? model.Enddate : DateTime.Now,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now
                };

                var result = await _leaveRepository.CreateAsync(leave);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if terminal creation fails
                    Console.WriteLine($"There is error creating leave");
                    return false;
                }
            }
            else
                return false;
        }

        public async Task Delete(string id)
        {

            if (id != null)
            {
                var leave = await _leaveRepository.GetModelByIdAsync(id.Trim());
                leave.IsDeleted = true;
                await _leaveRepository.DeleteAsync();
            }
        }

        public async Task<BaseResponse<IEnumerable<LeaveResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var leave = await _leaveRepository.GetByIdAsync(id.Trim());
                if (leave == null) return new BaseResponse<IEnumerable<LeaveResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"leave having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<LeaveResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"leave having id {id} retrieved successfully",
                        Data = leave.Select(x => ReturnLeaveResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<LeaveResponseModel>>> GetAllLeaves(CancellationToken cancellationToken = default)
        {
            try
            {
                var leave = await _leaveRepository.GetAllAsync(cancellationToken);
                if (leave == null) return new BaseResponse<IEnumerable<LeaveResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"leave not found"
                };
                else
                    return new BaseResponse<IEnumerable<LeaveResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"leave retrieved successfully",
                        Data = leave.Select(x => ReturnLeaveResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<LeaveResponseModel>>> GetInactiveLeaves(CancellationToken cancellationToken = default)
        {
            try
            {
                var leave = await _leaveRepository.InactiveLeaves(cancellationToken);
                if (leave == null) return new BaseResponse<IEnumerable<LeaveResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"leave not found"
                };
                else
                    return new BaseResponse<IEnumerable<LeaveResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"leave retrieved successfully",
                        Data = leave.Select(x => ReturnLeaveResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task IsDenied(string id)
        {
            if (id != null)
            {
                var leave = await _leaveRepository.GetModelByIdAsync(id.Trim());
                leave.IsGranted = false;
                leave.IsDenied = true;
                leave.IsModified = true;
                leave.LastModified = DateTime.Now;
                leave.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                leave.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                await _leaveRepository.UpdateAsync(leave);
            }
        }

        public async Task IsGranted(string id)
        {
            if (id != null)
            {
                var leave = await _leaveRepository.GetModelByIdAsync(id.Trim());
                leave.IsDenied = false;
                leave.IsGranted = true;
                leave.IsModified = true;
                leave.LastModified = DateTime.Now;
                leave.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                leave.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                await _leaveRepository.UpdateAsync(leave);
            }
        }

        public async Task IsOpened(string id)
        {
            if (id != null)
            {
                var leave = await _leaveRepository.GetModelByIdAsync(id.Trim());
                leave.IsOpened = true;
                leave.IsModified = true;
                leave.LastModified = DateTime.Now;
                leave.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                leave.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                await _leaveRepository.UpdateAsync(leave);
            }
        }

        public async Task<BaseResponse<IEnumerable<LeaveResponseModel>>> SearchLeaves(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var leave = await _leaveRepository.SearchLeaves(keyword,cancellationToken);
                if (leave == null) return new BaseResponse<IEnumerable<LeaveResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"leave not found"
                };
                else
                    return new BaseResponse<IEnumerable<LeaveResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"leave retrieved successfully",
                        Data = leave.Select(x => ReturnLeaveResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Update(UpdateLeaveRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var leave = await _leaveRepository.GetModelByIdAsync(updateModel.Id.Trim());

                    if (leave == null)
                        return false;

                    // Update user properties based on your model
                    leave.Subject = !string.IsNullOrWhiteSpace(updateModel.Subject.Trim()) ? updateModel.Subject.Trim() : leave.Subject;
                    leave.ReasonForLeave = updateModel.ReasonForLeave.Trim() != null ? updateModel.ReasonForLeave.Trim() : leave.ReasonForLeave;
                    leave.ReasonForDenial = updateModel.ReasonForDenial.Trim() != null || updateModel.ReasonForDenial != " " || updateModel.ReasonForDenial != "" ? updateModel.ReasonForDenial.Trim() : leave.ReasonForDenial;
                    leave.LeaveType = (int)updateModel.LeaveType > 0 ? updateModel.LeaveType.Value : leave.LeaveType;
                    leave.Startdate = updateModel.Startdate != null && updateModel.Startdate != DateTime.MinValue? updateModel.Startdate.Value : leave.Startdate;
                    leave.Enddate = updateModel.Enddate != null && updateModel.Enddate != DateTime.MinValue ? updateModel.Enddate.Value : leave.Enddate;
                    leave.IsModified = true;
                    leave.LastModified = DateTime.Now;
                    leave.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    leave.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _leaveRepository.UpdateAsync(leave);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        private LeaveResponseModel ReturnLeaveResponseModel(Leave model)
        {
            return new LeaveResponseModel
            {
               Id = model.Id,
               Subject = model.Subject,
               LeaveType = model.LeaveType,
               ReasonForLeave = model.ReasonForLeave,
               Startdate = model.Startdate,
               Enddate = model.Enddate,
               IsOpened = model.IsOpened,
               IsGranted = model.IsGranted,
               IsDenied = model.IsDenied,
               ReasonForDenial = model.ReasonForDenial,
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
