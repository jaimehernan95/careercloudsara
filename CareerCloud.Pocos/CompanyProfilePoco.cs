using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace CareerCloud.Pocos
{
    [Table("Company_Profiles")]
    public class CompanyProfilePoco:IPoco
    {
        [Key]
        public Guid Id { get; set; }
        [Column("Registration_Date")]
        public DateTime RegistrationDate { get; set; }
        [Column("Company_Website")]
        public String CompanyWebsite { get; set; }
        [Column("Contact_Phone")]
        public String ContactPhone { get; set; }
        [Column("Contact_Name")]
        public String ContactName { get; set; }
        [Column("Company_Logo")]
        public Byte[] CompanyLogo { get; set; }
        [Column("Time_Stamp")]
        public Byte[] TimeStamp { get; set; }
    }
}
