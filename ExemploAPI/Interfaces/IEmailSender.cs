using ExemploAPI.Models;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace ExemploAPI.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(Mensagem message);
        Task SendEmailAsync(Mensagem message);
    }
}
