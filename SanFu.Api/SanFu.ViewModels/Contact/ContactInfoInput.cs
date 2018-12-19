using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanFu.ViewModels.Contact
{
    public class ContactInfoInput:BaseInput
    {
        [Required]
        [StringLength(11,MinimumLength =11)]
        public string Mobile { get; set; }

        [Required]
        [StringLength(100)]
        public string HotwireTelephone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public IFormFile WeChat { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        public long QQ { get; set; }

    }
}
