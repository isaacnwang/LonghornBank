using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LonghornBank.Models
{
    public enum Majors { Accounting, Business_Honors, Finance, International_Business, Management, MIS, Marketing, Supply_Chain_Management, STM }
    public class Member
    {
        [Display(Name = "Member ID")]
        public Int32 MemberID { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter a valid email address.")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        [Display(Name = "Phone Number")]
        [DisplayFormat(DataFormatString = "{0:###-###-####}", ApplyFormatInEditMode = true)]
        public string phoneNumber { get; set; }

        [Required(ErrorMessage = "Ok To Text is required.")]
        [Display(Name = "Ok To Text")]
        public bool OkToText { get; set; }

        [Required(ErrorMessage = "Major is required.")]
        [EnumDataType(typeof(Majors))]
        [Display(Name = "Major")]
        public Majors Major { get; set; }

        public string UserId { get; set; }

        public virtual List<Event>Events { get; set; }
    }
}