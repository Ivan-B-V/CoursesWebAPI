using MimeKit;

namespace CoursesWebAPI.Shared.Models;

public sealed class MailMessage
{
    public ICollection<MailboxAddress> Addresses { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }

    public MailMessage(IEnumerable<string> addresses, string subject, string content)
    {
        Addresses = new List<MailboxAddress>(addresses.Select(a => new MailboxAddress(a, a)));
        Subject = subject;
        Content = content;
    }
}
