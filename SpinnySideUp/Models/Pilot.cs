using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SpinnySideUp.Models
{
    public class Pilot
    {
        [Key]//primary key
        public int PilotId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public string Rank { get; set; }
        public Boolean Pc { get; set; }
        public Boolean Pi { get; set; }
        public Boolean Sp { get; set; }
        public Boolean Ip { get; set; }
        public string Fac { get; set; }
        public ICollection<Flight> Flight { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    public class PilotDto
    {
        public int PilotId { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("DOB")]
        public DateTime DateBirth { get; set; }
        [DisplayName("Rank")]
        public string Rank { get; set; }
        [DisplayName("Pilot in Command")]
        public Boolean Pc { get; set; }
        [DisplayName("Pilot")]
        public Boolean Pi { get; set; }
        [DisplayName("SP")]
        public Boolean Sp { get; set; }
        [DisplayName("Instructor Pilot")]
        public Boolean Ip { get; set; }
        public string Fac { get; set; }
        public string UserId { get; set; }
    }
}