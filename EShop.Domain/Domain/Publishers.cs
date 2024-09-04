using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Domain
{
    public class Publishers : BaseEntity
    {
        public Guid BookId { get; set; }
        public string? AuthorsId { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public int RentAmount { get; set; }
        public Book? Book { get; set; }
        public Authors? Author { get; set; }
        public Orders? Order { get; set; }
    }
}
