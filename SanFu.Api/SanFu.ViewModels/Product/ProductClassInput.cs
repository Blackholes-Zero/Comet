using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanFu.ViewModels.Product
{
    public class ProductClassInput:BaseInput
    {
        [Required]
        public string ClassName { get; set; }

    }
}
