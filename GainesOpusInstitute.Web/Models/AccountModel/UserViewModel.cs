
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GainesOpusInstitute.Web.Models
{
   public class UserViewModel
    {
        public List<UserModel> Users { get; set; }
        //[Required]
        public string Surname { get; set; }
        //[Required]
        public string Othernames { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string CreatedBy { get; set; }


    }
}
