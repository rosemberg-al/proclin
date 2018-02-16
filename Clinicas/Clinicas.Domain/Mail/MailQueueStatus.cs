using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Mail
{
    public class MailQueueStatus
    {
        public int MailQueueId { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }
    }
}