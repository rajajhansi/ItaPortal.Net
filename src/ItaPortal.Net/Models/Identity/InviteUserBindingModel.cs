using System;
using System.ComponentModel.DataAnnotations;

namespace ItaPortal.Net.Models.Identity
{
    public class InviteUserBindingModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }        

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
       
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }        
    }
}