using PTS_CORE.Domain.DataTransferObject.Email;

namespace PTS_BUSINESS.Email
{
    public interface IEmailSender1
    {
        Task<bool> SendEmail(EmailRequestModel email);
        Task<bool> EmailValidaton(string email);
    }
}
