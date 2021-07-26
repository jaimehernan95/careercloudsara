using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CareerCloud.Pocos
{
    [Table("System_Country_Codes")]
    public class SystemCountryCodePoco 
    {
        [Key]
        public String Code { get; set; }
        [Column("Name")]
        public String Name { get; set; }
    }
}
