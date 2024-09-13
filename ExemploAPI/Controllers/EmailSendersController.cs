using ExemploAPI.Interfaces;
using ExemploAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace ExemploAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EmailSender : IEmailSender
    {
        // Criamos a Injecao de Dependencia
        private readonly EmailConfig _EmailConfig;
        public EmailSender(EmailConfig emailConfig)
        {
            _EmailConfig = emailConfig;
        }

        // Implementação do Metodo da Interface

        public void SendEmail(Mensagem mensagem)
        {
            var emailMessage = CreateEmailMessage(mensagem);
            Send(emailMessage);
        }

        [HttpPost]
        // Implementação do Metodo da Interface
        public async Task SendEmailAsync(Mensagem mensagem)
        {
            var emailMessage = CreateEmailMessage(mensagem);
            await SendAsync(emailMessage);
        }

        // Implementação da Tarefa de Envio de Email Assincrona
        private async Task SendAsync(MimeMessage emailMessage)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_EmailConfig.SmtpServer, _EmailConfig.Port, true);
                    await client.AuthenticateAsync(_EmailConfig.UserName, _EmailConfig.Password);
                    await client.SendAsync(emailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        // Implementação da Tarefa de Envio de Email Sincrona
        private void Send(MimeMessage emailMessage)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(_EmailConfig.SmtpServer, _EmailConfig.Port, true);
                    client.Authenticate(_EmailConfig.UserName, _EmailConfig.Password);
                    client.Send(emailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        // Metodo Responsavel por "Compor" o Email
        private MimeMessage CreateEmailMessage(Mensagem mensagem)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Prof. Cristiano de Paula", _EmailConfig.From));
            emailMessage.To.AddRange(mensagem.To);
            emailMessage.Subject = mensagem.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = mensagem.Content
            };

            return emailMessage;
        }


    }
}


