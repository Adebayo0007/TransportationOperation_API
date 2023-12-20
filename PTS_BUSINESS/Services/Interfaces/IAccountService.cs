using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResponseModel> Login(LoginRequestModel loginRequestModel);
    }
}
