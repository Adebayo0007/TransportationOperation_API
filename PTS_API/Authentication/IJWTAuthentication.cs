using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Account;
using PTS_CORE.Domain.Entities;
using System.Security.Claims;

namespace PTS_API.Authentication
{
    public interface IJWTAuthentication
    {
        string GenerateToken(ApplicationUserDto model);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
