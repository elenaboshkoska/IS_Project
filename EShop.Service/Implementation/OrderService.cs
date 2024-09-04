using EShop.Domain.Domain;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Orders> returnRepository;

        public OrderService(IRepository<Orders> returnRepository)
        {
            this.returnRepository = returnRepository;
        }

        public Orders CreateNewReturn(Orders r)
        {
            return returnRepository.Insert(r);
        }

        public Orders DeleteReturn(Guid id)
        {
            Orders r = returnRepository.Get(id);
            returnRepository.Delete(r);
            return r;
        }

        public Orders GetReturnById(Guid? id)
        {
            return returnRepository.Get(id);
        }

        public List<Orders> getReturns()
        {
            return returnRepository.GetAll().ToList();
        }

        public Orders UpdateReturn(Orders r)
        {
            return returnRepository.Update(r);
        }
    }
}
