using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GxShared.Identity
{
    public class EmailConfiguration
    {
        public string From { get; set; } = string.Empty;
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class InviteEmailRequest
    {
        [EmailAddress]
        public string To { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Subject { get; set; } = "You're invited!";
        public bool InEmailButton { get; set; } = false;
        public string Message { get; set; } = "You’ve been invited to register.";
        public string InviteLink { get; set; } = string.Empty;
        public string? QrBase64 { get; set; }
        public string? LogoBase64 { get; set; }
        public string QrImagePath { get; set; } = string.Empty;
        public string LogoImagePath { get; set; } = string.Empty;
    }
}
