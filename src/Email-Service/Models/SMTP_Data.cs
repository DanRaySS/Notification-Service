using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Email_Service.Models
{
    public class SMTP_Data
    {
        public SmtpClient smtpClient; 
        public string sender;
    }
}