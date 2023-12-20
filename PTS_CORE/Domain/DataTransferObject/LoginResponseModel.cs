using PTS_CORE.Domain.DataTransferObject.RequestModel.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Message { get; set; }
        public bool IsSuccessful { get; set; }
        public ApplicationUserDto? ApplicationUserDto { get; set; }
    }
}
