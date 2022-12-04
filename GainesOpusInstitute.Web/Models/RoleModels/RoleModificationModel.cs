using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GainesOpusInstitute.Web.Models
{
    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public string[] AddIds { get; set; }
        public string[] DeleteIds { get; set; }

    }
}
