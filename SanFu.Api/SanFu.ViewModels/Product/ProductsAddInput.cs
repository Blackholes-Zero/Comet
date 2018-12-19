using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanFu.ViewModels.Product
{
    public class ProductsAddInput:BaseInput
    {
        [Required]
        public long ProductClassID { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal? Price { get; set; }

        [Required]
        public string Brief { get; set; }

        [Required]
        public string Details { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        public int Sort { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Specifications { get; set; }
    }
}
