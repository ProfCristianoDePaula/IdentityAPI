using MimeKit;

namespace ExemploAPI.Models
{
    public class Mensagem
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public IFormFileCollection Attachments { get; set; }

        public Mensagem(IEnumerable<string> to, string subject, string content, IFormFileCollection attachments)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(t => new MailboxAddress(t, t)));
            Subject = subject;
            Content = content;
            Attachments = attachments;
        }
    }
}
