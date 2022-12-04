using GainesOpusInstitute.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainesOpusInstitute.DataLayer.Entity
{
   public class Profile : BaseEnities
    {
      
        
        public int UserId { get; set; }
        public string Surname { get; set; }
        public string Othernames { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ResidentialAddress { get; set; }



    }
}
