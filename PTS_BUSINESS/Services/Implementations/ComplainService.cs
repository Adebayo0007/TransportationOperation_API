using Microsoft.AspNetCore.Http;
using PTS_BUSINESS.Services.Interfaces;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Complain;
using PTS_CORE.Domain.Entities;
using PTS_DATA.Repository.Interfaces;
using System.Security.Claims;

namespace PTS_BUSINESS.Services.Implementations
{
    public class ComplainService : IComplainService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IComplainRepository _complainRepository;

        public ComplainService(IComplainRepository complainRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _complainRepository = complainRepository;
        }
        public async Task<bool> ActivateComplains(string id)
        {
            if (!string.IsNullOrWhiteSpace(id.Trim()))
            {
                try
                {
                    var complain = await _complainRepository.GetModelByIdAsync(id.Trim());

                    if (complain == null)
                        return false;

                    // Delete user properties based on your model
                    complain.IsDeleted = false;
                    complain.LastModified = DateTime.Now;
                    complain.IsDeleted = false;
                    complain.IsModified = true;
                    complain.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    complain.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;
                    await _complainRepository.UpdateAsync(complain);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        public async Task<bool> Create(CreateComplainRequestModel model)
        {
            if (model != null)
            {
                var complain = new Complain
                {
                    Subject = !string.IsNullOrWhiteSpace(model.Subject.Trim()) ? model.Subject.Trim() : null,
                    Content = !string.IsNullOrWhiteSpace(model.Content.Trim()) ? model.Content.Trim() : null,
                    CreatorId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null,
                    CreatorName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null,
                    DateCreated = DateTime.Now
                };

                var result = await _complainRepository.CreateAsync(complain);

                if (result == true)
                {
                    return true;
                }
                else
                {
                    // Handle errors if terminal creation fails
                    Console.WriteLine($"There is error creating complain");
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
                var complain = await _complainRepository.GetModelByIdAsync(id.Trim());
                complain.IsDeleted = true;
                await _complainRepository.DeleteAsync();
            }
        }

        public async Task<BaseResponse<IEnumerable<ComplainResponseModel>>> Get(string id)
        {
            try
            {

                if (id == null) { throw new NullReferenceException(); }
                var complain = await _complainRepository.GetByIdAsync(id.Trim());
                if (complain == null) return new BaseResponse<IEnumerable<ComplainResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"complain having  id {id} is not found"
                };
                else
                    return new BaseResponse<IEnumerable<ComplainResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"complain having id {id} retrieved successfully",
                        Data = complain.Select(x => ReturnComplainResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<ComplainResponseModel>>> GetAllComplains(CancellationToken cancellationToken = default)
        {
            try
            {
                var complains = await _complainRepository.GetAllAsync(cancellationToken);
                if (complains == null) return new BaseResponse<IEnumerable<ComplainResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"complain are not found"
                };
                else
                    return new BaseResponse<IEnumerable<ComplainResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"complain are retrieved successfully",
                        Data = complains.Select(x => ReturnComplainResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<ComplainResponseModel>>> GetInactiveComplains(CancellationToken cancellationToken = default)
        {
            try
            {
                var complains = await _complainRepository.InactiveComplains(cancellationToken);
                if (complains == null) return new BaseResponse<IEnumerable<ComplainResponseModel>>
                {
                    IsSuccess = false,
                    Message = $"complain are not found"
                };
                else
                    return new BaseResponse<IEnumerable<ComplainResponseModel>>
                    {
                        IsSuccess = true,
                        Message = $"complain are retrieved successfully",
                        Data = complains.Select(x => ReturnComplainResponseModel(x)).ToList()
                    };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponse<IEnumerable<ComplainResponseModel>>> SearchComplains(string? keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                if (keyword != null)
                {
                    var complains = await _complainRepository.SearchComplains(keyword,cancellationToken);
                    if (complains == null) return new BaseResponse<IEnumerable<ComplainResponseModel>>
                    {
                        IsSuccess = false,
                        Message = $"complain are not found"
                    };
                    else
                        return new BaseResponse<IEnumerable<ComplainResponseModel>>
                        {
                            IsSuccess = true,
                            Message = $"complain are retrieved successfully",
                            Data = complains.Select(x => ReturnComplainResponseModel(x)).ToList()
                        };
                }
                else return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Update(UpdateComplainRequestModel updateModel)
        {
            if (updateModel != null)
            {
                try
                {
                    var complains = await _complainRepository.GetModelByIdAsync(updateModel.Id.Trim());
                    if (complains == null)
                        return false;

                    // Update user properties based on your model
                    complains.Subject = !string.IsNullOrWhiteSpace(updateModel.Subject.Trim()) ? updateModel.Subject.Trim() : complains.Subject;
                    complains.Content = !string.IsNullOrWhiteSpace(updateModel.Content.Trim()) ? updateModel.Content.Trim() : complains.Content;
                    complains.IsModified = true;
                    complains.LastModified = DateTime.Now;
                    complains.ModifierId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
                    complains.ModifierName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? null;

                    // Update other properties as needed
                    await _complainRepository.UpdateAsync(complains);
                    return true;
                }
                catch (Exception ex)
                { throw; }
            }
            else return false;
        }

        private ComplainResponseModel ReturnComplainResponseModel(Complain model)
        {
            return new ComplainResponseModel
            {
                Id = model.Id,
                Subject = model.Subject,
                Content =  model.Content,
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
