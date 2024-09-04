using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Domain
{
    public class Orders : BaseEntity
    {
        public Guid PubilsherId { get; set; }
        public DateTime ReturnDate { get; set; }
        public int? LateFee { get; set; }
    }
}
