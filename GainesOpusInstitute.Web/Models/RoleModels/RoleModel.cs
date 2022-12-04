using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GainesOpusInstitute.Web.Models
{
    public class RoleModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Users { get; set; }
    }
}
