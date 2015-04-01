using System;

namespace ItaPortal.Net.Services
{
    public class InvitationMessage
    {
        public virtual string Body { get; set; }
        public virtual string Destination { get; set; }
        public virtual string Subject { get; set; }
    }
}