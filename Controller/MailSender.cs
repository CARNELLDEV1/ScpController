using System;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Specialized;

namespace ScpController.Controller
{
    public static class MailSender
    {
        public static void SendNotification(string jobname)
        {
            string carnellSMTP = ConfigurationManager.AppSettings["CarnellSMTP"];
            SmtpClient SmtpServer = new SmtpClient(carnellSMTP);
            string time = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("carnellgps@carnellgroup.co.uk");
            mail.To.Add("hao.ye@carnellgroup.co.uk");
            mail.Subject = "SMARTscan Automation Email - Status: Data Preprocessing Ready";
            mail.Body = $"<h3>This is an automated email, please do not reply.</h3><p>This email to is notify that your job has been uploaded to the server and are waiting to be processed in Carnell Office.</p><p> Date: {time} </p><p> Job Number: {jobname} </p><p> Thank you for uploading the data!</p> ";
            mail.IsBodyHtml = true;

            SmtpServer.Port = 25;
            SmtpServer.Credentials = new System.Net.NetworkCredential("hao.ye@carnellgroup.co.uk", "Carnell2019");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }

    }
}
