﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ItaPortal.Net.Models.Identity
{
    public class UserModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email Id")]
        public string Email { get; set; }

        [Required]
        [Display(Name="User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password {get; set; }
        
        [Display(Name="Security Question")]
        public string SecurityQuestion { get; set; }
        
        [Display(Name="Security Answer")]
        public string SecurityAnswer { get; set; }
    }
}