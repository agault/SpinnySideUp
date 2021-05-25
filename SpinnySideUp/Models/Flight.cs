using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SpinnySideUp.Models
{
    public class Flight
    {
        [Key] //primary key
        public int FlightId { get; set; }
        public DateTime Date { get; set; }
        public string Duty { get; set; }
        public string Seat { get; set; }
        public string Mission { get; set; }
        public decimal Day { get; set; }
        public decimal Night { get; set; }
        public decimal NightGoggles { get; set; }
        public decimal NightSystems { get; set; }
        public decimal Weather { get; set; }
        public string AircraftId { get; set; }
        public string Notes { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    public class FlightDto
    {
        public int FlightId { get; set; }

        [DisplayName("Flight Date")]
        [Required(ErrorMessage = "Please Enter a Date DD/MM/YYYY")]
        public DateTime Date { get; set; }
        [DisplayName("Duty")]
        public string Duty { get; set; }
        [DisplayName("Seat")]
        public string Seat { get; set; }
        [DisplayName("Mission Type")]
        public string Mission { get; set; }
        [DisplayName("Day Hours")]
        public decimal Day { get; set; }
        [DisplayName("Night Hours")]
        public decimal Night { get; set; }
        [DisplayName("NightGoggle Hours")]
        public decimal NightGoggles { get; set; }
        [DisplayName("NightSystem Hours")]
        public decimal NightSystems { get; set; }
        [DisplayName("Weather Hours")]
        public decimal Weather { get; set; }
        [DisplayName("Aircraft Number")]
        public string AircraftId { get; set; }
        [DisplayName("Notes")]
        public string Notes { get; set; }
        public string UserId { get; set; }
    }
}