

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PTS_CORE.Domain.DataTransferObject.Email;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;

namespace PTS_BUSINESS.Email
{
    public class EmailSender1 : IEmailSender1
    {
        public readonly IConfiguration _configuration;
        public EmailSender1(IConfiguration configuration)
        {
            _configuration = configuration;
        }


      

        public async Task<bool> SendEmail(EmailRequestModel email)
        {
            Configuration.Default.ApiKey.Clear();
            Configuration.Default.ApiKey["api-key"] = "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";

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
            Bcc.Add(BccData)*/

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

    }
}
