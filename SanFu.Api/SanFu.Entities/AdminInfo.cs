using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SanFu.Entities
{
    [Table("AdminInfo")]
    public class AdminInfo :Entity
    {
        [Key]
        public  long Id { get; set; }

        public string LoginName { get; set; }

        public string PassWord { get; set; }


        public string Mobile { get; set; }

        public Guid SaltKey { get; set; }

        public bool IsDel { get; set; }

        public int State { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
