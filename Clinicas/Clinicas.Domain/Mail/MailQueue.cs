using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Mail
{
    public class MailQueue
    {
        public int Id { get; private set; }
        public int TemplateId { get; private set; }
        public int Priority { get; private set; }
        public DateTime Date { get; private set; }
        public DateTime? Scheduled { get; private set; }
        public string Status { get; private set; }

        public string To { get; private set; }
        public string Parameters { get; private set; }

        public virtual MailTemplate Template { get; private set; }

        protected MailQueue() { }

        public MailQueue(MailTemplate template, string to, string parameters)
        {
            ChangeParameters(parameters);
            ChangeToRecipient(to);
            ChangeTemplate(template);
            SetPriority(MailPriority.Low);

            Date = DateTime.Now;
            Status = MailStatus.Pending;
        }

        public void ChangeToRecipient(string to)
        {
            if (string.IsNullOrEmpty(to))
                throw new ArgumentNullException("to");
            if (!IsValidEmail(to))
                throw new InvalidRecipientException(to);
            To = to;
        }

        public void ChangeParameters(string paramsContent)
        {
            if (string.IsNullOrEmpty(paramsContent))
                throw new ArgumentNullException(paramsContent);
            this.Parameters = paramsContent;
        }

        public void ChangeTemplate(MailTemplate template)
        {
            if (template.Id == 0)
                throw new ArgumentException("Template ID deve ser válido");
            this.Template = template;
        }

        public void Schedule(DateTime dateToDeliver)
        {
            if (dateToDeliver < DateTime.Now)
                throw new ArgumentException("Data de agendamento deve ser maior que a data atual");
            this.Scheduled = dateToDeliver;
            ChangeStatus(MailStatus.Scheduled);
        }

        public void SetStatus(string status)
        {
            this.Status = status;
        }

        public void SetPriority(MailPriority priority)
        {
            this.Priority = (int)priority;
        }

        public void ChangeStatus(string status)
        {
            if (string.IsNullOrEmpty(status))
                throw new ArgumentNullException("status");
            if (status.Length > 1)
                throw new ArgumentException("Status deve ter no máximo 1 caractere");
            this.Status = status;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }

    public static class MailStatus
    {
        public static string Pending = "P";
        public static string Sent = "E";
        public static string Scheduled = "A";
        public static string Error = "R";
        public static string Returned = "T";
    }

    public enum MailPriority
    {
        Low = 1,
        Moderate = 2,
        High = 3
    }
}
