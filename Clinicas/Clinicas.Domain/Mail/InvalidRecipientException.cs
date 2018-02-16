using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Mail
{
    public class InvalidRecipientException : Exception
    {
        public InvalidRecipientException(string recipient)
            : base(string.Format("O destinatário '{0}' não é válido", recipient))
        {
        }
    }
}