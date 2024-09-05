using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Domain
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int yearOfRelease { get; set; }

        public Authors? Author { get; set; }
        public Guid? AuthorId { get; set; }

        public string genre { get; set; }

        public bool? IsAvailable { get; set; }
        public int Price { get; set; }
        public ICollection<Publishers>? Publishers { get; set; }
    }
}
