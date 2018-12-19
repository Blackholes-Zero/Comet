using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SanFu.Entities
{
    [Table("Products")]
    public class Products:Entity
    {
        [Key]
        public  override long Id { get; set; }

        public long ProductClassID { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Brief { get; set; }

        public string Details { get; set; }

        public string ImageUrl { get; set; }

        public int Sort { get; set; }

        public string Model { get; set; }

        public string Specifications { get; set; }
    }
}
