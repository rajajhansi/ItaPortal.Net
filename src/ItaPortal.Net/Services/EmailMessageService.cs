using Microsoft.AspNet.Mvc;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.OptionsModel;
using SendGrid;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ItaPortal.Net.Services
{
    public class EmailMessageService : IMessageService
    {
        [Activate]
        protected IOptions<EmailMessageServiceOptions> EmailMessageServiceOptions { get; set; }
        public async Task SendAsync(InvitationMessage invitationMessage)
        {           
            IConfiguration configuration = new Configuration().
                AddJsonFile("config.json");
#if SENDGRID
            // Create the email object first, then add the properties
            var invite = new SendGridMessage();

            // define the email and name of the sender
            invite.From = new MailAddress("donot-reply@itaportal.net", "ITA Support Account");

            // set where are we sending the email
            invite.AddTo(invitationMessage.Destination);
            invite.Subject = invitationMessage.Subject;

            // Make sure all your messages are formatted as HTML
            invite.Html = invitationMessage.Body;

            // Create Credentials, specifiying the SendGrid Username and password            
            var credentials = new NetworkCredential(configuration.Get("SendGrid:Credential:Username"),
                configuration.Get("SendGrid:Credential:Password"));

            // Create web transport for sending Email
            var transportWeb = new Web(credentials);

            // Send the mail
            await transportWeb.DeliverAsync(invite);
#endif
            using (SmtpClient smtp = new SmtpClient())
            {                
                smtp.DeliveryMethod = (SmtpDeliveryMethod)Enum.Parse(typeof(SmtpDeliveryMethod), configuration.Get("Smtp:DeliveryMethod"));
                smtp.UseDefaultCredentials = bool.Parse(configuration.Get("Smtp:UseDefaultCredentials"));
                smtp.EnableSsl = bool.Parse(configuration.Get("Smtp:EnableSsl"));
                smtp.Host = configuration.Get("Smtp:Host");
                smtp.Port = int.Parse(configuration.Get("Smtp:Port")); 
                smtp.Credentials = new NetworkCredential(configuration.Get("Smtp:Credential:Username"),                        
                    configuration.Get("Smtp:Credential:Password")); 
                // send the email
                var to = new MailAddress(invitationMessage.Destination);
                // define the email and name of the sender
                var from = new MailAddress("donot-reply@itaportal.net", "ITA Support Account");               
                var msg = new MailMessage();

                msg.To.Add(to);
                msg.From = from;
                msg.IsBodyHtml = true;
                msg.Subject = invitationMessage.Subject;
                msg.Body = invitationMessage.Body;

                await smtp.SendMailAsync(msg);
            }
        }
    }
}