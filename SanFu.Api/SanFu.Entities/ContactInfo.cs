using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SanFu.Entities
{
    [Table("ContactInfo")]
    public class ContactInfo : Entity
    {
        [Key]
        public override long Id { get; set; }

        public string Mobile { get; set; }

        public string HotwireTelephone { get; set; }

        public string Email { get; set; }

        public byte [] WeChat { get; set; }

        public string Address { get; set; }

        public string QQ { get; set; }

        public bool IsEnabled { get; set; }
    }
}
