using System;
using System.Net.Mail;
using System.Collections.Specialized;

namespace ScpController.Controller
{
    class EmailController
    {
        public String SmtpClient { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public int Port { get; set; }
        public StringCollection EmailCollection;
        MailMessage _mail;
        SmtpClient SmtpServer;

        public EmailController(string smtpClient, string smtpUser, string smtpPassword, int port)
        {
            SmtpClient = smtpClient;
            SmtpUser = smtpUser;
            SmtpPassword = smtpPassword;
            Port = port;

            SmtpServer = new SmtpClient(SmtpClient)
            {
                Port = port,
                Credentials = new System.Net.NetworkCredential(smtpUser, smtpPassword),
                EnableSsl = true
            };

            _mail = new MailMessage();

            //EmailCollection = Properties.Settings.Default.Uploaders;
        }

        public void SendEmail(string projectName)
        {
            try
            {
                _mail.From = new MailAddress(this.SmtpUser);

                foreach (var mailitem in EmailCollection)
                {
                    _mail.To.Add(mailitem);
                    _mail.Subject = "SmartScan Project" + projectName + " Created Notification";
                    _mail.Body = "This is a notification that your SmartScan project is detected " +
                        "from WinSCP and has been download to process!/n/t The current time is" + DateTime.Now.ToLongTimeString();
                }

                //SmtpServer.Send(_mail);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
