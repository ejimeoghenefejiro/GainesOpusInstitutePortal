using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GainesOpusInstitute.DataEntity.Entity

{
   public class User: IdentityUser<int>
    {
        public virtual ICollection<IdentityUserRole<int>> Roles { get; set; } = new List<IdentityUserRole<int>>();

        public virtual ICollection<IdentityUserClaim<int>> Claims { get; set; } = new List<IdentityUserClaim<int>>();
        //public virtual ICollection<CourseExamRegistration> CourseExamRegr { get; set; }
        public string Surname { get; set; }
        public string Othernames { get; set; }
        public string Instrument { get; set; }
        public string ResidentialAddress { get; set; }
        public User()
        {

        }
        //[NotMapped]
        //public string FullName
        //{
        //    get
        //    {
        //        return this.Surname + " " + this.Othernames;
        //    }
        //}
    }
}
