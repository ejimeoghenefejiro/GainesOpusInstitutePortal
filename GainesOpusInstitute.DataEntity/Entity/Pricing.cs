using GainesOpusInstitute.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainesOpusInstitute.DataLayer.Entity
{
    public class Pricing : BaseEnities
    {
        public int PriceId { get; set; }
        public Decimal Amount { get; set; }
    }
}
