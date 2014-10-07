using System;
using System.ComponentModel.DataAnnotations;

namespace Authorization.Models
{
    public class AuthenticationTicketModel
    {
        public AuthenticationTicketModel()
        {
        }

        public AuthenticationTicketModel(Guid contextToken)
        {
            ContextToken = contextToken;
        }

        [Key]
        public Guid ContextToken { get; set; }

        public string AuthenticationTicket { get; set; }
    }
}