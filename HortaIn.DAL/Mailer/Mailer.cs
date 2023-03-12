using System;
using System.Collections.Generic;
using System.Linq;
using HortaIn.DAL.Mailer.templates;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HortaIn.DAL.utils
{
    public class Mailer
    {
       static public void Send(string to,string secret){
        MailMessage message = new MailMessage();
    message.From = new MailAddress("luccazvps@gmail.com");
    message.To.Add(new MailAddress(to));
    message.Subject = "Test Email";
    message.Body = MakePasswordChangeTemplate.make(secret);

    SmtpClient smtpClient = new SmtpClient("smtp-relay.sendinblue.com", 587);
    smtpClient.Credentials = new System.Net.NetworkCredential("luccazvps@gmail.com", "dvj0gbHBP2EcpNmL");
    smtpClient.EnableSsl = true;
    smtpClient.Send(message);
    
    
       }
    }
}