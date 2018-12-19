using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanFu.ViewModels.Account
{
    public class RegisterInput:BaseInput
    {
        
        [StringLength(50, MinimumLength = 5)]
        [Required]
        public string LoginName { get; set; }

        [StringLength(100, MinimumLength = 8)]
        [Required]
        public string PassWord { get; set; }

        [StringLength(11, MinimumLength = 11)]
        [Required]
        public string Mobile { get; set; }

        [StringLength(6, MinimumLength = 6)]
        public string CodeNum { get; set; }
    }
}
