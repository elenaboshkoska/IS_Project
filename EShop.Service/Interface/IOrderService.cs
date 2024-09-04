using EShop.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Interface
{
    public interface IOrderService
    {
        public List<Orders> getReturns();
        public Orders GetReturnById(Guid? id);
        public Orders CreateNewReturn(Orders r);
        public Orders DeleteReturn(Guid id);

        public Orders UpdateReturn(Orders r);
    }
}
