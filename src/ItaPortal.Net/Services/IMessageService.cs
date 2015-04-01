using System;
using System.Threading.Tasks;

namespace ItaPortal.Net.Services
{
    public interface IMessageService
    {
        Task SendAsync(InvitationMessage invitationMessage);
    }
}