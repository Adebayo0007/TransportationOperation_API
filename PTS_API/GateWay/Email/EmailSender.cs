
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json.Linq;
using PTS_API.Helper;
using PTS_CORE.Domain.DataTransferObject.Email;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;

namespace PTS_API.GateWay.Email
{
    public class EmailSender : IEmailSender
    {
        public readonly IConfiguration _configuration;
        private readonly EmailSettings _emailSettings;
        public EmailSender(IConfiguration configuration, IOptions<EmailSettings> emailSettings)
        {
            _configuration = configuration;
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmail1(EmailRequestModel mail)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                message.To.Add(new MailboxAddress(mail.ReceiverName, mail.ReceiverEmail));
               message.Subject = mail.Subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = mail.Message,
                };

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                    client.Authenticate(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                    client.Send(message);
                    client.Disconnect(true);

                   
                   /* client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.ConnectAsync("smtp-relay.brevo.com", 587, SecureSocketOptions.StartTls).ConfigureAwait(false);
                    await client.AuthenticateAsync(new NetworkCredential("tijaniadebayoabdllahi@gmail.com", "Adebayo347!")).ConfigureAwait(false);
                    await client.SendAsync(message).ConfigureAwait(false);*/
                    return true;
                }
              
            }
            catch (SmtpCommandException ex)
            {
                Console.WriteLine($"SMTP command error: {ex.Message}");
                return false;
            }
            catch (SmtpProtocolException ex)
            {
                Console.WriteLine($"SMTP protocol error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendEmail(EmailRequestModel email)
        {
            Configuration.Default.ApiKey.Clear();
            var apiKey = _configuration.GetValue<string>("SendinblueAPIkey:ApiKey");
            Configuration.Default.ApiKey.Add("api-key", apiKey);

            var apiInstance = new TransactionalEmailsApi();
            string SenderName = "Pacesetter Transportation Services";
            string SenderEmail = email.SenderEmail;
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(email.ReceiverEmail, email.ReceiverName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>
            {
                smtpEmailTo
            };
            string BccName = email.ReceiverName;
            string BccEmail = email.ReceiverEmail;
            SendSmtpEmailBcc BccData = new SendSmtpEmailBcc(BccEmail, BccName);
            List<SendSmtpEmailBcc> Bcc = new List<SendSmtpEmailBcc>
            {
                BccData
            };
            string CcName = "Pacesetter Transportation Services";
            string CcEmail = _emailSettings.SenderEmail;
            SendSmtpEmailCc CcData = new SendSmtpEmailCc(CcEmail, CcName);
            List<SendSmtpEmailCc> Cc = new List<SendSmtpEmailCc>
            {
                CcData
            };
            string TextContent = null;
            string ReplyToName = "Pacesetter Transportation Services";
            string ReplyToEmail = _emailSettings.SenderEmail;
            SendSmtpEmailReplyTo ReplyTo = new SendSmtpEmailReplyTo(ReplyToEmail, ReplyToName);
            string stringInBase64 = "aGVsbG8gdGhpcyBpcyB0ZXN0";
            string AttachmentUrl = null;
            string AttachmentName = "Pacesetter.txt";
            byte[] Content = System.Convert.FromBase64String(stringInBase64);
            SendSmtpEmailAttachment AttachmentContent = new SendSmtpEmailAttachment(AttachmentUrl, Content, AttachmentName);
            List<SendSmtpEmailAttachment> Attachment = new List<SendSmtpEmailAttachment>
            {
                AttachmentContent
            };
            JObject Headers = new JObject
            {
                { "Some-Custom-Name", "unique-id-1234" }
            };
            long? TemplateId = null;
            JObject Params = new JObject
            {
                { "parameter", "My param value" },
                { "subject", "Dansnom" }
            };
            List<string> Tags = new List<string>
            {
                "mytag"
            };
            SendSmtpEmailTo1 smtpEmailTo1 = new SendSmtpEmailTo1(email.ReceiverEmail, email.ReceiverName);
            List<SendSmtpEmailTo1> To1 = new List<SendSmtpEmailTo1>
            {
                smtpEmailTo1
            };
            Dictionary<string, object> _parmas = new Dictionary<string, object>
            {
                { "params", Params }
            };
            SendSmtpEmailReplyTo1 ReplyTo1 = new SendSmtpEmailReplyTo1(ReplyToEmail, ReplyToName);
            SendSmtpEmailMessageVersions messageVersion = new SendSmtpEmailMessageVersions(To1, _parmas, Bcc, Cc, ReplyTo1, email.Subject);
            List<SendSmtpEmailMessageVersions> messageVersiopns = new List<SendSmtpEmailMessageVersions>
            {
                messageVersion
            };
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(Email, To, Bcc, Cc, email.Message, TextContent, email.Subject, ReplyTo, Attachment, Headers, TemplateId, Params, messageVersiopns, Tags);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                Debug.WriteLine(result.ToJson());
                return true;
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }


        }

        public async Task<bool> EmailValidaton(string email)
        {
            if (email is not null)
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                if (re.IsMatch(email))
                    return (true);
                else
                    return (false);
            }
            return false;

        }

        public async Task<bool> SendSMS(EmailRequestModel mail)
        {
            var apiKey = _configuration.GetValue<string>("SendinblueAPIkey:ApiKey");
            Configuration.Default.ApiKey.Add("api-key", apiKey);

            var apiInstance = new TransactionalSMSApi();
            string sender = mail.ReceiverName;
            string recipient = mail.Number;
            string content = mail.Message;
            SendTransacSms.TypeEnum type = SendTransacSms.TypeEnum.Transactional;
            string tag = "accountValidation";
            string webUrl = "http://requestb.in/173lyyx1";
            try
            {
                var sendTransacSms = new SendTransacSms(sender, recipient, content, type, tag, webUrl);
                SendSms result = apiInstance.SendTransacSms(sendTransacSms);
                Debug.WriteLine(result.ToJson());
                Console.WriteLine(result.ToJson());
                Console.ReadLine();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return false;
            }
        }
    }
}
