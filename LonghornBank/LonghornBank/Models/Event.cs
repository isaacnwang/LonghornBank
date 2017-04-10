using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LonghornBank.Models
{
    public class Event
    {
        [Display(Name = "Event ID")]
        public Int32 EventID { get; set; }

        [Required(ErrorMessage = "Event Title is required.")]
        [Display(Name = "Event Title")]
        public string EventTitle { get; set; }

        [Required(ErrorMessage = "Event Date is required.")]
        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Event Location is required.")]
        [Display(Name = "Event Location")]
        public string EventLocation { get; set; }

        [Required(ErrorMessage = "Members Only is required.")]
        [Display(Name = "Members Only?")]
        public bool MembersOnly { get; set; }

        //navigational properties
        public virtual List<Member>Members { get; set; }
    }
}