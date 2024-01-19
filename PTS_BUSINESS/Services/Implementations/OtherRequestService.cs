using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.OtherRequest;
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
    public class OtherRequestService : IOtherRequestService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOtherRequestRepository _otherRequestRepository;

        public OtherRequestService(IOtherRequestRepository otherRequestRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _otherRequestRepository = otherRequestRepository;
        }
        public async Task<bool> ActivateOtherRequests(string id)
        {
            if (id.Trim() != null)
            {
                try
                {
                    var otherRequest = await _otherRequestRepository.GetModelByIdAsync(id.Trim());

                    if (otherRequest == null)
                        return false;
                    otherRequest.IsDeleted = false;
                    // Update other properties as needed
                    await _otherRequestRepository.UpdateAsync(otherRequest);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateOtherRequestRequestModel model)
        {
            if (model != null)
            {
                var otherRequest = new OtherRequest
                {

                    Subject = !string.IsNullOrWhiteSpace(model.Subject.Trim()) ? model.Subject.Trim() : null,
                    Content = !string.IsNullOrWhiteSpace(model.Content.Trim()) ? model.Content.Trim() : null,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now
                };

                var result = await _otherRequestRepository.CreateAsync(otherRequest);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if terminal creation fails
                    Console.WriteLine($"There is error creating other request");
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
                try
                {
                    var otherRequest = await _otherRequestRepository.GetModelByIdAsync(id.Trim());
                    otherRequest.IsDeleted = true;

                    // Update other properties as needed
                    await _otherRequestRepository.UpdateAsync(otherRequest);
                }
                catch (Exception ex)
                { throw; }
            }
        }

        public async Task<BaseResponse<IEnumerable<OtherRequestResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var otherRequest = await _otherRequestRepository.GetByIdAsync(id.Trim());
                if (otherRequest == null) return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"other request having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"other request having id {id} retrieved successfully",
                        Data = otherRequest.Select(x => ReturnOtherRequestResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<OtherRequestResponseModel>>> GetAllOtherRequests(CancellationToken cancellationToken = default)
        {
            try
            {
                var otherRequest = await _otherRequestRepository.GetAllAsync(cancellationToken);

                if (otherRequest != null)
                {
                    return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Other request retrieved successfully",
                        Data = otherRequest.Select(x => ReturnOtherRequestResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Other request failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving other request {ex.Message}",
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<OtherRequestResponseModel>>> GetInactiveOtherRequests(CancellationToken cancellationToken = default)
        {
             try
              {
                  var otherRequest = await _otherRequestRepository.InactivatedOtherRequest(cancellationToken);

                  if (otherRequest != null)
                  {
                      return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                      {
                          IsSuccess = true,
                          Message = $"Other request retrieved successfully",
                          Data = otherRequest.Select(x => ReturnOtherRequestResponseModel(x)).ToList()
                      };
                  }
                  else
                  {
                      return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                      {
                          IsSuccess = false,
                          Message = $"Other request failed to retrieve successfully",
                      };
                  }
              }
              catch (Exception ex)
              {
                  // Handle exceptions appropriately
                  return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                  {
                      IsSuccess = false,
                      Message = $"An error occurred while retrieving other request {ex.Message}",
                  };
              }
        }

        public async Task<BaseResponse<IEnumerable<OtherRequestResponseModel>>> SearchOtherRequests(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var otherRequest = await _otherRequestRepository.SearchOtherRequests(keyword,cancellationToken);

                if (otherRequest != null)
                {
                    return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Other request retrieved successfully",
                        Data = otherRequest.Select(x => ReturnOtherRequestResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Other request failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving other request {ex.Message}",
                };
            }
        }
        public async Task<BaseResponse<IEnumerable<OtherRequestResponseModel>>> MyRequest(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                var otherRequest = await _otherRequestRepository.MyRequest(keyword, cancellationToken);

                if (otherRequest != null)
                {
                    return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"Other request retrieved successfully",
                        Data = otherRequest.Select(x => ReturnOtherRequestResponseModel(x)).ToList()
                    };
                }
                else
                {
                    return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"Other request failed to retrieve successfully",
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new BaseResponse<IEnumerable<OtherRequestResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred while retrieving other request {ex.Message}",
                };
            }
        }

        public async Task<bool> Update(UpdateOtheRequestRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var otherRequest = await _otherRequestRepository.GetModelByIdAsync(updateModel.Id.Trim());

                    if (otherRequest == null)
                        return false;

                    // Update user properties based on your model
                    otherRequest.Subject = !string.IsNullOrWhiteSpace(updateModel.Subject.Trim()) ? updateModel.Subject.Trim() : otherRequest.Subject;
                    otherRequest.Content = !string.IsNullOrWhiteSpace(updateModel.Content.Trim()) ? updateModel.Content.Trim() : otherRequest.Content;
                    otherRequest.IsDDPCommented = updateModel.DDPComment != null ? true : false;
                    otherRequest.DDPComment = updateModel.DDPComment != null ? updateModel.DDPComment.Trim() : otherRequest.DDPComment;
                    otherRequest.IsAuditorCommented = updateModel.AuditorComment != null ? true : false;
                    otherRequest.AuditorComment = updateModel.AuditorComment != null ? updateModel.AuditorComment.Trim() : otherRequest.AuditorComment;
                    otherRequest.AvailabilityType = updateModel.AvailabilityType != null ? updateModel.AvailabilityType.Value : AvailabilityType.UnKnown;
                    otherRequest.RequestType = updateModel.RequestType != null ? updateModel.RequestType.Value : RequestType.Unknown;

                    // Update other properties as needed
                    await _otherRequestRepository.UpdateAsync(otherRequest);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }
        public async Task<bool> ChairmanApproving(string id)
        {
            if (id != null)
            {
                try
                {
                    var otherRequest = await _otherRequestRepository.GetModelByIdAsync(id.Trim());

                    if (otherRequest == null)
                        return false;

                    // Update user properties based on your model
                    otherRequest.IsChairmanApproved = true;
                    otherRequest.IsResolved = true;
                    otherRequest.IsVerified = true;

                    // Update other properties as needed
                    await _otherRequestRepository.UpdateAsync(otherRequest);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        private OtherRequestResponseModel ReturnOtherRequestResponseModel(OtherRequest model)
        {
            return new OtherRequestResponseModel
            {
                Id = model.Id,
                Subject = model.Subject,
                Content = model.Content,
                IsChairmanApproved = model.IsChairmanApproved,
                RequestType = model.RequestType,
                IsDDPCommented = model.IsDDPCommented,
                DDPComment = model.DDPComment,
                IsAuditorCommented = model.IsAuditorCommented,
                AuditorComment = model.AuditorComment,
                IsAvailable = model.IsResolved,
                IsVerified = model.IsVerified,
                IsResolved = model.IsResolved,
                DateCreated = model.DateCreated,
                CreatorName = model.CreatorName,
                CreatorId = model.CreatorId,
                AvailabilityType = model.AvailabilityType.Value

            };
        }

        
    }
}
