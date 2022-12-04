using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GainesOpusInstitute.Web.Models
{
    public class UserModel
    {
        private string _currentClaim;

        [Required]
        public string Id { get; set; }
        public string Email { get; set; }
        public IList<ClaimDto> Claims { get; set; } = new List<ClaimDto>();
        public string SelectedClaim { get; set; }

        public string CurrentClaim
        {
            get => Claims.Count > 0 ? Claims[0].Type : string.Empty;
            set => _currentClaim = value;
        }
        public class ClaimDto
        {
            public string Type { get; set; }
        }

    }
}
