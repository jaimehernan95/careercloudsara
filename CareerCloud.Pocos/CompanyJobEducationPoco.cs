using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace CareerCloud.Pocos
{
    [Table("Company_Job_Educations")]
    public class CompanyJobEducationPoco:IPoco
    {
        [Key]
        public Guid Id { get; set; }
        [Column("Job")]
        public Guid Job { get; set; }
        [Column("Major")]
        public String Major { get; set; }
        [Column("Importance")]
        public Int16 Importance { get; set; }
        [Column("Time_Stamp")]
        public Byte[] TimeStamp { get; set; }



    }
}
