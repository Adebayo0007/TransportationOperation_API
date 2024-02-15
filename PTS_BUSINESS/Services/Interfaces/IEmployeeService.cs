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
        Task<BaseResponse<IEnumerable<EmployeeResponseModel>>> GetByEmail(string email);
        Task<BaseResponse<IEnumerable<EmployeeResponseModel>>> GetAllEmployees(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<EmployeeResponseModel>>> EmployeeBirthdayNotification(CancellationToken cancellationToken = default);
        Task EmployeeBirthdayForToday();
        Task<BaseResponse<IEnumerable<EmployeeResponseModel>>> GetInactiveEmployees(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<EmployeeResponseModel>>> SearchEmployee(string? keyword,CancellationToken cancellationToken = default);
        Task<bool> UpdateEmployeeAccount(UpdateEmployeeRequestModel updateModel);
        Task<bool> ActivateEmployee(string employeeId);
        Task Delete(string id);
    }
}
