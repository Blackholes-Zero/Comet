using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SanFu.Entities
{
    [Table("ProductClass")]
    public class ProductClass : Entity
    {
        [Key]
        public override long Id { get; set; }

        public long ClassID { get; set; }

        public string ClassName { get; set; }

        public bool IsEnabled { get; set; }
    }

}
