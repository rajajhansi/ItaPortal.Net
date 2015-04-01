using System;
using System.Net;
using System.Net.Mail;

namespace ItaPortal.Net.Services
{
    public class EmailMessageServiceOptions
    {
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
        public int Port { get; set; } 
        public NetworkCredential Credential { get; set; }
    }
}