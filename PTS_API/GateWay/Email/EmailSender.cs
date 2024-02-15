
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
                    client.Authenticate("tijaniadebayoabdllahi@gmail.com", "Adebayo347!");
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
            Configuration.Default.ApiKey["api-key"] = apiKey;

            var apiInstance = new TransactionalEmailsApi();
            string SenderName = "Pacesetter Transport Service";
            string SenderEmail = email.SenderEmail;
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
            string ToEmail = email.ReceiverEmail;
            string ToName = email.ReceiverName;

            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(smtpEmailTo);

        /*    //Bcc the reciever also have the copy of the message but name do not visisble to othe reciepient of the email
            string BccName = email.ReceiverName;
            string BccEmail = email.ReceiverEmail;
            SendSmtpEmailBcc BccData = new SendSmtpEmailBcc(BccEmail, BccName);
            List<SendSmtpEmailBcc> Bcc = new List<SendSmtpEmailBcc>();
            Bcc.Add(BccData)*/;

            //CC Sender also recieve the email
           /* string CcName = "Wazobia agro Wxpress";
            string CcEmail = email.SenderEmail;
            SendSmtpEmailCc CcData = new SendSmtpEmailCc(CcEmail, CcName);
            List<SendSmtpEmailCc> Cc = new List<SendSmtpEmailCc>();
            Cc.Add(CcData);*/
            string HtmlContent = email.Message;
            string TextContent = HtmlContent;
           // string Subject = "{{params.subject}}";
           string ReplyToName = "Pacesetter Transport Services";
            string ReplyToEmail = email.SenderEmail;
            SendSmtpEmailReplyTo ReplyTo = new SendSmtpEmailReplyTo(ReplyToEmail, ReplyToName);
            /*string AttachmentUrl = null;
           string stringInBase64 = "aGVsbG8gdGhpcyBpcyB0ZXN0";
           byte[] Content = System.Convert.FromBase64String(stringInBase64);
           string AttachmentName = "test.txt";
           SendSmtpEmailAttachment AttachmentContent = new SendSmtpEmailAttachment(AttachmentUrl, Content, AttachmentName);
           List<SendSmtpEmailAttachment> Attachment = new List<SendSmtpEmailAttachment>();
           Attachment.Add(AttachmentContent);
           JObject Headers = new JObject();
           Headers.Add("Some-Custom-Name", "unique-id-1234");
           long? TemplateId = null;
           JObject Params = new JObject();

           //this is subtituted by the params.parameter
           Params.Add("parameter", email.Message);

           //this is subtituted by the params.subbject
           Params.Add("subject", email.Subject);
           List<string> Tags = new List<string>();
           Tags.Add("mytag");
           SendSmtpEmailTo1 smtpEmailTo1 = new SendSmtpEmailTo1(ToEmail, ToName);
           List<SendSmtpEmailTo1> To1 = new List<SendSmtpEmailTo1>();
           To1.Add(smtpEmailTo1);
           Dictionary<string, object> _parmas = new Dictionary<string, object>();
           _parmas.Add("params", Params);
           SendSmtpEmailReplyTo1 ReplyTo1 = new SendSmtpEmailReplyTo1(ReplyToEmail, ReplyToName);
           SendSmtpEmailMessageVersions messageVersion = new SendSmtpEmailMessageVersions(To1, _parmas, Bcc, Cc, ReplyTo1, Subject);
           List<SendSmtpEmailMessageVersions> messageVersiopns = new List<SendSmtpEmailMessageVersions>();
           messageVersiopns.Add(messageVersion);*/
            try
            {

                var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, TextContent, email.Subject,ReplyTo);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                return true;
            }
            catch (Exception ex)
            {
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
