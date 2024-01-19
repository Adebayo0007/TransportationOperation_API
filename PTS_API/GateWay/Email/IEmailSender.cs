using PTS_CORE.Domain.DataTransferObject.Email;

namespace PTS_API.GateWay.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(EmailRequestModel email);
        Task<bool> EmailValidaton(string email);
        Task<bool> SendEmail1(EmailRequestModel mail);
        Task<bool> SendSMS(EmailRequestModel mail);
    }
}
