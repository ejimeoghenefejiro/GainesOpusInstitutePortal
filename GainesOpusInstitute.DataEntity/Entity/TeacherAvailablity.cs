using GainesOpusInstitute.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainesOpusInstitute.DataLayer.Entity
{
    public class TeacherAvailablity : BaseEnities
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool? Status { get; set; }
        public string DayName { get; set; }
    }
}
