using PTS_CORE.Domain.DataTransferObject.RequestModel.Vehicle;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Terminal;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface ITerminalService
    {
        Task<bool> Create(CreateTerminalRequestModel model);
        Task<BaseResponse<IEnumerable<TerminalResponseModel>>> Get(string terminalId);
        Task<BaseResponse<IEnumerable<TerminalResponseModel>>> GetAllTerminals(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<TerminalResponseModel>>> GetInactiveTerminals(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<TerminalResponseModel>>> SearchTerminal(string? keyword,CancellationToken cancellationToken = default);
        Task<bool> UpdateTerminal(UpdateTerminalRequestModel updateModel);
        Task<bool> ActivateTerminal(string terminalId);
        Task<bool> Delete(string terminalId);
    }
}
