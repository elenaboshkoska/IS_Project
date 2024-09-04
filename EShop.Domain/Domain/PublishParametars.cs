using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Domain
{
    public class PublishParametars : BaseEntity
    {
        public int MinimumRentDays { get; set; } = 5;
        public int? Discount { get; set; }
    }
}
