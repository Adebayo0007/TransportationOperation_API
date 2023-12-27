using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Account;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Employee;
using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<bool> Create(CreateEmployeeRequestModel model);
        Task<BaseResponse<IEnumerable<EmployeeResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<EmployeeResponseModel>>> GetAllEmployees();
    }
}
