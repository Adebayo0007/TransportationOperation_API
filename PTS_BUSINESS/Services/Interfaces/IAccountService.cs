using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Account;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponseModel> Login(LoginRequestModel loginRequestModel);
        Task<bool> UpdateUserAccount(UserUpdateModel updateModel);
        Task<bool> DeleteUserAccount(string userId);
        Task<bool> ActivateUserAccount(string userId);  
        Task<bool> CreateUserAccount(CreateUserRequestModel model);  
        Task<bool> CreateRole(CreateRoleRequestModel model);  
        Task<ApplicationUserDto> SendOTPForPasswordReset(string email);  
        Task<bool> UpdateRefreshToken(string id, string refreshToken);  
        Task<BaseResponse<bool>> ResetPassword(PasswordResetRequestModel model);  
        Task<BaseResponse<IEnumerable<ApplicationUserDto>>> GetAllUsers();
        Task<BaseResponse<IEnumerable<ApplicationUserDto>>> GetAllDeactivatedUsers();
        Task<BaseResponse<IEnumerable<ApplicationUserDto>>> SearchUsers(string? keyword);
        Task<BaseResponse<IEnumerable<ApplicationRoleDto>>> GetAllRoles();
        Task<BaseResponse<IEnumerable<ApplicationUserDto>>> Drivers();
        Task<double> NumberOfUser();
        Task<Dashboard> Dashboard();
        Task<BaseResponse<IEnumerable<ApplicationUserDto>>> GetUserByEmail(string email);
        Task<BaseResponse<IEnumerable<ApplicationUserDto>>> GetUserById(string Id);  
    }
}
