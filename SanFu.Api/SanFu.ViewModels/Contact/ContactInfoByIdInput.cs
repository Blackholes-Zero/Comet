using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanFu.ViewModels.Contact
{
    public class ContactInfoByIdInput:BaseInput
    {
        [Required]
        public long Id { get; set; }
    }
}
