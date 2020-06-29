namespace ContactsBook.Infrastructure.Mailer
{
    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
    }
}
