using System.ComponentModel.DataAnnotations;

namespace Authorization.Models
{
    public class ConsumerModel
    {
        [Key]
        public int ConsumerId { get; set; }

        public string ConsumerKey { get; set; }

        public string ConsumerSecret { get; set; }

        public string RedirectUrl { get; set; }
    }
}