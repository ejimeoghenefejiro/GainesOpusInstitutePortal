using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GainesOpusInstitute.Web.Models
{
    public class RegisterViewModel
    {

        [Required]
        public string Username { get; set; }
        [Required]
        public string Instrument { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Othernames { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        //[Required]
        public string SelectedRole { get; set; }

        public List<SelectListItem> RoleSelect { get; set; }  

        public RegisterViewModel()
        {
            RoleSelect = new List<SelectListItem> 
            {
                //new SelectListItem("Select role", string.Empty, true, true),
                //new SelectListItem(Claims.Admin, Claims.Admin),
                new SelectListItem(Claims.User, Claims.User),
            };

        }
    }
}
